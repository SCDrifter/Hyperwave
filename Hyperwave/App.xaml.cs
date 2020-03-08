using Hyperwave.Config;
using Hyperwave.Controller;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Hyperwave.Shell;
using System.Windows.Interop;
using Hyperwave.Common;
using Hyperwave.UserCache;
using System.Windows.Threading;
using Hyperwave.Properties;
using System.Diagnostics;
using Microsoft.Win32;
using System.IO;
using Hyperwave.ViewModel;
using System.Web;
using Hyperwave.Notifications;
using System.Reflection;

namespace Hyperwave
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        DispatcherTimer mMailTimer = null;
        string mEvePath = null;
        int mProcessCount = 0;
        private SkinStyle mColorScheme;
        ServiceConnection mServiceLink = null;
        NLog.Logger mLog = NLog.LogManager.GetCurrentClassLogger();
        
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            mLog.Info("Application Instance Startup");
            if(Client.Commands.ConnectToExistingClient(50))
            {
                mLog.Info("Existing instance found forwarding commandline and exiting");
                if (e.Args.Length == 0)
                    Client.Commands.SendCommand("ShowWindow");
                else
                    Client.Commands.SendCommand(new ApplicationCommand(ParseUrlCmdline(e.Args)));

                Shutdown();
                return;
            }

            mServiceLink = new ServiceConnection(new Common.LoggerWrapperFactory());

            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            AddProcessCount();
            mColorScheme = Settings.Default.ColorScheme;
            
            
            Settings.Default.PropertyChanged += Settings_PropertyChanged;

            Client.Commands.CommandRecieved += Commands_CommandRecieved;
            Client.Commands.RegisterCommandHandler("ShowWindow", Command_ShowWindow);
            Client.Commands.RegisterCommandHandler("NewMail", Command_NewMail);
            Client.Commands.RegisterCommandHandler("CheckMail", Command_CheckMail);

            Client.Commands.RegisterCommandHandler("NoOp", delegate (object s, CommandEventArgs ce)
             {
             });


            Client.AccountNotification += mClient_AccountNotification;

            Client.ControllerIdle += mClient_ControllerIdle;
            Client.ControllerActive += mClient_ControllerActive;
            
            Task discard = Client.Commands.StartServerLoop();

            SetStyle(Settings.Default.ColorScheme);

            Client.Init();

            if (e.Args.Length == 0)
                Client.Commands.SendCommand("ShowWindow");
            else
            {
                Client.Commands.SendCommand(new ApplicationCommand(ParseUrlCmdline(e.Args)));
            }

            RemoveProcessCount();
        }

        private string[] ParseUrlCmdline(string[] args)
        {
            Uri url;
            if (args.Length != 1 || !Uri.TryCreate(args[0], UriKind.Absolute, out url))
                return args;
            mLog.Info($"Found URL: {url}");
            switch (url.Scheme)
            {
                case "eve-mail":
                    mLog.Info("Url is an eve-mail link");
                    return ParseEveMailUrl(url);
                default:
                    mLog.Warn("Unknown url type");
                    return new string[] { "NoOp" };
            }
        }

        private string[] ParseEveMailUrl(Uri url)
        {
            List<string> ret = new List<string>() { "NewMail" };
            
            switch(url.Host)
            {
                case "character":
                    ret.Add("-character");
                    break;
                case "corporation":
                    ret.Add("-corporation");
                    break;
                case "alliance":
                    ret.Add("-alliance");
                    break;
                case "mailinglist":
                    ret.Add("-mailinglist");
                    break;
                default:
                    return new string[] { "NoOp" };
            }

            ret.Add(string.Join(" ", url.LocalPath.Split("/".ToArray(), StringSplitOptions.RemoveEmptyEntries)));

            var query = HttpUtility.ParseQueryString(url.Query);
            foreach (string i in query)
            {
                switch(i)
                {
                    case "subject":
                        ret.Add("-subject");
                        ret.Add(query[i]);
                        break;
                    case "body":
                        ret.Add("-body");
                        ret.Add(query[i]);
                        break;
                }
            }

            return ret.ToArray();
        }

        private void mClient_ControllerActive(object sender, EventArgs e)
        {
            AddProcessCount();
        }

        private void mClient_ControllerIdle(object sender, EventArgs e)
        {
            RemoveProcessCount();
        }

        private void Settings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            
            switch(e.PropertyName)
            {
                case "ColorScheme":
                    if (Settings.Default.ColorScheme != mColorScheme)
                    {
                        SetStyle(Settings.Default.ColorScheme);
                        mColorScheme = Settings.Default.ColorScheme;
                    }
                    break;
                
            }
        }
               

        private Window GetMsgBoxOwner()
        {
            Window best_candidate = null;

            foreach(Window i in this.Windows)
            {
                if (i.Owner != null)
                    continue;
                if (i.IsActive)
                    return i;
                best_candidate = i;
            }

            if (MailWindow != null)
                return MailWindow;

            return best_candidate;
        }

        void StartTimer()
        {
            if (mMailTimer != null)
                return;

            Client.UpdateAccounts(false);

            if (Settings.Default.ShowNotifications)
                Client.CheckMails();

            mMailTimer = new DispatcherTimer();
            mMailTimer.Interval = TimeSpan.FromSeconds((double)Settings.Default.MailCheckInterval);
            mMailTimer.Tick += mMailTimer_Tick;

            mMailTimer.Start();
        }

        internal void RemoveProcessCount()
        {
            bool stop = (--mProcessCount <= 0);
            mLog.Info($"Process activity count decreased to {mProcessCount}.");
            if(stop)
                Shutdown();

        }

        internal void AddProcessCount()
        {
            mProcessCount++;
            mLog.Info($"Process activity count increased to {mProcessCount}.");
        }

        

        private void mMailTimer_Tick(object sender, EventArgs e)
        {
            mLog.Info("Checking Mails");
            Client.UpdateAccounts(true);

            if (Settings.Default.ShowNotifications)
                Client.CheckMails();
        }

        bool IsEveClientRunning()
        {
            if (mEvePath == null)
                GetEvePath();

            bool ret = false;

            foreach(var i in Process.GetProcessesByName("exefile.exe"))
            {
                if (i.Modules[0].FileName.Equals(mEvePath, StringComparison.InvariantCultureIgnoreCase))
                    ret = true;
                i.Dispose();
            }

            mLog.Info($"IsEveClientRunning()={ret}");

            return ret;
        }

        private void GetEvePath()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\CCP\EVEONLINE"))
            {
                mEvePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Eve\SharedCache");

                if (key != null)
                    mEvePath = key.GetValue("CACHEFOLDER", mEvePath).ToString();
                mEvePath = Path.Combine(mEvePath, @"tq\bin\exefile.exe");
            }
        }

        private void mClient_AccountNotification(object sender, AccountNotificationEventArgs args)
        {
            if (Settings.Default.SupressNotificationsClient && IsEveClientRunning())
                return;

            if (args.NewMails != null)
            {
                mLog.Info($"{args.NewMails.Length} new mails notifications");
                if (args.NewMails.Length > 2)
                {
                    Notification notify = new Notification()
                    {
                        ImageUrl = args.ForAccount.ImageUrl.ImageUrlToUri(UriKind.Absolute),
                        SoundUrl = "@://Sound/NewMails.wav".SoundUrlToUri(UriKind.Absolute),
                        Title = string.Format("{0} new Eve-Mails for character {1}",args.NewMails.Length,args.ForAccount.UserName)                        
                    };

                    notify.NotificationActivated += (o,e) => Client.Commands.SendCommand("ShowWindow", args.ForAccount.Id.ToString());
                    notify.Show();
                }
                else
                {
                    EventHandler openmail = delegate (object nsender,EventArgs e)
                    {
                        Notification n = nsender as Notification;

                        var mail = n.ContextData as ViewMailItem;
                        MailNavigationNode node = new MailNavigationNode(mail);
                        MailView.MailViewWindow window = new MailView.MailViewWindow()
                        {
                            Item = node
                        };

                        window.Show();

                        mail.IsItemRead = true;

                        Client.SaveMailMetaData(mail);
                    };
                    foreach(var item in args.NewMails)
                    {
                        var notify = new Notification()
                        {
                            Title = item.MailSubject,
                            Line1 = item.From.Name,
                            Line2 = args.ForAccount.UserName,
                            SoundUrl = "@://Sound/NewMail.wav".SoundUrlToUri(UriKind.Absolute),
                            ContextData = item
                        };
                        if (item.From.Type == EntityType.Mailinglist)
                            notify.ImageUrl = item.From.ImageUrl128.ImageUrlToUri(UriKind.Absolute);
                        else
                            notify.ImageUrl = new Uri(item.From.ImageUrl128);

                        notify.NotificationActivated += openmail;

                        notify.Show();
                    }
                }
            }
        }
        
        private void Commands_CommandRecieved(object sender, CommandEventArgs e)
        {
            mLog.Info($"Invalid commandline argument {e.Command.Command}");
            MessageBox.Show($"Invalid commandline argument {e.Command.Command}");
        }
                        

        private void Command_CheckMail(object sender, CommandEventArgs e)
        {
            mLog.Info("CheckMail command received");
            if (mMailTimer != null)
            {
                mLog.Warn("CheckMail ignored client timer already running");
                return;
            }

            Client.UpdateAccounts(false);
            if (Settings.Default.ShowNotifications)
                Client.CheckMails();
            mLog.Info("CheckMail finished");
        }

        private void Command_ShowWindow(object sender, CommandEventArgs e)
        {
            mLog.Info("ShowWindow command received");
            StartTimer();

            if(e.Command.Args.Length == 1)
            {
                ViewAccount account = null;
                foreach(var i in Client.ViewAccounts)
                {
                    if (i.Id.ToString() != e.Command.Args[0] && i.UserName != e.Command.Args[0])
                        i.IsSelected = false;
                    else
                    {
                        i.IsSelected = true;
                        account = i;
                    }
                }
            }

            if (MailWindow == null)
            {
                MailWindow = new MainWindow();
                MailWindow.Closed += (a, b) => MailWindow = null;
                MailWindow.Show();
            }
            e.Handle = new WindowInteropHelper(MailWindow).Handle;
        }

        private void Command_NewMail(object sender,CommandEventArgs e)
        {
            mLog.Info("Create new mail command recieved");
            StartTimer();
            DraftMessageSource msg = Client.CreateDraft();

            for(int i = 0;i < e.Command.Args.Length;i++)
            {
                EntityType type;
                switch(e.Command.Args[i])
                {
                    case "-alliance":
                        type = EntityType.Alliance;
                        break;
                    case "-corp":
                        type = EntityType.Corporation;
                        break;
                    case "-character":
                        type = EntityType.Character;
                        break;
                    case "-subject":
                        msg.Subject = e.Command.Args[++i];
                        continue;
                    case "-body":
                        msg.Body = e.Command.Args[++i];
                        continue;
                    default:
                        return;
                }
                
                long id;
                if (!long.TryParse(e.Command.Args[++i], out id))
                    msg.AddRecipient(e.Command.Args[i], type);
                else
                {
                    var entity = new EntityInfo()
                    {
                        EntityType = type,
                        EntityID = id
                    };

                    Client.AddLookup(entity);
                    msg.AddRecipients(entity);
                }
            }

            Client.FinishLookups();

            MailWriter.MailWriter writer;

            Client.OpenDraft(msg, out writer);

            writer.Show();

            e.Handle = new WindowInteropHelper(writer).Handle;
        }

        public void SetStyle(SkinStyle style)
        {
            Uri uri = new Uri(style.ToString() + ".xaml", UriKind.Relative);
            ResourceDictionary skin = LoadComponent(uri) as ResourceDictionary;
            if (skin == null)
                return;

            var res = Resources.MergedDictionaries;

            if (res.Count > 0)
                res.Clear();

            res.Add(skin);
        }

        public EveMailClient Client { get; } = new EveMailClient();

        public ServiceConnection ServiceLink => mServiceLink;

        public static EveMailClient CurrentClient
        {
            get
            {
                return ((App)Current).Client;
            }
        }

        public MainWindow MailWindow { get; private set; }
        public static string AppDirectory => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            this.ServiceLink.Dispose();
            Client.Dispose();
            mLog.Info("Application shutdown");
        }
        
    }    
}
