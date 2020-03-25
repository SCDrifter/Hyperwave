using System;
using Hyperwave.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eve.Api.Api;
using Hyperwave.Config;
using Eve.Api.Model;
using Hyperwave.Common;
using System.Linq;

namespace Hyperwave.Controller
{
    class Account : IAccountSource,IComparable<Account>
    {
        ViewAccount mViewAccount = new ViewAccount();
        DB.Account mDBAccount;
        EveMailClient mClient;
        UserCache.EntityInfo mEntityInfo = new UserCache.EntityInfo();

        bool mReadFailed = false;

        List<ILabelSource> mLabels = new List<ILabelSource>();
        private Label mDraftLabel;

        NLog.Logger mLog = NLog.LogManager.GetCurrentClassLogger();
#if FAKE_NAMES
        UserCache.EntityInfo mFakeEntityInfo;
#endif

        internal Account(EveMailClient client,DB.Account account)
        {
            mDBAccount = account;
            mClient = client;

            mDraftLabel = new Label(mClient, this, -1, "Drafts", LabelType.Drafts);

            mEntityInfo.EntityType = EntityType.Character;
            mEntityInfo.EntityID = account.CharacterId;
            mEntityInfo.Name = account.CharacterName;
#if FAKE_NAMES
            GenerateFakeInfo();
#endif
            mLabels.Add(mDraftLabel);

            mViewAccount.RegisterHandler("IsExpanded", ViewAccount_IsExpandedChanged);
            
            mViewAccount.Sync(this);

            mClient.UpdateAccountOperationStarted += mClient_UpdateAccountOperationStarted;
            mClient.AccountRemoved += mClient_AccountRemoved;

        }

#if FAKE_NAMES
        static string[] mNames = new string[]
        {
            "Fornfox",
            "Scotoads",
            "Bilmarshall Bagthy Evanswood",
            "Hillbus Barneshot",
            "Johnreek",
            "The Harrisonumble",
            "Grahapheles",
            "Feriri The Casttt",
            "Winable",
            "Silvagmagog"
        };

        static Random mRandom = new Random();
        private void GenerateFakeInfo()
        {
            if (mEntityInfo.EntityID == 96181148)
                mFakeEntityInfo = mEntityInfo;
            else
                mFakeEntityInfo = new UserCache.EntityInfo()
                {
                    EntityType = EntityType.Character,
                    EntityID = 96181148,
                    Name = mNames[mRandom.Next(0, mNames.Length)]
                };
        }
#endif

        private void ViewAccount_IsExpandedChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            DBAccount.IsExpanded = mViewAccount.IsExpanded;
        }

        private void mClient_AccountRemoved(object sender, AccountOperationEventArgs e)
        {
            if (e.Account == this)
            {
                mClient.AccountRemoved -= mClient_AccountRemoved;
                mClient.UpdateAccountOperationStarted -= mClient_UpdateAccountOperationStarted;
            }
        }

        private void mClient_UpdateAccountOperationStarted(object sender, EventArgs e)
        {
            mViewAccount.ShowSpinner = true;
        }

        public async Task UpdateAccount()
        {
            mLog.Info($"{this.UserName}: Updating account");

            await RefreshToken();

            MailApi api = new MailApi();
            GetCharactersCharacterIdMailLabelsOk labels;

            int draft_index = 0;

            try
            {
                labels = await api.GetCharactersCharacterIdMailLabelsAsync(
                    characterId:(int)mDBAccount.CharacterId,
                    datasource: ESIConfiguration.DataSource,
                    token: mDBAccount.AccessToken);


                mReadFailed = false;
                mViewAccount.AccountState = AccountState;

                mLabels.Clear();

                mLog.Info($"{this.UserName}: {labels.TotalUnreadCount.Value} unread mails");

                mDBAccount.UnreadCount = labels.TotalUnreadCount.Value;

                for (int i = 0;i < labels.Labels.Count;i++)
                {
                    var item = labels.Labels[i];
                    var label = new Label(mClient, this, item);
                    mLabels.Add(label);
                    mLog.Info($"{this.UserName}: Added label '{label.Name}'");
                    if (label.Type == LabelType.Outbox)
                        draft_index = i + 1;
                }

                await GetMailingLists(api);
            }
            catch (Eve.Api.Client.ApiException e)
            {
                ExceptionHandler.HandleApiException(null, e);
                mReadFailed = true;
                mLabels.Clear();
            }

            mDraftLabel.UnreadCount = mClient.GetDraftCountForAccount(this);

            mLabels.Insert(draft_index, mDraftLabel);
            mViewAccount.Sync(this);
            mViewAccount.ShowSpinner = false;
        }

