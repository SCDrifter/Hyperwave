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
using Hyperwave.Auth;
using System.ComponentModel;
using System.Threading;

namespace Hyperwave.Accounts
{
    /// <summary>
    /// Interaction logic for RegisterDialog.xaml
    /// </summary>
    public partial class RegisterDialog : Window
    {
        class AccountOptions  : INotifyPropertyChanged
        {
            private bool mUseBuiltinBrowser;

            public AccessFlag Flags { get; private set; }

            public event PropertyChangedEventHandler PropertyChanged;

            public AccountOptions()
            {
                mUseBuiltinBrowser = CanUseBuiltinBrowser;
            }


            bool SetOption(AccessFlag flag,bool enabled)
            {
                if (Flags.HasFlag(flag) == enabled)
                    return false;

                if (enabled)
                    Flags |= flag;
                else
                    Flags &= ~flag;

                return true;
            }

            protected virtual void OnPropertyChanged(string property_name)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property_name));
            }

            public bool MailWrite
            {
                get { return Flags.HasFlag(AccessFlag.MailWrite); }
                set
                {
                    if (SetOption(AccessFlag.MailWrite, value))
                        OnPropertyChanged("MailWrite");
                }
            }

            public bool MailSend
            {
                get { return Flags.HasFlag(AccessFlag.MailSend); }
                set
                {
                    if (SetOption(AccessFlag.MailSend, value))
                        OnPropertyChanged("MailSend");
                }
            }
            public bool ContactsRead
            {
                get { return Flags.HasFlag(AccessFlag.ContactsRead); }
                set
                {
                    if (SetOption(AccessFlag.ContactsRead, value))
                        OnPropertyChanged("ContactsRead");
                }
            }

            public bool ContactsWrite
            {
                get { return Flags.HasFlag(AccessFlag.ContactsWrite); }
                set
                {
                    if (SetOption(AccessFlag.ContactsWrite, value))
                        OnPropertyChanged("ContactsWrite");
                }
            }

            public bool CalendarRead
            {
                get { return Flags.HasFlag(AccessFlag.CalendarRead); }
                set
                {
                    if (SetOption(AccessFlag.CalendarRead, value))
                        OnPropertyChanged("CalendarRead");
                }
            }

            public bool CalendarWrite
            {
                get { return Flags.HasFlag(AccessFlag.CalendarWrite); }
                set
                {
                    if (SetOption(AccessFlag.CalendarWrite, value))
                        OnPropertyChanged("CalendarWrite");
                }
            }

            public bool UseBuiltinBrowser
            {
                get { return mUseBuiltinBrowser; }
                set
                {
                    if (mUseBuiltinBrowser == value)
                        return;
                    mUseBuiltinBrowser = value;
                    OnPropertyChanged("UseBuiltinBrowser");
                }
            }

            public bool CanUseBuiltinBrowser => Environment.Is64BitProcess;
        }

        AccountOptions mOptions = new AccountOptions()
        {
            MailSend = true,
            MailWrite = true
        };
        
        CancellationTokenSource mCancel = null;
        bool mIsCancelling = false;
        private SSOLoginController mLogin;

        public RegisterDialog()
        {
            InitializeComponent();
            DataContext = mOptions;
        }

        private async void cCreateAccount_Click(object sender, RoutedEventArgs e)
        {
            cCreationPage.Visibility = Visibility.Hidden;
            cActionPage.Visibility = Visibility.Visible;
            cActionText.Text = "Waiting for character login.";

            mLogin = new SSOLoginController()
            {
                AccessFlags = mOptions.Flags,
                PrivateData = "privatedata",
                UseBuildinBrowser = mOptions.UseBuiltinBrowser
            };

            await mLogin.Run();

            if (!mLogin?.Success ?? false)
            {
                cCreationPage.Visibility = Visibility.Visible;
                cActionPage.Visibility = Visibility.Hidden;
                return;
            }
            string authcode = mLogin.AuthCode;
            string challenge_code = mLogin.ChallengeCode;
            mLogin = null;

            await Authorize(authcode,challenge_code);

        }

        private async Task Authorize(string authcode,string challenge_code)
        {
            try
            {
                mCancel = new CancellationTokenSource();
                cActionText.Text = "Authorizing account.";
                TokenInfo tinfo = await SSOAuth.GetTokenInfoAsync(authcode,challenge_code,mCancel.Token);

                cActionText.Text = "Retrieving Character information.";
                CharacterInfo cinfo = await SSOAuth.GetCharacterInfoAsync(tinfo.AccessToken, mCancel.Token);

                await ((App)Application.Current).Client.AddAccountAsync(tinfo, cinfo, mOptions.Flags);
            }
            catch(TaskCanceledException)
            {                
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message, "Unable to add account");
                mIsCancelling = false;
                mCancel = null;

                cCreationPage.Visibility = Visibility.Visible;
                cActionPage.Visibility = Visibility.Hidden;

                return;
            }

            mIsCancelling = false;
            mCancel = null;
            Close();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (mLogin?.IsOpen ?? false)
            {
                mLogin.Close();
            }
            else if (mIsCancelling)
            {
                e.Cancel = true;
            }
            else if(mCancel != null)
            {
                mIsCancelling = true;
                mCancel.Cancel();
                e.Cancel = true;
            }
            
        }
    }
}
