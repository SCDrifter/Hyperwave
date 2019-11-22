using Hyperwave.UserCache;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hyperwave.ViewModel;
using System.Text.RegularExpressions;
using Hyperwave.Auth;

namespace Hyperwave.Controller
{
    public class EveMailClient : IDisposable
    {
        public CommandRouter Commands { get; private set; } = new CommandRouter();

        DB.AccountDatabase mAccountDB = new DB.AccountDatabase();
        List<Account> mAccounts = new List<Account>();
        ObservableCollection<ViewAccount> mViewAccounts = new ObservableCollection<ViewAccount>();
        ObservableCollection<ViewAccount> mSendAccounts = new ObservableCollection<ViewAccount>();

        List<DraftMessageSource> mDrafts = new List<DraftMessageSource>();

        Dictionary<Guid, IDraftWindow> mDraftWindows = new Dictionary<Guid, IDraftWindow>();

        Dictionary<long,EntityInfo> mLookups = new Dictionary<long, EntityInfo>();

        internal List<Account> Accounts { get { return mAccounts; } }
        internal List<DraftMessageSource> Drafts { get { return mDrafts; } }

        public MailView MailView { get; private set; } = new MailView();

        public UserInfoCache InfoCache { get; private set; }

        public void Init()
        {
            InfoCache = new UserInfoCache(this);
            mAccountDB.Load();
            

            foreach (var i in mAccountDB.Accounts)
            {
                Account account = new Account(this, i);
                mAccounts.Add(account);
                mViewAccounts.Add(account.ViewAccount);
                if (account.DBAccount.Permissions.HasFlag(Auth.AccessFlag.MailSend))
                    mSendAccounts.Add(account.ViewAccount);
            }

            List<DraftMessageSource> loaded_drafts = new List<DraftMessageSource>();

            foreach (var i in mAccountDB.Drafts)
            {
                DraftMessageSource source = new DraftMessageSource(this, i);

                if (source.Account == null)
                    continue;

                mDrafts.Add(source);
                loaded_drafts.Add(source);
            }

            TaskChainHandler load_draft_ids = async delegate ()
            {
                foreach (var i in loaded_drafts)
                {
                    await EntityLookupAsync.LookupIDs(i.Recipients);
                }
            };

            AddTaskToChain(load_draft_ids);
        }

        
        public event EventHandler<AccountNotificationEventArgs> AccountNotification;
        public event EventHandler ControllerActive;
        public event EventHandler ControllerIdle;

        public void Dispose()
        {
            if(Commands != null)
            {
                Commands.Dispose();
                mAccountDB.SaveChanges();
                mAccountDB.Dispose();
            }
        }

        struct TaskItem
        {
            public TaskChainHandler Handler;
            public TaskCompletionSource<object> Completion;
        }

        delegate Task TaskChainHandler();

        Task mTaskChainProcessor = null;
        Queue<TaskItem> mTaskChain = new Queue<TaskItem>();

        
        

        async Task TaskChainProcessor()
        {
            try
            {
                if (ControllerActive != null)
                    ControllerActive(this, new EventArgs());

                TaskItem chain;
                while (mTaskChain.Count > 0)
                {
                    chain = mTaskChain.Dequeue();
                    await chain.Handler();

                    if (chain.Completion != null)
                        chain.Completion.SetResult(null);
                }
            }
            finally
            {
                if (ControllerIdle != null)
                    ControllerIdle(this, new EventArgs());
            }
        }

        void AddTaskToChain(TaskChainHandler task,TaskCompletionSource<object> completion_token = null)
        {

            mTaskChain.Enqueue(new TaskItem()
            {
                Handler = task,
                Completion = completion_token
            });
            if (mTaskChainProcessor == null || mTaskChainProcessor.IsFaulted || mTaskChainProcessor.IsCompleted)
                mTaskChainProcessor = TaskChainProcessor();
        }

        internal DB.AccountDatabase AccountsDB { get { return mAccountDB; } }