        private async Task GetMailingLists(MailApi api)
        {
            List<GetCharactersCharacterIdMailLists200Ok> list;

            try
            {
                list = await api.GetCharactersCharacterIdMailListsAsync(
                    characterId: (int)mDBAccount.CharacterId,
                    datasource: ESIConfiguration.DataSource,
                    token: mDBAccount.AccessToken);
            }
            catch (Eve.Api.Client.ApiException e)
            {
                ExceptionHandler.HandleApiException(null, e);
                return;
            }

            UserCache.EntityInfo[] info = new UserCache.EntityInfo[list.Count];
            int index = 0;
            foreach(var i in list)
            {
                if (!i.MailingListId.HasValue)
                    continue;
                info[index++] = new UserCache.EntityInfo()
                {
                    EntityType = EntityType.Mailinglist,
                    EntityID = i.MailingListId.Value,
                    Name = i.Name
                };
                var label = new Label(mClient, this, i.MailingListId.Value, i.Name, LabelType.MailingList);
                mLabels.Add(label);
                mLog.Info($"{this.UserName}: Added mailing list '{label.Name}'");
            }

            await UserCache.EntityLookupAsync.AddLookups(info);
        }

        private async Task<bool> RefreshToken()
        {
            mViewAccount.AccountState = AccountState;

            if (mDBAccount.Expires < DateTime.Now)
            {
                mLog.Info($"{this.UserName}: Refreshing tokens");
                Auth.TokenInfo token = await Auth.SSOAuth.RefreshTokenInfoAsync(DBAccount.RefreshToken);
                if (token == null)
                {
                    mReadFailed = true;
                    return false;
                }
                mDBAccount.UpdateAuthInfo(token);
            }

            return true;
        }

        public ViewAccount ViewAccount { get { return mViewAccount; } }
        public DB.Account DBAccount { get { return mDBAccount; } }

        MailCache mGlobalCache = new MailCache();
        Dictionary<int, MailCache> mLabelCache = new Dictionary<int, MailCache>();

        public AccountState AccountState
        {
            get
            {
                if (mDBAccount.AccessToken == null || mReadFailed)
                    return AccountState.Failed;
                else if (mDBAccount.Expires < DateTime.Now)
                    return AccountState.Offline;
                else
                    return AccountState.Online;
            }
        }

        public string ImageUrl
        {
            get
            {
#if FAKE_NAMES
                return mFakeEntityInfo.ImageUrl32;
#else
                return mEntityInfo.ImageUrl32;
#endif
            }
        }

        public ILabelSource[] Labels
        {
            get
            {
                return mLabels.ToArray();
            }
        }

        public int UnreadCount
        {
            get
            {
                return mDBAccount.UnreadCount;
            }
            set
            {
                mDBAccount.UnreadCount = value;
            }
        }

