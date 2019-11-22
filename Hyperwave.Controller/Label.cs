using System;
using Hyperwave.ViewModel;
using IO.Swagger.Model;

namespace Hyperwave.Controller
{
    class Label : ILabelSource
    {
        private Account mAccount;
        
        private EveMailClient mClient;

        public Label(EveMailClient client,Account account,int id,string name,LabelType type)
        {
            mClient = client;
            mAccount = account;

            Name = name;
            Id = id;
            UnreadCount = 0;
            Type = type;
        }

        public Label(EveMailClient client, Account account, GetCharactersCharacterIdMailLabelsLabel label)
        {
            mClient = client;
            mAccount = account;

            Name = label.Name;
            Id = label.LabelId.Value;
            UnreadCount = label.UnreadCount.HasValue ? label.UnreadCount.Value : 0;

        
            switch(Name)
            {
                case "Inbox":
                    Type = LabelType.Inbox;
                    break;
                case "Sent":
                    Type = LabelType.Outbox;
                    break;
                case "[Corp]":
                    Type = LabelType.CorpMail;
                    break;
                case "[Alliance]":
                    Type = LabelType.AllianceMail;
                    break;
                default:
                    Type = LabelType.Label;
                    break;
            }
        }

        public int Id
        {
            get; set;
        }

        public LabelType Type
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }

        public int UnreadCount
        {
            get; set;
        }

        public bool IsVirtual
        {
            get
            {
                return Type == LabelType.MailingList || Type == LabelType.Drafts;
            }
        }

        internal Account Account
        {
            get { return mAccount; }
        }
    }
}