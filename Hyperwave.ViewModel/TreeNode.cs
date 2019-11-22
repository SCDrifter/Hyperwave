using System.ComponentModel;

namespace Hyperwave.ViewModel
{
    public class TreeNode : UIObject
    {
        bool mIsExpanded;
        bool mIsSelected;

        public bool IsExpanded
        {
            get { return mIsExpanded; }
            set
            {
                if(value != mIsExpanded)
                {
                    mIsExpanded = value;
                    OnPropertyChanged("IsExpanded");
                }
            }
        }
        public bool IsSelected
        {
            get { return mIsSelected; }
            set
            {
                if (value != mIsSelected)
                {
                    mIsSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }
        }

        
    }
}