        public async Task AddAccountAsync(Auth.TokenInfo token, Auth.CharacterInfo charinfo, Auth.AccessFlag permissions)
        {
            DB.Account dbaccount = await mAccountDB.AddAccountAsync(token, charinfo, permissions);
            Account account = new Account(this, dbaccount);
            mAccounts.Add(account);
            mViewAccounts.Add(account.ViewAccount);

            OnAccountAction(true, account);
        }

        async Task RemoveAccountInternal(ISourceInfo source)
        {
            Account account = (Account)source.AccountSource;
            await mAccountDB.RemoveAccountAsync(account.DBAccount);
            mAccounts.Remove(account);
            mViewAccounts.Remove(account.ViewAccount);
            mSendAccounts.Remove(account.ViewAccount);

            if (MailView.Source == source)
                await SetMailView(null);

        }

        public void RemoveAccount(ISourceInfo source)
        {
            TaskChainHandler handler = delegate ()
            {
                return RemoveAccountInternal(source);
            };

            AddTaskToChain(handler);
        }

        public async Task RemoveAccountWait(ISourceInfo source)
        {
            TaskChainHandler handler = delegate ()
            {
                return RemoveAccountInternal(source);
            };
            TaskCompletionSource<object> waitfor = new TaskCompletionSource<object>();
            AddTaskToChain(handler,waitfor);

            await waitfor.Task;
        }

        public void UpdateAccounts(bool silent)
        {
            TaskChainHandler handler = delegate ()
            {
                return UpdateAccountsAsync(silent);
            };

            AddTaskToChain(handler);
        }

        public async Task UpdateAccountsWait(bool silent)
        {
            TaskChainHandler handler = delegate ()
            {
                return UpdateAccountsAsync(silent);
            };

            TaskCompletionSource<object> waitfor = new TaskCompletionSource<object>();
            AddTaskToChain(handler, waitfor);

            await waitfor.Task;
        }

        async Task UpdateAccountsAsync(bool silent)
        {
            if (!silent && UpdateAccountOperationStarted != null)
                UpdateAccountOperationStarted(this, new EventArgs());
            
            List<Task> tasks = new List<Task>();

            foreach(var i in mAccounts)
            {
                tasks.Add(i.UpdateAccount());
            }

            await Task.WhenAll(tasks);

            int count = 0;

            foreach (var i in mAccounts)
            {
                count += i.UnreadCount;
            }

            await mAccountDB.SaveChangesAsync();

            if (AccountNotification != null)
                AccountNotification(this, new AccountNotificationEventArgs() { MailCount = count });
        }

        internal void UpdateAccountDraftCounts()
        {
            foreach (var i in mAccounts)
            {
                i.DraftsLabel.UnreadCount = 0;
            }

            foreach (var i in mDrafts)
            {
                var source = i.Account as ISourceInfo;
                var account = source.AccountSource as Account;
                account.DraftsLabel.UnreadCount++;
            }

            foreach (var i in mAccounts)
            {
                i.ViewAccount.Sync(i);
            }
        }

        public void UpdateAccount(IAccountSource account, bool silent)
        {
            TaskChainHandler handler = delegate ()
            {
                return UpdateAccountAsync(account,silent);
            };

            AddTaskToChain(handler);
        }

        public async Task UpdateAccountWait(IAccountSource account, bool silent)
        {
            TaskChainHandler handler = delegate ()
            {
                return UpdateAccountAsync(account, silent);
            };

            TaskCompletionSource<object> waitfor = new TaskCompletionSource<object>();
            AddTaskToChain(handler, waitfor);

            await waitfor.Task;
        }

        async Task UpdateAccountAsync(IAccountSource account,bool silent)
        {
            Account raccount = account as Account;

            raccount.ViewAccount.ShowSpinner = !silent;
            await raccount.UpdateAccount();

            await mAccountDB.SaveChangesAsync();
        }

