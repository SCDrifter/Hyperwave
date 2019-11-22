using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperwave.ViewModel
{
    public class MailView : UIObject
    {
        ObservableCollection<ViewMailItem> mMailItems = new ObservableCollection<ViewMailItem>();
        bool mCanSend = true;
        bool mCanDelete = true;
        bool mCanDownload = true;
        bool mIsLoading = false;
        bool mHasItems = false;
        long mLastMailId = -1;
        private MailOperationFlags mOperationFlags;
        private bool mIsDraft;
        private MailViewColumn mSelectedColumn;

        public MailView()
        {
            SubjectColumn = new MailViewColumn(this, "Subject", SortCompare_Subject);
            FromColumn = new MailViewColumn(this, "From", SortCompare_From);
            TimeColumn = new MailViewColumn(this, "Recieved", SortCompare_Time);
            SelectColumn(TimeColumn);
        }

        int SortCompare_From(ViewMailItem i1,ViewMailItem i2)
        {
            return (i1.From.Name ?? "").CompareTo(i2.From.Name ?? "");
        }

        int SortCompare_Subject(ViewMailItem i1, ViewMailItem i2)
        {
            return (i1.MailSubject ?? "").CompareTo(i2.MailSubject ?? "");
        }

        int SortCompare_Time(ViewMailItem i1, ViewMailItem i2)
        {
            return i1.Timestamp.CompareTo(i2.Timestamp);
        }

        public void Reset()
        {
            CanSend = true;
            CanDelete = true;
            CanDownload = true;
            HasItems = false;
            IsLoading = false;
            IsDraft = false;
            LastMailId = -1;
            NotifyMailListChanging();
            mMailItems.Clear();
        }

        public ISourceInfo Source { get; set; }

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
                    OnPropertyChanged("CanDelete");
                }
            }
        }

        public event EventHandler MailListChanging;

        protected void NotifyMailListChanging()
        {
            if (MailListChanging != null)
                MailListChanging(this, new EventArgs());
        }

        public ObservableCollection<ViewMailItem> MailItems
        {
            get
            {
                return mMailItems;
            }
        }

        public bool CanSend
        {
            get
            {
                return mCanSend;
            }

            set
            {
                if (mCanSend != value)
                {
                    mCanSend = value;
                    OnPropertyChanged("CanSend");
                }
            }
        }

        public bool CanDelete
        {
            get
            {
                return !mOperationFlags.HasFlag(MailOperationFlags.Working) && mCanDelete;
            }

            set
            {
                if (mCanDelete != value)
                {
                    mCanDelete = value;
                    OnPropertyChanged("CanDelete");
                }
            }
        }

        public bool CanDownload
        {
            get
            {
                return mCanDownload;
            }

            set
            {
                if (mCanDownload != value)
                {
                    mCanDownload = value;
                    OnPropertyChanged("CanDownload");
                }
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

        public bool IsDraft
        {
            get
            {
                return mIsDraft;
            }

            set
            {
                if (mIsDraft != value)
                {
                    mIsDraft = value;
                    OnPropertyChanged("IsDraft");
                }
            }
        }

        public bool HasItems
        {
            get
            {
                return mHasItems;
            }

            set
            {
                if (mHasItems != value)
                {
                    mHasItems = value;
                    OnPropertyChanged("HasItems");
                }
            }
        }

        public long LastMailId
        {
            get
            {
                return mLastMailId;
            }

            set
            {
                if (mLastMailId != value)
                {
                    mLastMailId = value;
                    OnPropertyChanged("LastMailId");
                }
            }
        }

        internal MailViewColumn SelectedColumn
        {
            get
            {
                return mSelectedColumn;
            }
        }

        public MailViewColumn TimeColumn { get; private set; }
        public MailViewColumn FromColumn { get; private set; }
        public MailViewColumn SubjectColumn { get; private set; }

        public void SelectColumn(MailViewColumn column)
        {
            if (mSelectedColumn == column)
                mSelectedColumn.AscendingOrder = !mSelectedColumn.AscendingOrder;
            else
            {
                mSelectedColumn = column;
                mSelectedColumn.AscendingOrder = false;
                OnPropertyChanged("SelectedColumn");
            }

            SortItems();
        }

        public void SortItems()
        {
            if (mMailItems.Count == 0)
                return;
            List<ViewMailItem> items = new List<ViewMailItem>(mMailItems);
            mSelectedColumn.Sort(items);
            
            for(int i =0;i < items.Count;i++)
            {
                mMailItems.Move(mMailItems.IndexOf(items[i]), i);
            }
        }

        public void UpdateWorkingItems(IEnumerable<ViewMailItem> items)
        {
            foreach(var i in items)
            {
                if (i.OperationFlags.HasFlag(MailOperationFlags.Removing))
                    MailItems.Remove(i);
                i.OperationFlags = MailOperationFlags.None;
            }
        }
    }

    public class MailViewColumn : UIObject
    {
        MailView mView;
        bool mAscendingOrder = false;
        bool mSelected = false;
        Comparison<ViewMailItem> mSort;

        internal MailViewColumn(MailView view,string name,Comparison<ViewMailItem> sortfunc)
        {
            mView = view;
            Name = name;
            mSort = sortfunc;
            mView.RegisterHandler("SelectedColumn", mView_PropertyChanged);
        }

        private void mView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Selected = this == mView.SelectedColumn;
        }

        public MailView View { get { return mView; } }

        public string Name { get; private set; }

        internal void Sort(List<ViewMailItem> items)
        {
            items.Sort((i1, i2) => mSort(i1, i2) * (mAscendingOrder ? 1 : -1));
        }

        public bool Selected
        {
            get
            {
                return mSelected;
            }
            private set
            {
                if (value == mSelected)
                    return;
                mSelected = value;
                OnPropertyChanged("Selected");
            }
        }

        public bool AscendingOrder
        {
            get
            {
                return mAscendingOrder;
            }
            set
            {
                if(mAscendingOrder != value)
                {
                    mAscendingOrder = value;
                    OnPropertyChanged("AscendingOrder");
                }
            }
        }
    }
}
