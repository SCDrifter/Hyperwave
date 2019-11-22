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

namespace Hyperwave.MailView
{
    /// <summary>
    /// Interaction logic for MailListView.xaml
    /// </summary>
    public partial class MailListView : UserControl
    {
        public MailListView()
        {
            InitializeComponent();
        }

        ViewMailItem mLastSelectedItem = null;

        public ViewMailItem SelectedItem
        {
            get
            {
                return (cList.SelectedItems.Count == 1) ? cList.SelectedItems[0] as ViewMailItem : null;
            }
        }

        public static readonly RoutedEvent mSelectedItemChangedEvent = EventManager.RegisterRoutedEvent(
            "SelectedItemChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MailListView));

        public ViewModel.MailView View
        {
            get { return DataContext as ViewModel.MailView; }
            set
            {
                DataContext = value;
            }
        }

        public event RoutedEventHandler SelectedItemChanged
        {
            add { AddHandler(mSelectedItemChangedEvent, value); }
            remove { RemoveHandler(mSelectedItemChangedEvent, value); }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(mSelectedItemChangedEvent, this));

            var view = DataContext as ViewModel.MailView;

            
            if (!view.IsDraft && Properties.Settings.Default.MailReadAction == Config.EmailReadAction.AfterMessageLeave &&
                view.CanDelete && mLastSelectedItem != null && !mLastSelectedItem.IsItemRead)
            {
                mLastSelectedItem.IsItemRead = true;
                GetClient().SaveMailMetaData(mLastSelectedItem);
            }
            if (cList.SelectedItems == null || cList.SelectedItems.Count != 1)
                mLastSelectedItem = null;
            else
                mLastSelectedItem = cList.SelectedItem as ViewModel.ViewMailItem;
            
        }

        private static Controller.EveMailClient GetClient()
        {
            return ((App)Application.Current).Client;
        }

        private void cMarkUnread_Click(object sender, RoutedEventArgs e)
        {
            mLastSelectedItem = null;
            List<ViewMailItem> items = new List<ViewMailItem>(cList.SelectedItems.Count);
            foreach(ViewMailItem item in cList.SelectedItems)
            {
                item.IsItemRead = false;
                items.Add(item);
            }

            GetClient().SaveMailMetaData(items.ToArray());
        }

        private void cMarkRead_Click(object sender, RoutedEventArgs e)
        {
            List<ViewMailItem> items = new List<ViewMailItem>(cList.SelectedItems.Count);
            foreach (ViewMailItem item in cList.SelectedItems)
            {
                item.IsItemRead = true;
                items.Add(item);
            }

            GetClient().SaveMailMetaData(items.ToArray());
        }

        private void cEditLabels_Click(object sender, RoutedEventArgs e)
        {
            LabelEditor editor = new LabelEditor();
            editor.Items = (from object a in cList.SelectedItems select a as ViewMailItem).ToArray();
            editor.ShowDialog();
        }

        private void cList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (null != cList.GetObjectAtPoint<ListViewItem>(e.GetPosition(cList)))
            {
                ItemsActivated();
            }
        }

        private void ItemsActivated()
        {
            foreach (ViewMailItem i in cList.SelectedItems)
            {
                if(i.Draft != null)
                {
                    OpenDraft(i.Draft as DraftMessageSource);
                    continue;
                }
                MailViewWindow window = new MailViewWindow();
                window.Item = new MailNavigationNode(GetClient().MailView, i);
                window.Show();
            }
        }

        private void OpenDraft(DraftMessageSource draft)
        {
            MailWriter.MailWriter writer;

            if (GetClient().OpenDraft(draft, out writer))
                writer.Show();
        }

        private void cList_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
                ItemsActivated();
        }

        private void cNewMail_Click(object sender, RoutedEventArgs e)
        {
            DraftMessageSource source = GetClient().CreateDraft();
            MailWriter.MailWriter writer;

            GetClient().OpenDraft(source, out writer);
            writer.Show();
        }

        private void cDelete_Click(object sender, RoutedEventArgs e)
        {
            List<ViewMailItem> items = new List<ViewMailItem>(from ViewMailItem i in cList.SelectedItems select i);
            GetClient().DeleteMail(items);
        }

        private void cReply_Click(object sender, RoutedEventArgs e)
        {
            DraftMessageSource source = GetClient().CreateDraft((ViewMailItem)cList.SelectedItem,DraftType.Reply);
            MailWriter.MailWriter writer;

            GetClient().OpenDraft(source, out writer);
            writer.Show();
        }

        private void cReplyAll_Click(object sender, RoutedEventArgs e)
        {
            DraftMessageSource source = GetClient().CreateDraft((ViewMailItem)cList.SelectedItem, DraftType.ReplyAll);
            MailWriter.MailWriter writer;

            GetClient().OpenDraft(source, out writer);
            writer.Show();
        }

        private void cForward_Click(object sender, RoutedEventArgs e)
        {
            DraftMessageSource source = GetClient().CreateDraft((ViewMailItem)cList.SelectedItem, DraftType.Forward);
            MailWriter.MailWriter writer;

            GetClient().OpenDraft(source, out writer);
            writer.Show();
        }

        private async void cDownloadMore_Click(object sender, RoutedEventArgs e)
        {
            await GetClient().UpdateMailView();
        }

        private void HeaderButton_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            MailViewColumn col = btn.Tag as MailViewColumn;
            col.View.SelectColumn(col);
        }
    }
}