        public async Task<ViewMailItem[]> GetNewMails()
        {
            if (!await RefreshToken())
                return null;

            List<ViewMailItem> items = new List<ViewMailItem>();
            List<ViewMailItem> list;
            try
            {
#if TEST_NOTIFICATIONS
                list = await LoadMailsWorker(null, ViewAccount);
#else
                list = await LoadMailsWorker(null, ViewAccount, null, DBAccount.LastMailId, true);
#endif
            }
            catch (Eve.Api.Client.ApiException e)
            {
                ExceptionHandler.HandleApiException(null, e);
                mReadFailed = true;
                mViewAccount.AccountState = AccountState;
                return null;
            }
            catch(Exception)
            {
                return null;
            }
#if !TEST_NOTIFICATIONS
            if (list.Count > 0)
            {
                DBAccount.LastMailId = list[0].MailId;
            }
#endif
            foreach (var i in list)
            {
#if !TEST_NOTIFICATIONS
                if (!i.IsItemRead)
#else
                if (!i.IsItemRead || i == list[0])
#endif
                    items.Add(i);
            }

            return items.ToArray();
        }
        public async Task<bool> LoadMoreMailView()
        {
            var view = mClient.MailView;
            List<ViewMailItem> viewmails;
            ILabelSource label = mClient.MailView.Source.LabelSource;
            try
            {
                
                view.IsLoading = true;
                viewmails = await LoadMailsWorker(label, view.Source,(int)view.LastMailId);
                GetCache(label).Add(viewmails);                
            }
            catch (Eve.Api.Client.ApiException e)
            {
                ExceptionHandler.HandleApiException(null, e);
                mReadFailed = true;
                view.IsLoading = false;
                mViewAccount.AccountState = AccountState;
                return false;
            }
            view.IsDraft = false;


            foreach (var i in viewmails)
            {
                view.MailItems.Add(i);
                view.LastMailId = i.MailId;
            }

            view.SortItems();

            view.IsLoading = false;
            view.CanDownload = viewmails.Count > 0;

            mReadFailed = false;
            mViewAccount.AccountState = AccountState;
            return true;
        }
        public async Task<bool> LoadMailView(ILabelSource label)
        {            
            var view = mClient.MailView;

            view.Reset();

            if (label != null && label.Type == LabelType.Drafts)
            {
                return await LoadDrafts(view);
            }
            if (!await RefreshToken())
                return false;
                                   
            view.CanDelete = DBAccount.Permissions.HasFlag(Auth.AccessFlag.MailWrite);
            view.CanSend = DBAccount.Permissions.HasFlag(Auth.AccessFlag.MailSend);

            List<ViewMailItem> viewmails;

            try
            {
                if (!GetCachedItems(label, out viewmails))
                {
                    mLog.Info($"{this.UserName}: Updating from source");
                    view.IsLoading = true; 
                    viewmails = await LoadMailsWorker(label, view.Source);
                    GetCache(label).Set(viewmails);
                }
            }
            catch (Eve.Api.Client.ApiException e)
            {
                ExceptionHandler.HandleApiException(null, e);
                mReadFailed = true;
                view.IsLoading = false;
                view.HasItems = false;
                view.MailItems.Clear();
                mViewAccount.AccountState = AccountState;
                return false;
            }
            view.IsDraft = false;

            mLog.Info($"{this.UserName}: {viewmails.Count} mails retrieved");

            foreach (var i in viewmails)
            {
                view.MailItems.Add(i);
                view.LastMailId = i.MailId;
            }

            view.IsLoading = false;
            view.HasItems = viewmails.Count > 0;
            view.SortItems();

            mReadFailed = false;
            mViewAccount.AccountState = AccountState;
            return true;
            
        }

        private bool GetCachedItems(ILabelSource label, out List<ViewMailItem> viewmails)
        {
            var cache = GetCache(label);
            if(!cache.IsValid)
            {
                cache.Reset();
                viewmails = null;
                return false;
            }

            viewmails = new List<ViewMailItem>(cache.MailItems);
            return true;
        }

        private bool GetItemFromCache(long id, out ViewMailItem item)
        {
            Predicate<ViewMailItem> search = (q) => q.MailId == id;
            if (mGlobalCache.IsValid && null != (item = mGlobalCache.MailItems.Find(search)))
            {
                return true;
            }

            foreach(var i in mLabelCache.Values)
            {
                if (i.IsValid && null != (item = i.MailItems.Find(search)))
                    return true;
            }

            item = null;
            return false;
        }

