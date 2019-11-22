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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Hyperwave.ViewModel;
using Hyperwave.Controller;
using Hyperwave.Shell;

namespace Hyperwave.MailView
{
    /// <summary>
    /// Interaction logic for MailViewer.xaml
    /// </summary>
    public partial class MailBodyView : UserControl
    {
        Hyperlink[] mLinks = null;

        public const double BASE_FONT_SIZE = 16.0;

        public MailBodyView()
        {
            InitializeComponent();
        }
        


        EveMailClient Client
        {
            get
            {
                return ((App)Application.Current).Client;
            }
        }

        public static readonly DependencyProperty CurrentItemProperty = 
            DependencyProperty.Register("CurrentItem", typeof(ViewMailItem), typeof(MailBodyView)
                ,new PropertyMetadata(null, OnCurrentItemChanged));

        public ViewMailItem CurrentItem
        {
            get { return (ViewMailItem)GetValue(CurrentItemProperty); }
            set
            {
                SetValue(CurrentItemProperty,value);
            }
        }

        static void OnCurrentItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (MailBodyView)d;
            control.LoadText();
        }

        public static readonly DependencyProperty AllowMarkReadProperty = DependencyProperty.Register("AllowMarkRead", typeof(bool), typeof(MailBodyView));

        public bool AllowMarkRead
        {
            get { return (bool)GetValue(AllowMarkReadProperty); }
            set { SetValue(CurrentItemProperty, value); }
        }

        void HookupLinks()
        {
            if (mLinks == null)
                return;

            foreach(var link in mLinks)
            {
                link.Click += Link_Click;
            }
        }

        void ResetLinks()
        {
            if (mLinks == null)
                return;

            foreach (var link in mLinks)
            {
                link.Click -= Link_Click;
            }

            mLinks = null;
        }

        private async void Link_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink link = (Hyperlink)e.OriginalSource;

            //Clipboard.SetText(link.NavigateUri.ToString());

            var action = await GetClient().ClassifyLink(link.NavigateUri);

            switch(action.Action)
            {
                default:
                case LinkAction.None:
                    MessageBox.Show("Unable to access this feature out of client.");
                    break;
                case LinkAction.HandledInternally:
                    break;
                case LinkAction.ShowEntity:
                    cInfoPanel.SetItem(action.Recipient);
                    cInfoPopup.IsOpen = true;
                    break;
                case LinkAction.WebLink:
                    OpenWebLink(action.Url);
                    break;
            }
        }

        private void OpenWebLink(Uri url)
        {
            var progstart = new System.Diagnostics.ProcessStartInfo()
            {
                UseShellExecute = true,
                FileName = url.ToString()
            };

            System.Diagnostics.Process.Start(progstart);
        }

        private void LoadText()
        {
            if (CurrentItem == null)
                return;

            Task task;
            if (CurrentItem.HasBody)
                LoadHtml(CurrentItem.Body);
            else
            {
                task = DownloadText(CurrentItem);
            }
        }

        private async Task DownloadText(ViewMailItem item)
        {
            item.RegisterHandler("IsLoading",Item_Downloaded);

            if (item.IsLoading)
                return;

            item.IsLoading = true;

            await Client.LoadMailBody(item);

            item.IsLoading = false;
        }

        private void Item_Downloaded(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ViewMailItem item = sender as ViewMailItem;

            if (item.IsLoading)
                return;

            item.UnregisterHandler("IsLoading", Item_Downloaded);
            
            if (item != CurrentItem)
                return;

            if (!item.HasBody)
                LoadHtml("(Unable to load message)");
            else
            {
                LoadHtml(item.Body);
                //var notifytest = new Notification()
                //{
                //    Text = new string[]
                //    {
                //    "New Eve-Mail",
                //    item.MailSubject,
                //    item.From.Name,
                //    ""
                //    },
                //    ImageUrl = new Uri(item.From.ImageUrl128),
                //    AppID = App.APPID
                //};

                //notifytest.Show();
            }

            
        }

        private void LoadHtml(string html)
        {
            var view = GetClient().MailView;

            ResetLinks();

            cContent.Document = EveMarkupLanguage.ConvertToFlowDocument(html,out mLinks, cMailSubject.FontFamily, Application.Current.Resources["WebBrowserBackgroundBrush"] as Brush,BASE_FONT_SIZE);
            cContent.Document.ColumnWidth = SystemParameters.PrimaryScreenWidth;            

            HookupLinks();

            if (AllowMarkRead && CurrentItem.Draft == null && Properties.Settings.Default.MailReadAction == Config.EmailReadAction.BeforeMessageLoad &&
                view.CanDelete && CurrentItem != null && !CurrentItem.IsItemRead)
            {
                CurrentItem.IsItemRead = true;
                GetClient().SaveMailMetaData(CurrentItem);
            }
        }

        private static EveMailClient GetClient()
        {
            return ((App)Application.Current).Client;
        }

        private void cSender_MouseUp(object sender, MouseButtonEventArgs e)
        {
            cInfoPanel.SetItem(CurrentItem.From);

            if (CurrentItem.From.Type == Common.EntityType.Mailinglist)
                return;

            cInfoPopup.IsOpen = true;
        }

        private void cTo_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var item = (MailRecipient)((FrameworkElement)sender).Tag;
            if (item.Type == Common.EntityType.Mailinglist)
                return;

            cInfoPanel.SetItem(item);
            cInfoPopup.IsOpen = true;
        }

        private void cShowSource_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void UserControl_LayoutUpdated(object sender, EventArgs e)
        {
            if(cContent.Document != null)
                cContent.Document.PageWidth = ActualWidth - 32;
        }
    }
}
