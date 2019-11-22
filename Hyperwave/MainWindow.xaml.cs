using Hyperwave.Accounts;
using Hyperwave.Config;
using Hyperwave.Controller;
using Hyperwave.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Hyperwave
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            cAccounts.ItemsSource = Client.ViewAccounts;
            cMailList.View = Client.MailView;

           

            Client.AccountAdded += Client_AccountAdded;
        }

        
        private void Client_AccountAdded(object sender, AccountOperationEventArgs e)
        {
            Client.UpdateAccount(e.AccountSource,false);
        }

        EveMailClient Client
        {
            get
            {
                return ((App)Application.Current).Client;
            }
        }

        private void cCreateAccount_Click(object sender, RoutedEventArgs e)
        {
            //cAccounts.Foreground = Brushes.Wheat;
            RegisterDialog dlg = new RegisterDialog();
            dlg.Owner = this;
            dlg.ShowDialog();
        }

        

        private async void cAccounts_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var info = cAccounts.SelectedItem as ISourceInfo;

            await Client.SetMailView(info);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Client.AccountAdded -= Client_AccountAdded;
            ((App)Application.Current).RemoveProcessCount();
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ((App)Application.Current).AddProcessCount();
        }

        private void cMailList_SelectedItemChanged(object sender, RoutedEventArgs e)
        {
            if (cMailList.SelectedItem == null)
            {
                cMailViewer.Visibility = Visibility.Hidden;
                cMailViewer.CurrentItem = null;
            }
            else
            {
                cMailViewer.Visibility = Visibility.Visible;
                cMailViewer.CurrentItem = cMailList.SelectedItem;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(DBOperation.OperationInProgress && Application.Current.Windows.Count == 1)
            {
                MessageBox.Show("Database operation is in progress");
                e.Cancel = true;
            }
        }

        private void AccountRemove_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement item = e.Source as FrameworkElement;
            ISourceInfo account = item.Tag as ISourceInfo;

            var result = MessageBox.Show("Are you sure to want to remove " + account.AccountSource.UserName + "?"
                , "Confirm Action", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

            if (result == MessageBoxResult.No)
                return;
            
            Client.RemoveAccount(account);
        }

        private void cRefresh_Click(object sender, RoutedEventArgs e)
        {
            Client.UpdateAccounts(false);
        }

        private void cUpgrade_Click(object sender, RoutedEventArgs e)
        {
            ShowUpgrade();
        }

        private void ShowUpgrade()
        {
            Admin.AboutDialog dlg = new Admin.AboutDialog(1);
            dlg.Owner = this;
            dlg.ShowDialog();
        }

        private void cSettings_Click(object sender, RoutedEventArgs e)
        {
            Admin.SettingsDialog dlg = new Admin.SettingsDialog();
            dlg.Owner = this;
            dlg.ShowDialog();
        }

        private void cAbout_Click(object sender, RoutedEventArgs e)
        {
            Admin.AboutDialog dlg = new Admin.AboutDialog();
            dlg.Owner = this;
            dlg.ShowDialog();
        }

        private void cManageLabels_Click(object sender, RoutedEventArgs e)
        {
            LabelEditor.ManageLabelsDialog dlg = new LabelEditor.ManageLabelsDialog();
            dlg.Owner = this;
            dlg.Account = ((Button)sender).Tag as ViewAccount;
            dlg.ShowDialog();
        }
    }
}
