using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Hyperwave.ViewModel
{
    public class ViewMailItem : UIObject,IComparable<ViewMailItem>
    {
        bool mIsItemRead = false;
        MailRecipient mFrom;
        MailRecipient[] mRecipients;
        string mMailSubject;
        string mBody;
        private DateTime mTimestamp;
        private bool mIsLoading = false;
        ObservableCollection<ViewMailLabelLink> mLabels = new ObservableCollection<ViewMailLabelLink>();

        ViewMailMetaState mReadState;
        private MailOperationFlags mOperationFlags;
        private IDraftMailSource mDraft;

        internal int LabelAddedCount
        {
            get; set;
        }
        internal int LabelRemovedCount
        {
            get; set;
        }

        public ViewMailItem(long id, bool read)
        {
            MailId = id;
            mIsItemRead = read;
        }
        public long MailId { get; private set; }

        public IDraftMailSource Draft
        {
            get
            {
                return mDraft;
            }
            set
            {
                if (mDraft == value)
                    return;
                mDraft = value;
                OnPropertyChanged("Draft");
            }
        }


        public bool IsItemRead
        {
            get
            {
                return mIsItemRead;
            }

            set
            {
                if (mIsItemRead != value)
                {
                    mIsItemRead = value;

                    if(value)
                    {
                        mReadState = (mReadState == ViewMailMetaState.ReadFlagUnset)
                            ? ViewMailMetaState.NoChange
                            : ViewMailMetaState.ReadFlagSet;
                    }
                    else
                    {
                        mReadState = (mReadState == ViewMailMetaState.ReadFlagSet)
                            ? ViewMailMetaState.NoChange
                            : ViewMailMetaState.ReadFlagUnset;
                    }

                    OnPropertyChanged("IsItemRead");
                    OnPropertyChanged("IsItemUnread");
                    OnPropertyChanged("ShowNewItemMark");
                }
            }
        }

        public MailOperationFlags OperationFlags
        {
            get
            {
                return mOperationFlags;
            }
            set
            {
                if(mOperationFlags != value)
                {
                    mOperationFlags = value;
                    OnPropertyChanged("OperationFlags");
                    OnPropertyChanged("ShowNewItemMark");
                    OnPropertyChanged("ShowSpinner");
                    OnPropertyChanged("ItemOpacity");
                }
            }
        }

        public ViewMailMetaState MetaState
        {
            get
            {
                ViewMailMetaState ret = mReadState;

                if (LabelAddedCount > 0)
                    ret |= ViewMailMetaState.LabelAdded;
                if (CurrentLabelRemoved)
                    ret |= ViewMailMetaState.CurrentLabelRemoved;
                else if (LabelRemovedCount > 0)
                    ret |= ViewMailMetaState.LabelRemoved;
                
                return ret;
            }
        }

        public bool ShowNewItemMark
        {
            get { return !mIsItemRead && !mOperationFlags.HasFlag(MailOperationFlags.ShowSpinner); }
        }

        public bool ShowSpinner
        {
            get { return mOperationFlags.HasFlag(MailOperationFlags.ShowSpinner); }
        }

        public double ItemOpacity
        {
            get
            {
                return mOperationFlags.HasFlag(MailOperationFlags.Removing) ? 0.5 : 1.0;
            }
        }


        public bool IsItemUnread
        {
            get { return !mIsItemRead; }
        }


        public MailRecipient From
        {
            get
            {
                return mFrom;
            }

            set
            {
                mFrom = value;
                OnPropertyChanged("From");
            }
        }

        public MailRecipient[] Recipients
        {
            get
            {
                return mRecipients;
            }

            set
            {
                mRecipients = value;
                OnPropertyChanged("Recipients");
            }
        }

        public string MailSubject
        {
            get
            {
                return string.IsNullOrEmpty(mMailSubject) ? "(No Subject)" : mMailSubject;
            }

            set
            {
                if (mMailSubject != value)
                {
                    mMailSubject = value;
                    OnPropertyChanged("MailSubject");
                }
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
                if (Body != value)
                {
                    mBody = value;
                    OnPropertyChanged("Body");
                    OnPropertyChanged("HasBody");
                }
            }
        }

        public bool HasBody
        {
            get
            {
                return mBody != null;
            }
        }

        public bool IsLoading
        {
            get
            {
                return mIsLoading;
            }

            set
            {
                if (mIsLoading != value)
                {
                    mIsLoading = value;
                    OnPropertyChanged("IsLoading");
                    OnPropertyChanged("UIEnabled");
                }
            }
        }

        public bool UIEnabled
        {
            get
            {
                return !mIsLoading;
            }
        }

        public DateTime Timestamp
        {
            get
            {
                return mTimestamp;
            }

            set
            {
                if (mTimestamp != value)
                {
                    mTimestamp = value;
                    OnPropertyChanged("Timestamp");
                }
            }
        }

        public ISourceInfo Source { get; set; }

        public ObservableCollection<ViewMailLabelLink> Labels
        {
            get
            {
                return mLabels;
            }
        }

        internal bool CurrentLabelRemoved { get; set; }

        public void RollbackChanges()
        {
            if (mReadState.HasFlag(ViewMailMetaState.ReadFlagSet))
                IsItemRead = false;
            else if (!mReadState.HasFlag(ViewMailMetaState.ReadFlagUnset))
                IsItemRead = true;

            foreach(var i in mLabels)
            {
                if (i.MetaState.HasFlag(ViewMailMetaState.LabelRemoved))
                    i.Subscribed = true;
                else if (i.MetaState.HasFlag(ViewMailMetaState.LabelAdded))
                    i.Subscribed = false;
            }
        }

        public void ClearChanges()
        {
            mReadState = ViewMailMetaState.NoChange;
            LabelAddedCount = 0;
            LabelRemovedCount = 0;
            CurrentLabelRemoved = false;

            foreach (var i in mLabels)
            {
                i.MetaState = ViewMailMetaState.NoChange;
            }
        }

        public int CompareTo(ViewMailItem other)
        {
            if (ReferenceEquals(other, null))
                return 1;

            if (Draft != null && other.Draft != null)
                return Draft.DraftId.CompareTo(other.Draft.DraftId);

            return MailId.CompareTo(other.MailId);
        }

        public static bool operator ==(ViewMailItem lhs,ViewMailItem rhs)
        {
            if(ReferenceEquals(lhs,null))
                return ReferenceEquals(rhs, null);

            return lhs.Equals(rhs);
        }

        public static bool operator !=(ViewMailItem lhs, ViewMailItem rhs)
        {
            if (ReferenceEquals(lhs, null))
                return !ReferenceEquals(rhs, null);

            return !lhs.Equals(rhs);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
                return false;
            
            return CompareTo(obj as ViewMailItem) == 0;
        }

        public override int GetHashCode()
        {
            return MailId.GetHashCode();
        }
    }

    [Flags]
    public enum ViewMailMetaState
    {
        NoChange = 0,
        ReadFlagSet = 1 << 0,
        ReadFlagUnset = 1 << 1,
        LabelAdded = 1 << 2,
        LabelRemoved = 1 << 3,
        CurrentLabelRemoved = (1 << 4) | LabelRemoved,
    }

    [Flags]
    public enum MailOperationFlags
    {
        None = 0,
        Working = 1,
        ShowSpinner = (1 << 1) | Working,
        Removing = (1 << 2) | Working,
    }

    public class ViewMailLabelLink : UIObject
    {
        private bool mSubscribed;
        bool mCurrent;
        
        public ViewMailLabelLink(ViewMailItem item, ViewLabel label, bool subscribed,bool current)
        {
            Label = label;
            Mail = item;
            mSubscribed = subscribed;
            mCurrent = current;            
        }
        public ViewMailItem Mail { get; private set; }
        public ViewLabel Label { get; private set; }

        public ViewMailMetaState MetaState { get; internal set; }

        public bool Subscribed
        {
            get
            {
                return mSubscribed;
            }
            set
            {
                if (mSubscribed != value)
                {
                    mSubscribed = value;

                    if (value)
                    {
                        if (MetaState == ViewMailMetaState.LabelRemoved)
                        {
                            MetaState = ViewMailMetaState.NoChange;
                            Mail.LabelRemovedCount--;
                            if (mCurrent)
                                Mail.CurrentLabelRemoved = false;
                        }
                        else
                        {
                            MetaState = ViewMailMetaState.LabelAdded;
                            Mail.LabelAddedCount++;
                        }
                    }
                    else
                    {
                        if (MetaState == ViewMailMetaState.LabelAdded)
                        {
                            MetaState = ViewMailMetaState.NoChange;
                            Mail.LabelAddedCount--;
                        }
                        else
                        {
                            MetaState = ViewMailMetaState.LabelRemoved;
                            Mail.LabelRemovedCount++;

                            if (mCurrent)
                                Mail.CurrentLabelRemoved = true;
                        }
                    }
                    OnPropertyChanged("Subscribed");
                }
            }
        }
    }
    
}