        private async Task<List<ViewMailItem>> LoadMailsWorker(ILabelSource label,ISourceInfo source,int? from = null,long? to = null,bool fresh = false)
        {
            List<int?> labels = null;

            if (label != null && !label.IsVirtual)
            {
                labels = new List<int?>();
                labels.Add(label.Id);
            }

            MailApi api = new MailApi();

            int id = (int)DBAccount.CharacterId;

            List<GetCharactersCharacterIdMail200Ok> mails;

            mails = await api.GetCharactersCharacterIdMailAsync(
                characterId:id, 
                datasource:ESIConfiguration.DataSource, 
                labels:labels,
                lastMailId: from,
                token: DBAccount.AccessToken);            

            List<ViewMailItem> viewmails = new List<ViewMailItem>();
            List<MailRecipient> recipients = new List<MailRecipient>();

            foreach (var i in mails)
            {
                if (!i.MailId.HasValue)
                    continue;

                if (to.HasValue && to.Value == i.MailId.Value)
                    break;

                if (label != null && label.Type == LabelType.MailingList)
                {
                    if (!HasRecipient(i, EntityType.Mailinglist, label.Id))
                        continue;
                }

                recipients.Clear();

                ViewMailItem item;

                if(!fresh && GetItemFromCache(i.MailId.Value,out item))
                {
                    viewmails.Add(item);
                    continue;
                }
                item = new ViewMailItem(i.MailId.Value, i.IsRead.HasValue && i.IsRead.Value);
                item.Source = source;
                item.From = new MailRecipient(EntityType.Character, i.From.HasValue ? i.From.Value : -1);
                await mClient.AddLookupAsync(item.From);
                item.MailSubject = i.Subject;
                item.Timestamp = i.Timestamp.HasValue ? i.Timestamp.Value : DateTime.MinValue;

                foreach (var j in i.Recipients)
                {
                    MailRecipient recipient = new MailRecipient(ESIConvert.RecipientTypeToEntityType(j.RecipientType.GetValueOrDefault()), j.RecipientId.Value);
                    recipients.Add(recipient);
                }

                foreach (var j in mViewAccount.Labels)
                {
                    if (j.IsVirtual)
                        continue;

                    item.Labels.Add(new ViewMailLabelLink(item, j, i.Labels.Contains(j.Id), label != null && label.Id == j.Id));
                }

                item.Recipients = recipients.ToArray();

                viewmails.Add(item);
            }

            await mClient.FinishLookupsAsync();

            return viewmails;
        }

        private async Task<bool> LoadDrafts(MailView view)
        {
            view.CanDelete = view.CanSend = DBAccount.Permissions.HasFlag(Auth.AccessFlag.MailSend);
            view.CanDownload = false;

            view.IsDraft = true;

            List<MailRecipient> recipients = new List<MailRecipient>();

            foreach(var i in mClient.GetDraftsForAccount(this))
            {
                ViewMailItem item = new ViewMailItem(-1, true);
                item.Draft = i;
                item.MailSubject = i.Subject;
                item.Body = i.Body;
                item.Timestamp = i.LastModified;
                item.From = new MailRecipient(EntityType.Character, DBAccount.CharacterId);
                await mClient.AddLookupAsync(item.From);

                recipients.Clear();

                foreach(var j in i.Recipients)
                {
                    MailRecipient recipient = new MailRecipient(j.EntityType, j.EntityID);
                    await mClient.AddLookupAsync(recipient);
                    recipients.Add(recipient);
                }

                item.Recipients = recipients.ToArray();
                item.Source = view.Source;
                view.MailItems.Add(item);
            }

            await mClient.FinishLookupsAsync();

            view.HasItems = true;
            view.IsLoading = false;

            return true;
        }

        private bool HasRecipient(GetCharactersCharacterIdMail200Ok mailitem, EntityType type, int id)
        {
            foreach (var i in mailitem.Recipients)
            {
                
                if (type == ESIConvert.RecipientTypeToEntityType(i.RecipientType.GetValueOrDefault()) && id == i.RecipientId.GetValueOrDefault())
                    return true;
            }

            return false;
        }

        internal async Task<bool> DeleteMail(ViewMailItem item)
        {
            if (!await RefreshToken())
                return false;
                        
            MailApi api = new MailApi();

            int? charid = (int?)mDBAccount.CharacterId;
            int? mailid = (int?)item.MailId;
            
            try
            {
                await api.DeleteCharactersCharacterIdMailMailIdAsync(
                    characterId: charid, 
                    mailId: mailid,
                    datasource: ESIConfiguration.DataSource);

                
            }
            catch (Eve.Api.Client.ApiException e)
            {
                ExceptionHandler.HandleApiException(null, e);
                mReadFailed = true;
                mViewAccount.AccountState = AccountState;
                return false;
            }

            mGlobalCache.MailItems.Remove(item);

            foreach(var i in mLabelCache.Values)
            {
                i.MailItems.Remove(item);
            }

            return true;
        }

