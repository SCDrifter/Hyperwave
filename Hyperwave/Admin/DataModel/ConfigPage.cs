using Hyperwave.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Hyperwave.Admin.DataModel
{
    abstract class ConfigPage : UIObject,IDisposable
    {
        ConfigPage mParent = null;
        RootConfigPage mRoot = null;
        bool mIsSelected;
        bool mIsExpanded;
        BitmapImage mImage;

        ConfigPage[] mSubItems = null;

        public ConfigPage(ConfigPage parent)
        {
            mParent = parent;
            mRoot = mParent?.RootPage;
            mParent?.RegisterHandler("IsSelected",mParent_IsSelectedChanged);
        }

        private void mParent_IsSelectedChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged("IsSelectedOrParentSelected");
        }
        
        protected static App GetApp()
        {
            return ((App)Application.Current);
        }

        protected abstract Uri ImageUri { get; }

        public BitmapImage Image
        {
            get
            {
                if (mImage == null)
                {
                    mImage = new BitmapImage(ImageUri);
                }
                return mImage;
            }
        }
        
        public abstract string Name { get; }

        public bool IsSelected
        {
            get
            {
                return mIsSelected;
            }
            set
            {
                if (mIsSelected == value)
                    return;
                mIsSelected = value;
                OnPropertyChanged("IsSelected");
                OnPropertyChanged("IsSelectedOrParentSelected");
                OnPropertyChanged("Visible");

                if (!mIsSelected)
                    OnUnselected();
                else
                {
                    RootPage.LastSelectedPage = this;
                    OnSelected();
                }
            }
        }

        protected override void OnPropertyChanged(string property_name)
        {
            base.OnPropertyChanged(property_name);
            switch(property_name)
            {
                case "IsSelected":
                case "IsSelectedOrParentSelected":
                case "Visible":
                case "IsExpanded":
                case "HasChanged":
                    break;
                default:
                    base.OnPropertyChanged("HasChanged");
                    break;
            }
        }

        protected virtual void OnSelected()
        {

        }

        protected virtual void OnUnselected()
        {

        }

        public virtual bool HasChanged
        {
            get
            {
                foreach (var i in SubPages)
                {
                    if (i.HasChanged)
                        return true;
                }

                return false;
            }
        }

        public virtual Visibility Visible
        {
            get
            {
                return IsSelected ? Visibility.Visible : Visibility.Hidden;
            }
        }

        public bool IsSelectedOrParentSelected
        {
            get
            {
                return IsSelected || (mParent != null && mParent.IsSelected);
            }
        }

        public ConfigPage[] SubPages
        {
            get
            {
                if (mSubItems == null)
                {
                    mSubItems = CreateSubPages();
                    OnSubPagesCreated(mSubItems);
                }
                return mSubItems;
            }
        }

        protected virtual void OnSubPagesCreated(ConfigPage[] subpages)
        {
            foreach(var i in subpages)
            {
                i.RegisterHandler("HasChanged",Child_HasChanged);
            }
        }
        

        private void Child_HasChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged("HasChanged");
        }

        protected virtual ConfigPage[] CreateSubPages()
        {
            return new ConfigPage[0];
        }

        public bool IsExpanded
        {
            get
            {
                return mIsExpanded;
            }
            set
            {
                if (mIsExpanded == value)
                    return;
                mIsExpanded = value;
                OnPropertyChanged("IsExpanded");
            }
        }

        public ConfigPage Parent
        {
            get
            {
                return mParent;
            }
        }

        public virtual RootConfigPage RootPage
        {
            get { return mRoot; }
        }

        public abstract void Apply();

        protected virtual void OnDisposed()
        {
            
        }
        public void Dispose()
        {
            OnDisposed();

            if (SubPages == null)
                return;

            foreach (var i in SubPages)
            {
                i.Dispose();
            }
                           
        }
    }

    abstract class RootConfigPage : ConfigPage
    {
        public RootConfigPage()
            :base(null)
        {

        }
        public abstract ConfigPage LastSelectedPage { get; set; }


        public override RootConfigPage RootPage
        {
            get
            {
                return this;
            }
        }
        
    }
}