        public void AddLabel(IAccountSource source, string label)
        {
            TaskChainHandler handler = delegate ()
            {
                return AddLabelAsync(source,label);
            };

            AddTaskToChain(handler);
        }

        public async Task AddLabelWait(IAccountSource source, string label)
        {
            TaskChainHandler handler = delegate ()
            {
                return AddLabelAsync(source, label);
            };

            TaskCompletionSource<object> waitfor = new TaskCompletionSource<object>();
            AddTaskToChain(handler, waitfor);

            await waitfor.Task;
        }

        async Task AddLabelAsync(IAccountSource source,string label)
        {
            var account = source as Account;

            await account.AddLabel(label);
        }

        public void RemoveLabel(ILabelSource label)
        {
            TaskChainHandler handler = delegate ()
            {
                return RemoveLabelAsync(label);
            };

            AddTaskToChain(handler);
        }

        public async Task RemoveLabelWait(ILabelSource label)
        {
            TaskChainHandler handler = delegate ()
            {
                return RemoveLabelAsync(label);
            };

            TaskCompletionSource<object> waitfor = new TaskCompletionSource<object>();
            AddTaskToChain(handler, waitfor);

            await waitfor.Task;
        }

        async Task RemoveLabelAsync(ILabelSource vlabel)
        {
            if (vlabel.IsVirtual)
                return;

            var label = vlabel as Label;
            var account = label.Account;

            await account.DeleteLabel(label);
        }

        public void CheckMails()
        {
            TaskChainHandler handler = delegate ()
            {
                return CheckMailsAsync();
            };

            AddTaskToChain(handler);
        }

        public async Task CheckMailsWait()
        {
            TaskChainHandler handler = delegate ()
            {
                return CheckMailsAsync();
            };

            TaskCompletionSource<object> waitfor = new TaskCompletionSource<object>();
            AddTaskToChain(handler, waitfor);

            await waitfor.Task;
        }

        async Task CheckMailsAsync()
        {
            Task<ViewMailItem[]>[] tasks = new Task<ViewMailItem[]>[mAccounts.Count];

            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = mAccounts[i].GetNewMails();
                //await tasks[i];
            }

            await Task.WhenAll(tasks);

            await mAccountDB.SaveChangesAsync();

            for (int i = 0; i < tasks.Length; i++)
            {
                if (!tasks[i].IsCompleted)
                    await tasks[i];

                if (tasks[i].Result == null || tasks[i].Result.Length == 0)
                    continue;
                AccountNotification?.Invoke(this, new AccountNotificationEventArgs()
                {
                    ForAccount = mAccounts[i].ViewAccount,
                    NewMails = tasks[i].Result
                });
            }
        }
        public void SaveMailMetaData(params ViewMailItem[] items)
        {
            List<ViewMailItem> itemlist = GetUpdatedItems(items);

            if (itemlist == null)
                return;

            TaskChainHandler handler = delegate ()
            {
                return SaveMailMetaDataAsync(items);
            };

            AddTaskToChain(handler);
        }

        public async Task SaveMailMetaDataWait(params ViewMailItem[] items)
        {
            List<ViewMailItem> itemlist = GetUpdatedItems(items);

            if (itemlist == null)
                return;

            TaskChainHandler handler = delegate ()
            {
                return SaveMailMetaDataAsync(items);
            };

            TaskCompletionSource<object> waitfor = new TaskCompletionSource<object>();
            AddTaskToChain(handler, waitfor);

            await waitfor.Task;

        }

        private List<ViewMailItem> GetUpdatedItems(ViewMailItem[] items)
        {
            List<ViewMailItem> ret = null;
            foreach(var i in items)
            {
                var state = i.MetaState;
                if (state == ViewMailMetaState.NoChange)
                    continue;

                if (ret == null)
                    ret = new List<ViewMailItem>();

                ret.Add(i);

                MailOperationFlags flags = MailOperationFlags.Working;

                if (state.HasFlag(ViewMailMetaState.LabelAdded) || state.HasFlag(ViewMailMetaState.LabelRemoved))
                    flags |= MailOperationFlags.ShowSpinner;
                if (state.HasFlag(ViewMailMetaState.CurrentLabelRemoved))
                    flags |= MailOperationFlags.Removing;

                i.OperationFlags = flags;
            }

            return ret;
        }

