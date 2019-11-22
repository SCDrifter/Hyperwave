
using System;

namespace Hyperwave.ViewModel
{
    public class ViewLabel : TreeNode,ISourceInfo
    {
        string mName;
        LabelType mType;
        int mUnreadCount;
        private int mId;
        ISourceInfo mParent;
        ILabelSource mSource;
        bool mIsVirtual;

        //public ViewLabel()
        //{
        //}

        internal ViewLabel(ISourceInfo parent,ILabelSource source)
        {
            Sync(parent, source);
        }

        internal void Sync(ISourceInfo parent, ILabelSource source)
        {
            mParent = parent;
            mSource = source;
            Id = source.Id;
            Name = source.Name;
            Type = source.Type;
            UnreadCount = source.UnreadCount;
            IsVirtual = source.IsVirtual;
        }

        public int Id
        {
            get { return mId; }
            set
            {
                if (mId != value)
                {
                    mId = value;
                    OnPropertyChanged("Id");
                }
            }
        }

        public string Name
        {
            get { return mName; }
            set
            {
                if (mName != value)
                {
                    mName = value;
                    OnPropertyChanged("Name");
                }
            }
        }
        public LabelType Type
        {
            get { return mType; }
            set
            {
                if (mType != value)
                {
                    mType = value;
                    OnPropertyChanged("Type");
                }
            }
        }
        public bool HasUnreadItems
        {
            get { return mUnreadCount > 0; }

        }
        public int UnreadCount
        {
            get { return mUnreadCount; }
            set
            {
                if (mUnreadCount != value)
                {
                    mUnreadCount = value;
                    OnPropertyChanged("UnreadCount");
                    OnPropertyChanged("HasUnreadItems");
                }
            }
        }

        ViewAccount ISourceInfo.Account
        {
            get
            {
                return mParent.Account;
            }
        }

        ViewLabel ISourceInfo.Label
        {
            get
            {
                return this;
            }
        }

        IAccountSource ISourceInfo.AccountSource
        {
            get
            {
                return mParent.AccountSource;
            }
        }

        ILabelSource ISourceInfo.LabelSource
        {
            get
            {
                return mSource;
            }
        }

        public bool IsVirtual
        {
            get
            {
                return mIsVirtual;
            }
            private set
            {
                if(mIsVirtual != value)
                {
                    mIsVirtual = value;
                    OnPropertyChanged("IsVirtual");
                }
            }
        }
    }
}
