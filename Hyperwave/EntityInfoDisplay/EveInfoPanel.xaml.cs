using Hyperwave.Controller;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hyperwave.EntityInfoDisplay
{
    /// <summary>
    /// Interaction logic for EveInfoPanel.xaml
    /// </summary>
    public partial class EveInfoPanel : UserControl
    {
        const int HISTORY_LENGTH = 10;
        EntityData[] mHistory = new EntityData[HISTORY_LENGTH];

        int mVirtualIndex = 0;
        int mVirtualSize = 0;

        EntityData mCurrentItem = null;

        public EveInfoPanel()
        {
            InitializeComponent();
            DataContext = null;
        }

        int ActualIndex
        {
            get
            {
                return mVirtualIndex % HISTORY_LENGTH;
            }
        }

        int FirstItemIndex
        {
            get
            {
                return Math.Max(0, mVirtualSize - HISTORY_LENGTH + 1);
            }
        }

        int LastItemIndex
        {
            get
            {
                return Math.Max(0, mVirtualSize - 1);
            }
        }

        private static EveMailClient GetClient()
        {
            return ((App)Application.Current).Client;
        }

        public void SetItem(MailRecipient subject)
        {
            mVirtualSize = 1;
            mVirtualIndex = 0;

            bool is_new;

            EntityData data = GetClient().InfoCache.GetData(subject, out is_new);
                        
            SetItem(data);
        }



        public void AddItem(MailRecipient subject)
        {
            mVirtualIndex++;
            mVirtualSize = mVirtualIndex + 1;
            
            bool is_new;

            EntityData data = GetClient().InfoCache.GetData(subject, out is_new);

            SetItem(data);
        }

        private void SetItem(EntityData data)
        {
            mHistory[ActualIndex] = data;

            UpdateItem();
        }

        private void UpdateText()
        {
            if (mCurrentItem.HasDescription)
            {
                Hyperlink[] links;
                cDescript.Document = EveMarkupLanguage.ConvertToFlowDocument(mCurrentItem.Description, out links, cBackButton.FontFamily, Application.Current.Resources["WebBrowserBackgroundBrush"] as Brush, 16);
            }
        }

        private void mCurrentItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            UpdateText();
        }

        private void UpdateItem()
        {
            if (mCurrentItem != null)
                mCurrentItem.UnregisterHandler("Description", mCurrentItem_PropertyChanged);

            mCurrentItem = mHistory[ActualIndex];
            UpdateText();

            mCurrentItem.RegisterHandler("Description", mCurrentItem_PropertyChanged);

            DataContext = mCurrentItem;

            cNavigationPane.Visibility = mVirtualSize > 1 ? Visibility.Visible : Visibility.Collapsed;
            cBackButton.IsEnabled = mVirtualIndex > FirstItemIndex;
            cForwardButton.IsEnabled = mVirtualIndex < LastItemIndex;
        }

        private void cBackButton_Click(object sender, RoutedEventArgs e)
        {
            mVirtualIndex = Math.Max(FirstItemIndex, mVirtualIndex - 1);
            UpdateItem();
        }

        private void cForwardButton_Click(object sender, RoutedEventArgs e)
        {
            mVirtualIndex = Math.Min(LastItemIndex, mVirtualIndex + 1);
            UpdateItem();
        }

        private void WebLink_Navigate(object sender, MouseButtonEventArgs e)
        {
        }

        private void Entity_Navigate(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement el = (FrameworkElement)sender;
            MailRecipient recipient = (MailRecipient)el.Tag;

            AddItem(recipient);
        }

        private async void cDescript_Click(object sender, RoutedEventArgs e)
        {
            await OnLink((Hyperlink)e.OriginalSource);
        }

        async Task OnLink(Hyperlink link)
        {
            var action = await GetClient().ClassifyLink(link.NavigateUri);

            switch (action.Action)
            {
                default:
                case LinkAction.None:
                    MessageBox.Show("Unable to access this feature out of client.");
                    break;
                case LinkAction.HandledInternally:
                    break;
                case LinkAction.ShowEntity:
                    AddItem(action.Recipient);
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

        private void cSendMail_Click(object sender, RoutedEventArgs e)
        {
            DraftMessageSource msg = GetClient().CreateDraft();
            msg.AddRecipients(mCurrentItem.Subject);
            MailWriter.MailWriter writer;
            GetClient().OpenDraft(msg, out writer);

            writer.Show();

        }
    }

    public class PropertyTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement el = container as FrameworkElement;

            DataTemplate ret = null;

            if (item is WebLinkProperty)
                ret = el.FindResource("WebLinkPropertyTemplate") as DataTemplate; 
            else if (item is UIProperty<double>)
                ret = el.FindResource("DoublePropertyTemplate") as DataTemplate;
            else if (item is UIProperty<DateTime>)
                ret = el.FindResource("DateTimePropertyTemplate") as DataTemplate;
            else if (item is UIProperty<string>)
                ret = el.FindResource("StringPropertyTemplate") as DataTemplate;
            else if (item is UIProperty<MailRecipient>)
                ret = el.FindResource("MailRecipientPropertyTemplate") as DataTemplate;

            return ret;
        }
    }
}