        async Task SaveMailMetaDataAsync(IEnumerable<ViewMailItem> items)
        {
            foreach (var item in items)
            {
                Account account = item.Source.AccountSource as Account;
                if (!await account.SaveMailMetaData(item))
                    item.RollbackChanges();
                else
                {
                    account.UpdateAccountCounts(item);
                    item.ClearChanges();
                }
            }

            MailView.UpdateWorkingItems(items);
            //await UpdateAccountAsync(account, true);
        }

        public void DeleteMail(IEnumerable<ViewMailItem> items)
        {
            TaskChainHandler handler = delegate ()
            {
                return DeleteMailAsync(items);
            };

            SetDeleting(items);

            AddTaskToChain(handler);
        }

        public async Task DeleteMailWait(IEnumerable<ViewMailItem> items)
        {
            TaskChainHandler handler = delegate ()
            {
                return DeleteMailAsync(items);
            };

            SetDeleting(items);

            TaskCompletionSource<object> waitfor = new TaskCompletionSource<object>();
            AddTaskToChain(handler, waitfor);

            await waitfor.Task;

        }

        private void SetDeleting(IEnumerable<ViewMailItem> items)
        {
            foreach(var i in items)
            {
                i.OperationFlags = MailOperationFlags.Removing | MailOperationFlags.ShowSpinner;
            }
        }

        private async Task DeleteMailAsync(IEnumerable<ViewMailItem> items)
        {
            foreach(var i in items)
            {
                if(i.Draft != null)
                {
                    DraftMessageSource draft = i.Draft as DraftMessageSource;
                    await DeleteDraft(draft);
                    continue;
                }
                var account = i.Source.AccountSource as Account;

                await account.DeleteMail(i);
            }
            

            MailView.UpdateWorkingItems(items);
        }

        private async Task DeleteDraft(DraftMessageSource draft)
        {
            await mAccountDB.DeleteDraftAsync(draft.DBDraft);
            mDrafts.Remove(draft);
            IDraftWindow window;

            if (mDraftWindows.TryGetValue(draft.DraftId, out window))
            {
                window.DraftDeleted();
            }

            if (MailView.Source != null && MailView.Source.AccountSource == draft.Account && MailView.Source.LabelSource != null && MailView.Source.LabelSource.Type == LabelType.Drafts)
                await ((Account)MailView.Source.AccountSource).LoadMailView(MailView.Source.LabelSource);

            UpdateAccountDraftCounts();
        }

        public ObservableCollection<ViewAccount> ViewAccounts { get { return mViewAccounts; } }

        public ObservableCollection<ViewAccount> SendAccounts { get { return mSendAccounts; } }

        internal event EventHandler UpdateAccountOperationStarted;
        public event EventHandler<AccountOperationEventArgs> AccountAdded;
        public event EventHandler<AccountOperationEventArgs> AccountRemoved;

        void OnAccountAction(bool added,Account account)
        {
            AccountOperationEventArgs e = new AccountOperationEventArgs()
            {
                Account = account
            };

            if (added && AccountAdded != null)
                AccountAdded(this, e);
            else if (!added && AccountRemoved != null)
                AccountRemoved(this, e);
        }

        public async Task SetMailView(ISourceInfo source)
        {
            MailView.Source = source;
            if(source == null || source.AccountSource == null)
            {
                MailView.Reset();
                MailView.CanSend = mSendAccounts.Count > 0;
                return;
            }

            await ((Account)source.AccountSource).LoadMailView(source.LabelSource);
        }

        public async Task UpdateMailView()
        {
            await ((Account)MailView.Source.AccountSource).LoadMoreMailView();
        }

