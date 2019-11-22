using Hyperwave.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Hyperwave.MailView
{
    /// <summary>
    /// Interaction logic for LabelEditor.xaml
    /// </summary>
    public partial class LabelEditor : Window
    {
        public LabelEditor()
        {
            InitializeComponent();
        }

        public ViewMailItem[] Items { get; set; }

        List<LabelEditItem> mLinks;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Dictionary<long, LabelEditItem> lookup = new Dictionary<long, LabelEditItem>();

            foreach(var i in Items[0].Source.Account.Labels)
            {
                if (i.IsVirtual)
                    continue;

                lookup.Add(i.Id, new LabelEditItem()
                {
                    Name = i.Name
                });
            }

            foreach(var i in Items)
            {
                foreach(var j in i.Labels)
                {
                    lookup[j.Label.Id].AddItem(j);
                }
            }

            mLinks = new List<LabelEditItem>(lookup.Values);
            cLabelList.ItemsSource = mLinks;
        }
        

        private static Controller.EveMailClient GetClient()
        {
            return ((App)Application.Current).Client;
        }

        private void cOK_Click(object sender, RoutedEventArgs e)
        {
            foreach(var i in mLinks)
            {
                i.Apply();
            }

            GetClient().SaveMailMetaData(Items);
            Close();
        }
    }

    class LabelEditItem : UIObject
    {
        List<ViewMailLabelLink> mItems = new List<ViewMailLabelLink>();
        bool mHasItemsIncluded = false;
        bool mHasItemsExcluded = false;
        bool? mIsChecked = false;
        private string mName;

        public void AddItem(ViewMailLabelLink link)
        {
            mItems.Add(link);
            mHasItemsIncluded |= link.Subscribed;
            mHasItemsExcluded |= !link.Subscribed;

            if (mHasItemsExcluded && mHasItemsIncluded)
                IsChecked = null;
            else if (mHasItemsIncluded)
                IsChecked = true;
            else if (mHasItemsExcluded)
                IsChecked = false;
        }

        public void Apply()
        {
            if (IsChecked == null)
                return;

            foreach(var i in mItems)
            {
                i.Subscribed = IsChecked.Value;
            }
        }

        public bool? IsChecked
        {
            get
            {
                return mIsChecked;
            }
            set
            {
                if(mIsChecked != value)
                {
                    mIsChecked = value;
                    OnPropertyChanged("IsChecked");
                }
            }
        }

        public string Name
        {
            get
            {
                return mName;
            }
            set
            {
                if (mName != value)
                {
                    mName = value;
                    OnPropertyChanged("Name");
                }
            }
        }
    }
}