        public async Task<bool> LoadMailBody(ViewMailItem item)
        {
            if (!await RefreshToken())
                return false;

            if (item.HasBody)
                return true;


            foreach (var i in item.Recipients)
            {
                if (i.ImageUrl64 == null)
                    await mClient.AddLookupAsync(i);
            }

            await mClient.FinishLookupsAsync();

            MailApi api = new MailApi();

            int id = (int)DBAccount.CharacterId;
            int mailid = (int)item.MailId;

            try
            {
                var resp = await api.GetCharactersCharacterIdMailMailIdAsync(
                    characterId: id, 
                    mailId: mailid,
                    datasource: ESIConfiguration.DataSource,
                    token: DBAccount.AccessToken);

                item.Body = resp.Body;
                mReadFailed = false;
                mViewAccount.AccountState = AccountState;
                return true;
            }
            catch (Eve.Api.Client.ApiException e)
            {
                ExceptionHandler.HandleApiException(null, e);
                mReadFailed = true;
                mViewAccount.AccountState = AccountState;
                return false;
            }

        }

        internal void UpdateAccountDraftCounts()
        {
            mDraftLabel.UnreadCount = mClient.GetDraftCountForAccount(this);
        }

        internal void UpdateAccountCounts(ViewMailItem item)
        {
            int[,] statetable = new int[,]
            {
                //Label Added|Label Removed|No Change(True)|No Change(False)
                { 0, -1 ,-1, 0 }, /*Read Set*/        
                { 1,  0,  1, 0 }, /*Read Unset*/      
                { 1, -1,  0, 0 }, /*No Change(Unread)*/ 
                { 0,  0,  0, 0 }  /*No Change(Read)*/
            };

            int readstate = -1;

            if (item.MetaState.HasFlag(ViewMailMetaState.ReadFlagSet))
            {
                readstate = 0;
                UnreadCount--;
            }
            else if (item.MetaState.HasFlag(ViewMailMetaState.ReadFlagUnset))
            {
                readstate = 1;
                UnreadCount++;
            }
            else if (item.IsItemUnread)
                readstate = 2;
            else
                readstate = 3;

            if (UnreadCount < 0)
                UnreadCount = 0;

            for (int i = 0;i < item.Labels.Count;i++)
            {
                var vlabel = item.Labels[i];

                Label label = ((ISourceInfo)vlabel.Label).LabelSource as Label;
                int labelstate = -1;
                
                if (vlabel.MetaState.HasFlag(ViewMailMetaState.LabelAdded))
                    labelstate = 0;
                else if (vlabel.MetaState.HasFlag(ViewMailMetaState.LabelRemoved))
                    labelstate = 1;
                else if (vlabel.Subscribed)
                    labelstate = 2;
                else
                    labelstate = 3;



                label.UnreadCount += statetable[readstate, labelstate];

                MailCache cache = GetCache(label);

                if (cache != null && cache.IsValid)
                {
                    cache.MailItems.Remove(item);

                    if (vlabel.Subscribed)
                        cache.MailItems.Add(item);
                }
            }

            ViewAccount.Sync(this);
        }

        internal MailCache GetCache(ILabelSource labelsource)
        {
            if (labelsource == null)
                return mGlobalCache;

            MailCache ret;

            if(!mLabelCache.TryGetValue(labelsource.Id,out ret))
            {
                ret = new MailCache();
                mLabelCache.Add(labelsource.Id, ret);
            }

            return ret; 
        }

        public async Task<bool> SaveMailMetaData(ViewMailItem item)
        {
            if (!await RefreshToken())
                return false;

            MailApi api = new MailApi();
            PutCharactersCharacterIdMailMailIdContents content = new PutCharactersCharacterIdMailMailIdContents();
            content.Read = item.IsItemRead;
            content.Labels = new List<int?>();
            foreach(var i in item.Labels)
            {
                if (i.Subscribed)
                    content.Labels.Add(i.Label.Id);
            }

            try
            {
                int id = (int)DBAccount.CharacterId;
                int mailid = (int)item.MailId;

                await api.PutCharactersCharacterIdMailMailIdAsync(
                    characterId: id, 
                    contents: content, 
                    mailId: mailid,
                    datasource: ESIConfiguration.DataSource,
                    token: DBAccount.AccessToken);

                return true;
            }
            catch (Eve.Api.Client.ApiException e)
            {
                ExceptionHandler.HandleApiException(null, e);
                mReadFailed = true;
                mViewAccount.AccountState = AccountState;
                return false;
            }
        }

