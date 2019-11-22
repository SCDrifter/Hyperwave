using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperwave.ViewModel
{
    public class MailNavigationNode : UIObject
    {
        private MailView mView = null;
        int mIndex = -1;
        private ViewMailItem mItem = null;
        IList<ViewMailItem> mList = null;

        public MailNavigationNode(MailView view,ViewMailItem item)
        {            
            mList = view.MailItems;
                        
            mIndex = mList.IndexOf(item);

            mItem = item;

            if (mIndex != -1)
            {
                mView = view;
                mView.MailListChanging += mView_MailListChanging;
            }
        }

        public MailNavigationNode(ViewMailItem item)
        {
            mList = new List<ViewMailItem>();
            mList.Add(item);
            mItem = item;
            mIndex = 0;
        }

        private void mView_MailListChanging(object sender, EventArgs e)
        {
            mView.MailListChanging -= mView_MailListChanging;
            mView = null;
            mList = new List<ViewMailItem>(mList);
            OnPropertyChanged("IsBackedByView");
        }

        public ViewMailItem Item
        {
            get
            {
                return mItem;
            }
        }

        public bool IsBackedByView
        {
            get { return mIndex != -1 && mView != null; }
        }

        public bool HasNext
        {
            get
            {
                return mIndex != -1 && mIndex < mList.Count - 1;
            }
        }

        public bool HasPrevious
        {
            get
            {
                return mIndex != -1 && mIndex > 0;
            }
        }

        public void MoveNext()
        {
            if (!HasNext)
                return;
            mIndex++;
            mItem = mList[mIndex];
            OnPropertyChanged("Item");
            OnPropertyChanged("HasNext");
            OnPropertyChanged("HasPrevious");
        }

        public void MovePrevious()
        {
            if (!HasPrevious)
                return;
            mIndex--;
            mItem = mList[mIndex];
            OnPropertyChanged("Item");
            OnPropertyChanged("HasNext");
            OnPropertyChanged("HasPrevious");
        }

        public void DeleteItem(ViewMailItem item)
        {
            if (IsBackedByView)
                return;

            mList.Remove(item);
            mIndex = mList.IndexOf(mItem);
        }
    }
}
