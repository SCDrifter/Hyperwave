using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperwave.ViewModel
{
    public class DraftMail : UIObject
    {
        private ObservableCollection<DraftMailRecipient> mEditableList = new ObservableCollection<DraftMailRecipient>();
        private ViewAccount mAccount;
        private string mSubject;
        private bool mIsReply;
        private string mBody;

        IDraftMailSource mSource = null;

        public DraftMail()
        {
            Recipients.CollectionChanged += Recipients_CollectionChanged;
            mEditableList.Add(NewMail);
            NewMail.NewReceipient += NewMail_NewReceipient;
        }

        public DraftMail(IDraftMailSource source)
            : this()
        {
            Load(source);
        }

        public void Load(IDraftMailSource source)
        {
            mSource = source;
            Account = source.Account;
            Subject = source.Subject;
            IsReply = source.IsReply;
            Body = source.Body;
            Recipients.Clear();
            foreach(var i in source.Recipients)
            {
                Recipients.Add(new DraftMailRecipient(i));
            }
        }

        public void Save(IDraftMailSource source = null)
        {
            if (source != null)
                mSource = source;
            List<UserCache.EntityInfo> list = new List<UserCache.EntityInfo>(Recipients.Count);
            foreach(var i in Recipients)
            {
                if(i.IsValid)
                    list.Add(i.ToEntityInfo());
            }

            mSource.Recipients = list.ToArray();

            mSource.Account = Account;
            mSource.Subject = Subject ?? "";
            mSource.IsReply = IsReply;
            mSource.Body = Body ?? "";
            mSource.NotifySaved();
        }

        public ObservableCollection<DraftMailRecipient> Recipients { get; private set; } = new ObservableCollection<DraftMailRecipient>();
        public NewDraftMailRecipient NewMail { get; private set; } = new NewDraftMailRecipient();

        public IDraftMailSource Source { get { return mSource; } }

        public ObservableCollection<DraftMailRecipient> EditableReceipients
        {
            get
            {
                return mEditableList;
            }
        }

        public ViewAccount Account
        {
            get
            {
                return mAccount;
            }
            set
            {
                if (mAccount == value)
                    return;
                mAccount = value;
                OnPropertyChanged("Account");
            }
        }

        public string Subject
        {
            get
            {
                return mSubject;
            }
            set
            {
                if (mSubject == value)
                    return;
                mSubject = value;
                OnPropertyChanged("Subject");
                OnPropertyChanged("MailTitle");
            }
        }

        public bool IsReply
        {
            get
            {
                return mIsReply;
            }
            set
            {
                if (mIsReply == value)
                    return;
                mIsReply = value;
                OnPropertyChanged("IsReply");
            }
        }

        public string Body
        {
            get
            {
                return mBody;
            }
            set
            {
                if (mBody == value)
                    return;
                mBody = value;
                OnPropertyChanged("Body");
            }
        }

        public string MailTitle
        {
            get
            {
                return string.IsNullOrEmpty(mSubject) ? "(No Subject)" : mSubject;
            }
        }

        private void NewMail_NewReceipient(object sender, NewReceipientEventArgs e)
        {
            Recipients.Add(e.Recipient);
        }

        private void Recipients_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (DraftMailRecipient i in e.NewItems)
                        mEditableList.Insert(mEditableList.Count - 1, i);
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (DraftMailRecipient i in e.OldItems)
                        mEditableList.Remove(i);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    foreach (DraftMailRecipient i in e.OldItems)
                        mEditableList.Remove(i);

                    foreach (DraftMailRecipient i in e.NewItems)
                        mEditableList.Add(i);

                    break;
                case NotifyCollectionChangedAction.Reset:
                    mEditableList.Clear();
                    mEditableList.Add(NewMail);
                    break;
            }
        }

    }
}