        public async Task LoadMailBody(ViewMailItem item)
        {
            if (item.Source == null || item.Source.AccountSource == null)
                return;

            await ((Account)item.Source.AccountSource).LoadMailBody(item);
        }

        public async Task AddLookupAsync(EntityInfo item)
        {
            if (item == null)
                return;
            EntityInfo existing;

            if (mLookups.TryGetValue(item.EntityID, out existing))
            {
                existing.LookupComplete += delegate (object sender, EventArgs e)
                {
                    item.Name = existing.Name;
                };
                return;
            }

            if (mLookups.Count >= EntityLookupAsync.MAX_NAME_LOOKUP)
                throw new InvalidOperationException("Max lookups exceeded");

            mLookups.Add(item.EntityID, item);

            if (mLookups.Count >= EntityLookupAsync.MAX_NAME_LOOKUP)
                await FinishLookupsAsync();
        }

        public async Task AddLookupAsync(MailRecipient item)
        {
            if (item == null)
                return;

            if (item.Id == -1)
            {
                item.ImageUrl16 = "@://Images/Icon/16/GenericEntity.png";
                item.ImageUrl32 = "@://Images/Icon/32/GenericEntity.png";
                item.ImageUrl64 = "@://Images/Icon/64/GenericEntity.png";
                item.ImageUrl128 = "@://Images/Icon/128/GenericEntity.png";
                return;
            }

            if (mLookups.Count >= EntityLookupAsync.MAX_NAME_LOOKUP)
                throw new InvalidOperationException("Max lookups exceeded");

            EntityInfo info;

            if(!mLookups.TryGetValue(item.Id,out info))
            {
                info = new EntityInfo()
                {
                    EntityID = item.Id,
                    EntityType = item.Type
                };
                mLookups.Add(item.Id, info);
            }

            item.ImageUrl16 = info.ImageUrl16;
            item.ImageUrl32 = info.ImageUrl32;
            item.ImageUrl64 = info.ImageUrl64;
            item.ImageUrl128 = info.ImageUrl128;

            info.LookupComplete += delegate (object sender, EventArgs e)
            {
                item.Name = info.Name;
            };

            if (mLookups.Count >= EntityLookupAsync.MAX_NAME_LOOKUP)
                await FinishLookupsAsync();
        }

        public async Task FinishLookupsAsync()
        {
            var array = mLookups.Values.ToArray();
            mLookups.Clear();
            await EntityLookupAsync.LookupIDs(array);           
        }

        public void AddLookup(EntityInfo item)
        {
            if (item == null)
                return;
            EntityInfo existing;

            if (mLookups.TryGetValue(item.EntityID, out existing))
            {
                existing.LookupComplete += delegate (object sender, EventArgs e)
                {
                    item.Name = existing.Name;
                };
                return;
            }

            if (mLookups.Count >= EntityLookupAsync.MAX_NAME_LOOKUP)
                throw new InvalidOperationException("Max lookups exceeded");

            mLookups.Add(item.EntityID, item);

            if (mLookups.Count >= EntityLookupAsync.MAX_NAME_LOOKUP)
                FinishLookups();
        }

        public void AddLookup(MailRecipient item)
        {
            if (item == null)
                return;

            if (item.Id == -1)
            {
                item.ImageUrl16 = "@://Images/Icon/16/GenericEntity.png";
                item.ImageUrl32 = "@://Images/Icon/32/GenericEntity.png";
                item.ImageUrl64 = "@://Images/Icon/64/GenericEntity.png";
                item.ImageUrl128 = "@://Images/Icon/128/GenericEntity.png";
                return;
            }

            if (mLookups.Count >= EntityLookupAsync.MAX_NAME_LOOKUP)
                throw new InvalidOperationException("Max lookups exceeded");

            EntityInfo info;

            if (!mLookups.TryGetValue(item.Id, out info))
            {
                info = new EntityInfo()
                {
                    EntityID = item.Id,
                    EntityType = item.Type
                };
                mLookups.Add(item.Id, info);
            }

            item.ImageUrl16 = info.ImageUrl16;
            item.ImageUrl32 = info.ImageUrl32;
            item.ImageUrl64 = info.ImageUrl64;
            item.ImageUrl128 = info.ImageUrl128;

            info.LookupComplete += delegate (object sender, EventArgs e)
            {
                item.Name = info.Name;
            };

            if (mLookups.Count >= EntityLookupAsync.MAX_NAME_LOOKUP)
                FinishLookups();
        }

