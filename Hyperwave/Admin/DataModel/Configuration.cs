using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hyperwave.Properties;

namespace Hyperwave.Admin.DataModel
{
    class Configuration : RootConfigPage
    {
        ConfigPage mLastSelectedPage = null;
        
        public override ConfigPage LastSelectedPage
        {
            get
            {
                return mLastSelectedPage;
            }

            set
            {
                if (mLastSelectedPage == value || value.RootPage != this)
                    return;
                mLastSelectedPage = value;
                OnPropertyChanged("LastSelectedPage");
                OnPropertyChanged("Name");
            }
        }
        
        public override string Name
        {
            get
            {
                return mLastSelectedPage?.Name ?? "Configuration";
            }
        }

        protected override Uri ImageUri
        {
            get
            {
                return new Uri("pack://application:,,,/Images/Icons/32/Configure.png");
            }
        }

        public override void Apply()
        {
            foreach(var i in SubPages)
            {
                i.Apply();
            }
            Settings.Default.Save();
            OnPropertyChanged("HasChanged");
        }

        protected override ConfigPage[] CreateSubPages()
        {
            return new ConfigPage[]
            {
                new MailCheckPage(this)
                {
                    IsSelected = true
                },
                new MailReadPage(this),
                new ColorThemePage(this)
            };
        }

        public MailCheckPage MailCheck { get { return SubPages[0] as MailCheckPage; } }
        public MailReadPage MailRead { get { return SubPages[1] as MailReadPage; } }
        public ColorThemePage ColorTheme { get { return SubPages[2] as ColorThemePage; } }
    }
}
