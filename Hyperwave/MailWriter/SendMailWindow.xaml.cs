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
using Hyperwave.Controller;
using System.Windows.Interop;
using System.Runtime.InteropServices;

namespace Hyperwave.MailWriter
{
    /// <summary>
    /// Interaction logic for SendMailWindow.xaml
    /// </summary>
    public partial class SendMailWindow : Window
    {
        public SendMailWindow()
        {
            InitializeComponent();

        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32.dll")]
        private static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);

        private const uint MF_BYCOMMAND = 0x00000000;
        private const uint MF_GRAYED = 0x00000001;
        private const uint SC_CLOSE = 0xF060;
        
        bool mEnableClose = true;

        bool EnableClose
        {
            get
            {
                return mEnableClose;
            }
            set
            {
                if (mEnableClose == value)
                    return;

                mEnableClose = value;

                var hWnd = new WindowInteropHelper(this);
                var sysMenu = GetSystemMenu(hWnd.Handle, false);

                if (mEnableClose)
                    EnableMenuItem(sysMenu, SC_CLOSE, MF_BYCOMMAND);
                else
                    EnableMenuItem(sysMenu, SC_CLOSE, MF_BYCOMMAND | MF_GRAYED);
            }
        }

        DraftMessageSource mDraft;
        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ((App)App.Current).AddProcessCount();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            ((App)App.Current).RemoveProcessCount();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            EnableClose = false;

            
        }


        internal async Task SendMail(DraftMessageSource source)
        {
            Show();

            await ResendMail(source);
        }

        private async Task ResendMail(DraftMessageSource source)
        {
            EnableClose = false;
            cProgress.Visibility = Visibility.Visible;
            cErrorPanel.Visibility = Visibility.Collapsed;

            if (await((App)Application.Current).Client.SendMail(source))
            {
                Close();
                return;
            }
            EnableClose = true;
            cProgress.Visibility = Visibility.Collapsed;
            cErrorPanel.Visibility = Visibility.Visible;

            mDraft = source;

            if (Config.ExceptionHandler.LastException == null)
                cDetails.Text = "Unable to authorize account";
            else
                cDetails.Text = Config.ExceptionHandler.LastException;
        }

        private async void cRetry_Click(object sender, RoutedEventArgs e)
        {
            await ResendMail(mDraft);
        }

        private void cCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
