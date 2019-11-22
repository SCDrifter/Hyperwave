using Hyperwave.DB;
using Hyperwave.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hyperwave.UserCache;

namespace Hyperwave.Controller
{
    public class DraftMessageSource : IDraftMailSource
    {
        DraftMessage mDBDraft;
        EveMailClient mClient;
        Account mAccount;

        bool mUnsaved = true;

        internal DraftMessageSource(EveMailClient client)
        {
            mClient = client;
            mDBDraft = new DraftMessage();

            if (client.SendAccounts.Count > 0)
            {
                var ainfo = ((ISourceInfo)client.SendAccounts[0]);
                mAccount = (Account)ainfo.AccountSource;
            }
        }

        internal DraftMessageSource(EveMailClient client,DraftMessage dbdraft)
        {
            mClient = client;
            mDBDraft = dbdraft;
            mAccount = client.Accounts.Find((t) => t.DBAccount.CharacterId == dbdraft.AccountId);
            mUnsaved = false;
        }

        public Guid DraftId
        {
            get
            {
                return mDBDraft.DraftId;
            }
        }

        public ViewAccount Account
        {
            get
            {
                return mAccount == null? null : mAccount.ViewAccount;
            }

            set
            {
                ISourceInfo info = (ISourceInfo)value;
                mAccount = (Account)info.AccountSource;
                mDBDraft.AccountId = mAccount.DBAccount.CharacterId;
            }
        }

        public string Body
        {
            get
            {
                return mDBDraft.Body;
            }

            set
            {
                mDBDraft.Body = value;
            }
        }

        public bool IsReply
        {
            get
            {
                return mDBDraft.IsReply;
            }

            set
            {
                mDBDraft.IsReply = value;
            }
        }

        public EntityInfo[] Recipients
        {
            get
            {
                return mDBDraft.Recipients.ToArray();
            }

            set
            {
                mDBDraft.Recipients.Clear();
                mDBDraft.Recipients.UnionWith(value);
            }
        }

        public bool AddRecipients(string name)
        {
            var entities = EntityLookup.Search(name, SearchOptions.ExactMatch | SearchOptions.Online);

            if (entities == null || entities.Length == 0)
                return false;

            AddRecipients(entities);

            return true;
        }

        public bool AddRecipient(string name,Common.EntityType type)
        {
            var entities = EntityLookup.Search(name, SearchOptions.ExactMatch | SearchOptions.Online,type);

            if (entities == null || entities.Length == 0)
                return false;

            AddRecipients(entities);

            return true;
        }

        public void AddRecipients(params EntityInfo[] recipients)
        {
            mDBDraft.Recipients.UnionWith(recipients);
        }

        public void AddRecipients(params MailRecipient[] recipients)
        {
            EntityInfo[] info = new EntityInfo[recipients.Length];
            for (int i = 0; i < recipients.Length; i++)
            {
                info[i] = new EntityInfo()
                {
                    EntityType = recipients[i].Type,
                    EntityID = recipients[i].Id,
                    Name = recipients[i].Name
                };
            }

            AddRecipients(info);
        }

        public string Subject
        {
            get
            {
                return mDBDraft.Subject;
            }

            set
            {
                mDBDraft.Subject = value;
            }
        }

        public DateTime LastModified
        {
            get
            {
                return mDBDraft.LastModified;
            }
            set
            {
                mDBDraft.LastModified = value;
            }
        }

        public DraftMessage DBDraft { get { return mDBDraft; } }

        public void NotifySaved()
        {
            Task task = NotifySavedAsync();
        }

        public async Task NotifySavedAsync()
        {
            await mClient.AccountsDB.SaveDraftAsync(mDBDraft);

            if (mUnsaved)
                mClient.Drafts.Add(this);

            mUnsaved = false;

            if (mClient.MailView.Source != null &&
                mClient.MailView.Source.Label != null &&
                mClient.MailView.Source.Label.Type == LabelType.Drafts)
            {
                await mClient.SetMailView(mClient.MailView.Source);
            }

            mClient.UpdateAccountDraftCounts();
        }
    }
}