        public void FinishLookups()
        {
            var array = mLookups.Values.ToArray();
            mLookups.Clear();
            EntityLookup.LookupIDs(array);
        }

        internal IEnumerable<DraftMessageSource> GetDraftsForAccount(Account account)
        {
            foreach(var i in mDrafts)
            {
                if (i.Account == account.ViewAccount)
                    yield return i;
            }
        }

        internal int GetDraftCountForAccount(Account account)
        {
            int ret = 0;
            foreach (var i in mDrafts)
            {
                if (i.Account == account.ViewAccount)
                    ret++;
            }

            return ret;
        }

        public DraftMessageSource CreateDraft()
        {
            var ret = new DraftMessageSource(this);

            if (MailView.Source != null && MailView.Source.Account != null)
                ret.Account = MailView.Source.Account;
            else if (mSendAccounts.Count > 0)
                ret.Account = mSendAccounts[0];

            return ret;
        }

        static Regex mRegEx_GetTitle = new Regex(@"^((Re|Fw):\s*)?(?<Title>.*)$", RegexOptions.IgnoreCase);

        static Regex mRegEx_WS = new Regex(@"\s+");

        private string CreateEveWhoMarkup(MailRecipient recipient)
        {
            string[] dirlist = new string[]
            {
                "alli",
                "pilot",
                "corp"
            };

            if (recipient.Type == Common.EntityType.Mailinglist)
                return recipient.Name;

            return string.Format("<font size=\"12\" color=\"#ffffa600\"><loc><a href=\"https://evewho.com/{1}/{2}\">{0}</a></loc></font>"
                ,recipient.Name
                ,dirlist[(int)recipient.Type]
                ,mRegEx_WS.Replace(recipient.Name,"+"));
        }

        public DraftMessageSource CreateDraft(ViewMailItem item, DraftType type)
        {
            var ret = new DraftMessageSource(this);
            ret.Account = item.Source.Account;

            switch (type)
            {
                case DraftType.NewMail:
                    return ret;
                case DraftType.Reply:
                    ret.Subject = mRegEx_GetTitle.Replace(item.MailSubject, "Re: ${Title}");
                    ret.AddRecipients(item.From);
                    break;
                case DraftType.ReplyAll:
                    ret.Subject = mRegEx_GetTitle.Replace(item.MailSubject, "Re: ${Title}");
                    ret.AddRecipients(item.From);
                    ret.AddRecipients(item.Recipients);
                    break;
                case DraftType.Forward:
                    ret.Subject = mRegEx_GetTitle.Replace(item.MailSubject, "Fw: ${Title}");
                    break;
            }

            StringBuilder body = new StringBuilder();

            body.Append("<font size=\"12\" color=\"#bfffffff\"><br>--------------------------------<br>");
            body.AppendFormat("{0}<br>", item.MailSubject);
            body.AppendFormat("From: {0}<br>",CreateEveWhoMarkup(item.From));
            body.AppendFormat("Sent: {0:yyyy.MM.dd HH:mm}<br>", item.Timestamp);
            body.Append("To: ");
            List<string> recipients = new List<string>();
            foreach(var i in item.Recipients)
            {
                recipients.Add(CreateEveWhoMarkup(i));
            }
            body.Append(string.Join(", ", recipients));
            body.AppendFormat("<br><br>{0}", item.Body);

            ret.Body = body.ToString();

            return ret;
        }

