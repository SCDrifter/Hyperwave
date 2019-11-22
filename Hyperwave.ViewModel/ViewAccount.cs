using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;
using System.Linq;
using System;
using System.Collections.Generic;

namespace Hyperwave.ViewModel
{
    public class ViewAccount : TreeNode,ISourceInfo
    {
        private ObservableCollection<ViewLabel> mFilters = new ObservableCollection<ViewLabel>();

        string mUserName;
        string mImageUrl;
        int mUnreadCount;
        private bool mShowSpinner;
        private AccountState mAccountState;

        IAccountSource mSource;

        public void Sync(IAccountSource source)
        {
            mSource = source;
            UserName = source.UserName;
            ImageUrl = source.ImageUrl;
            AccountState = source.AccountState;
            UnreadCount = source.UnreadCount;
            IsExpanded = source.IsExpanded;
            Id = source.Id;

            var label_dict = mFilters.ToDictionary((l) => l.Id);
            var delete = new List<ViewLabel>(mFilters);
            var order = new List<ViewLabel>(source.Labels.Length);
            
            foreach(var i in source.Labels)
            {
                ViewLabel label;

                if (!label_dict.TryGetValue(i.Id, out label))
                {
                    label = new ViewLabel(this, i);
                    mFilters.Add(label);
                    order.Add(label);
                    continue;
                }

                delete.Remove(label);
                order.Add(label);
                label.Sync(this, i);
            }

            foreach(var i in delete)
            {
                mFilters.Remove(i);
            }

            for(int i = 0;i < order.Count;i++)
            {
                int index = mFilters.IndexOf(order[i]);

                if (index == i)
                    continue;
                mFilters.Move(index, i);
            }
        }

        public string UserName
        {
            get { return mUserName; }
            set
            {
                if(mUserName != value)
                {
                    mUserName = value;
                    OnPropertyChanged("UserName");
                }
            }
        }
        public string ImageUrl
        {
            get { return mImageUrl; }
            set
            {
                if (mImageUrl != value)
                {
                    mImageUrl = value;
                    OnPropertyChanged("ImageUrl");
                }
            }
        }
        public bool ShowSpinner
        {
            get
            {
                return mShowSpinner;
            }
            set
            {
                if (mShowSpinner != value)
                {
                    mShowSpinner = value;
                    OnPropertyChanged("ShowSpinner");
                    OnPropertyChanged("ShowUnreadItems");
                }
            }
        }

        public long Id { get; private set; }

        public AccountState AccountState
        {
            get
            {
                return mAccountState;
            }
            set
            {
                if(mAccountState != value)
                {
                    mAccountState = value;
                    OnPropertyChanged("AccountState");
                    OnPropertyChanged("ShowUnreadItems");
                }
            }
        }
        public bool ShowUnreadItems
        {
            get { return mAccountState == AccountState.Online && !mShowSpinner && mUnreadCount > 0; }
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
                    OnPropertyChanged("ShowUnreadItems");
                }
            }
        }
        public ObservableCollection<ViewLabel> Labels { get { return mFilters; } }

        ViewAccount ISourceInfo.Account
        {
            get
            {
                return this;
            }
        }

        ViewLabel ISourceInfo.Label
        {
            get
            {
                return null;
            }
        }

        IAccountSource ISourceInfo.AccountSource
        {
            get
            {
                return mSource;
            }
        }

        ILabelSource ISourceInfo.LabelSource
        {
            get
            {
                return null;
            }
        }
    }

    public enum AccountState
    {
        Offline,
        Online,
        Failed
    }
}