        internal async Task<bool> SendMail(DraftMessageSource draft)
        {
            ExceptionHandler.LastException = null;

            if (!await RefreshToken())
                return false;
            try
            {
                MailApi api = new MailApi();
                PostCharactersCharacterIdMailMail mail = new PostCharactersCharacterIdMailMail
                    (
                        Recipients: new List<PostCharactersCharacterIdMailRecipient>(),
                        Subject: draft.Subject,
                        Body: draft.Body
                    );

                foreach (var i in draft.Recipients)
                {
                    mail.Recipients.Add(new PostCharactersCharacterIdMailRecipient(
                        RecipientType: ESIConvert.EntityTypeToRecipientType(i.EntityType), 
                        RecipientId:(int?)i.EntityID));
                }

                await api.PostCharactersCharacterIdMailAsync(
                    characterId:(int?)DBAccount.CharacterId,
                    mail: mail, 
                    datasource: ESIConfiguration.DataSource,
                    token: DBAccount.AccessToken);

                return true;
            }
            catch (Eve.Api.Client.ApiException e)
            {
                ExceptionHandler.HandleApiException(null, e);
                return false;
            }
            catch (Exception e)
            {
                ExceptionHandler.HandleOtherException(null, e);
                return false;
            }
        }

        internal async Task<bool> DeleteLabel(Label label)
        {
            if (!await RefreshToken())
                return false;

            MailApi api = new MailApi();

            int? charid = (int?)mDBAccount.CharacterId;
            int? labelid = (int?)label.Id;

            try
            {
                await api.DeleteCharactersCharacterIdMailLabelsLabelIdAsync(
                    characterId: charid,
                    labelId: labelid,
                    datasource: ESIConfiguration.DataSource,
                    token: DBAccount.AccessToken);

                mLabels.Remove(label);
                ViewAccount.Sync(this);
                return true;
            }
            catch(Eve.Api.Client.ApiException e)
            {
                ExceptionHandler.HandleApiException(null,e);
                return false;
            }
        }

        internal async Task<bool> AddLabel(string label_name)
        {
            if (!await RefreshToken())
                return false;

            MailApi api = new MailApi();

            int? charid = (int?)mDBAccount.CharacterId;
            PostCharactersCharacterIdMailLabelsLabel label = new PostCharactersCharacterIdMailLabelsLabel(
                PostCharactersCharacterIdMailLabelsLabel.ColorEnum.Ffffff, label_name);

            try
            {
                int? labelid = await api.PostCharactersCharacterIdMailLabelsAsync(
                    characterId: charid,
                    label: label,
                    datasource: ESIConfiguration.DataSource,
                    token: DBAccount.AccessToken);

                if (!labelid.HasValue)
                    return false;

                mLabels.Add(new Label(mClient, this,labelid ?? -1, label_name,LabelType.Label));
                ViewAccount.Sync(this);
                return true;
            }
            catch (Eve.Api.Client.ApiException e)
            {
                ExceptionHandler.HandleApiException(null, e);
                return false;
            }
        }

        int IComparable<Account>.CompareTo(Account other)
        {
            return DBAccount.CharacterId.CompareTo(other.DBAccount.CharacterId);
        }

        public string UserName
        {
            get
            {
#if FAKE_NAMES
                return mFakeEntityInfo.Name;
#else
                return mDBAccount.CharacterName;
#endif
            }
        }

        public long Id
        {
            get
            {
                return mDBAccount.CharacterId;
            }

        }

        public Label DraftsLabel
        {
            get { return mDraftLabel; }
        }

        public bool IsExpanded
        {
            get
            {
                return DBAccount.IsExpanded;
            }
        }
    }
}