        public bool OpenDraft<T>(DraftMessageSource draft,out T result)
            where T : class, IDraftWindow, new()
        {
            IDraftWindow window;
            if(mDraftWindows.TryGetValue(draft.DraftId,out window))
            {
                window.SetFocus();
                result = (T)window;
                return false;
            }

            result = new T();
            result.SetDraft(draft);
            mDraftWindows.Add(draft.DraftId, result);
            return true;
        }

        public void CloseDraft(DraftMessageSource draft)
        {
            mDraftWindows.Remove(draft.DraftId);
        }

        public async Task<bool> SendMail(DraftMessageSource draft)
        {
            var sourceinfo = draft.Account as ISourceInfo;
            var account = sourceinfo.AccountSource as Account;
            bool ret = await account.SendMail(draft);

            if (ret)
                await DeleteDraft(draft);

            return ret;
        }

        static Regex mRegEx_InfoPath = new Regex(@"^(?<Type>\d+)//(?<ID>\d+)$", RegexOptions.ExplicitCapture);
        public async Task<LinkResult> ClassifyLink(Uri link)
        {
            //if(link.Scheme == "hyperwave")
            //{
            //    switch (link.Host)
            //    {
            //        default:
            //            return new LinkResult()
            //            {
            //                Action = LinkAction.None
            //            };
            //    }
            //    return new LinkResult()
            //    {
            //        Action = LinkAction.HandledInternally
            //    };
            //}
            if (link.Scheme != "showinfo")
            {
                return new LinkResult()
                {
                    Action = LinkAction.WebLink,
                    Url = link
                };
            }

            Match item = mRegEx_InfoPath.Match(link.LocalPath);

            if (!item.Success)
            {
                return new LinkResult()
                {
                    Action = LinkAction.None
                };
            }

            LinkResult result = new LinkResult();

            switch(item.Groups["Type"].Value)
            {
                case "1373":
                case "1374":
                case "1385":
                case "1376":
                case "1375":
                case "1383":
                case "1377":
                case "1378":
                case "1384":
                case "1380":
                case "1379":
                case "1386":
                    result.Action = LinkAction.ShowEntity;
                    result.Recipient = new MailRecipient()
                    {
                        Type = Common.EntityType.Character,
                        Id = long.Parse(item.Groups["ID"].Value)
                    };
                    await AddLookupAsync(result.Recipient);
                    break;
                case "16159":
                    result.Action = LinkAction.ShowEntity;
                    result.Recipient = new MailRecipient()
                    {
                        Type = Common.EntityType.Alliance,
                        Id = long.Parse(item.Groups["ID"].Value)
                    };
                    await AddLookupAsync(result.Recipient);
                    break;
                case "2":
                    result.Action = LinkAction.ShowEntity;
                    result.Recipient = new MailRecipient()
                    {
                        Type = Common.EntityType.Corporation,
                        Id = long.Parse(item.Groups["ID"].Value)
                    };
                    await AddLookupAsync(result.Recipient);
                    break;
                default:
                    result.Action = LinkAction.None;
                    return result;
            }

            await FinishLookupsAsync();

            return result;
        }
    }

    public class AccountOperationEventArgs : EventArgs
    {
        internal Account Account { get; set; }
        public IAccountSource AccountSource { get { return Account; } }
        public ViewAccount ViewAccount { get { return Account.ViewAccount; } }
    }

    public class AccountNotificationEventArgs : EventArgs
    {
        public int? MailCount { get; set; }
        public ViewAccount ForAccount { get; set; }
        public ViewMailItem[] NewMails { get; set; }
    }
    
    public struct LinkResult
    {
        public LinkAction Action { get; set; }
        public Uri Url { get; set; }
        public MailRecipient Recipient { get; set; }
    }

    public enum LinkAction
    {
        None,
        HandledInternally,
        ShowEntity,
        WebLink
    }

    public enum DraftType
    {
        NewMail,
        Reply,
        ReplyAll,
        Forward
    }
}
