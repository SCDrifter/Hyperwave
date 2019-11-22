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
using System.Windows.Shapes;

namespace Hyperwave.MailView
{
    /// <summary>
    /// Interaction logic for MailViewWindow.xaml
    /// </summary>
    public partial class MailViewWindow : Window
    {
        public MailViewWindow()
        {
            InitializeComponent();
        }

        public MailNavigationNode Item
        {
            get
            {
                return DataContext as MailNavigationNode;
            }
            set
            {
                DataContext = value;
            }
        }

        private void ToggleLabel_Click(object sender, RoutedEventArgs e)
        {
            GetClient().SaveMailMetaData(Item.Item);
        }

        private static EveMailClient GetClient()
        {
            return ((App)Application.Current).Client;
        }

        private void cPrevious_Click(object sender, RoutedEventArgs e)
        {
            Item.MovePrevious();
        }

        private void cNext_Click(object sender, RoutedEventArgs e)
        {
            Item.MoveNext();
        }

        private void OpenDraft(DraftMessageSource source)
        {
            MailWriter.MailWriter writer;

            GetClient().OpenDraft(source, out writer);
            writer.Left = Left;
            writer.Top = Top;
            writer.Width = Width;
            writer.Height = Height;
            writer.Show();
            Close();
        }

        private void cForward_Click(object sender, RoutedEventArgs e)
        {
            DraftMessageSource source = GetClient().CreateDraft(Item.Item, DraftType.Forward);
            OpenDraft(source);
        }

        private void cReplyAll_Click(object sender, RoutedEventArgs e)
        {
            DraftMessageSource source = GetClient().CreateDraft(Item.Item, DraftType.ReplyAll);
            OpenDraft(source);
        }

        private void cReply_Click(object sender, RoutedEventArgs e)
        {
            DraftMessageSource source = GetClient().CreateDraft(Item.Item, DraftType.Reply);
            OpenDraft(source);
        }

        private void cDelete_Click(object sender, RoutedEventArgs e)
        {
            var item = Item.Item;
            if (Item.HasNext)
                Item.MoveNext();
            else
                Close();

            GetClient().DeleteMail(new ViewMailItem[] { item });

            Item.DeleteItem(item);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ((App)App.Current).AddProcessCount();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            ((App)App.Current).RemoveProcessCount();
        }
    }

    
}
