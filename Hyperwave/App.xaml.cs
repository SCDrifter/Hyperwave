﻿using Hyperwave.Config;
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
        EveMailClient mClient = new EveMailClient();
        DispatcherTimer mMailTimer = null;
        string mEvePath = null;
        private bool mBackgroundEnabled;
        private decimal mBackgroundMailCheckInterval;
        private MailCheckIntervalUnit mBackgroundMailCheckUnit;
        private int mBackgroundSettingChecksum;
        int mProcessCount = 0;
        private SkinStyle mColorScheme;

        public const string APPID = "Deadevetech.Hyperwave.1";
#if NDEBUG
        string mShellPath;
#endif

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            
            if(mClient.Commands.ConnectToExistingClient(50))
            {
                if (e.Args.Length == 0)
                    mClient.Commands.SendCommand("ShowWindow");
                else
                    mClient.Commands.SendCommand(new ApplicationCommand(ParseUrlCmdline(e.Args)));

                Shutdown();
                return;
            }

            ShutdownMode = ShutdownMode.OnExplicitShutdown;

#if NDEBUG
            mShellPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu), @"Programs\Hyperwave Test.lnk");
            Installer.CreateShortcut(mShellPath, System.Reflection.Assembly.GetEntryAssembly().Location, APPID);
#endif

            AddProcessCount();

            mBackgroundEnabled = Settings.Default.BackgroundEnabled;
            mBackgroundMailCheckInterval = Settings.Default.BackgroundMailCheckInterval;
            mBackgroundMailCheckUnit = Settings.Default.BackgroundMailCheckUnit;
            mBackgroundSettingChecksum = Settings.Default.BackgroundSettingChecksum;
            mColorScheme = Settings.Default.ColorScheme;
            
            
            Settings.Default.PropertyChanged += Settings_PropertyChanged;

            mClient.Commands.CommandRecieved += Commands_CommandRecieved;
            mClient.Commands.RegisterCommandHandler("ShowWindow", Command_ShowWindow);
            mClient.Commands.RegisterCommandHandler("NewMail", Command_NewMail);
            mClient.Commands.RegisterCommandHandler("CheckMail", Command_CheckMail);
            //mClient.Commands.RegisterCommandHandler("Install", Command_Install);
            //mClient.Commands.RegisterCommandHandler("Uninstall", Command_Uninstall);

            mClient.Commands.RegisterCommandHandler("NoOp", delegate (object s, CommandEventArgs ce)
             {
             });


            mClient.AccountNotification += mClient_AccountNotification;

            mClient.ControllerIdle += mClient_ControllerIdle;
            mClient.ControllerActive += mClient_ControllerActive;
            
            Task discard = mClient.Commands.StartServerLoop();

            SetStyle(Settings.Default.ColorScheme);

            mClient.Init();

            if (e.Args.Length == 0)
                mClient.Commands.SendCommand("ShowWindow");
            else
            {
                mClient.Commands.SendCommand(new ApplicationCommand(ParseUrlCmdline(e.Args)));
            }

            RemoveProcessCount();
        }

        private string[] ParseUrlCmdline(string[] args)
        {
            Uri url;
            if (args.Length != 1 || !Uri.TryCreate(args[0], UriKind.Absolute, out url))
                return args;
            switch (url.Scheme)
            {
                case "eve-mail":
                    return ParseEveMailUrl(url);
                case "hyperwave":
                    return ParseLicenseUrl(url);
                default:
                    return new string[] { "NoOp" };
            }
        }

        private string[] ParseLicenseUrl(Uri url)
        {
            switch (url.Host)
            {
                case "activate":
                    return new string[] { "ActivateLicense", (url.Query ?? "?").Substring(1) };
                default:
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

        private async void Settings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
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
                case "BackgroundSettingChecksum":
                    if(Settings.Default.BackgroundSettingChecksum != mBackgroundSettingChecksum)
                        await SetupBackgroundTask();
                    break;
            }
        }

        async Task SetupBackgroundTask()
        {
            AddProcessCount();

            try
            {
                if (await Installer.SetupTask(System.Reflection.Assembly.GetExecutingAssembly().Location, Settings.Default.BackgroundEnabled, Settings.Default.BackgroundMailCheckInterval))
                {
                    mBackgroundEnabled = Settings.Default.BackgroundEnabled;
                    mBackgroundMailCheckInterval = Settings.Default.BackgroundMailCheckInterval;
                    mBackgroundMailCheckUnit = Settings.Default.BackgroundMailCheckUnit;
                    mBackgroundSettingChecksum = Settings.Default.BackgroundSettingChecksum;
                }
                else
                {
                    MessageBox.Show(GetMsgBoxOwner(), "Unable to setup Background mail checking task, reverting settings to previous good value", "Hyperwave", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                    Settings.Default.BackgroundEnabled = mBackgroundEnabled;
                    Settings.Default.BackgroundMailCheckInterval = mBackgroundMailCheckInterval;
                    Settings.Default.BackgroundMailCheckUnit = mBackgroundMailCheckUnit;
                    Settings.Default.BackgroundSettingChecksum = mBackgroundSettingChecksum;
                }
            }
            finally
            {
                RemoveProcessCount();
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
            if(--mProcessCount <= 0)
                Shutdown();

        }

        internal void AddProcessCount()
        {
            mProcessCount++;
        }

        

        private void mMailTimer_Tick(object sender, EventArgs e)
        {
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
                if (args.NewMails.Length > 2)
                {
                    Notification notify = new Notification()
                    {
                        ImageUrl = args.ForAccount.ImageUrl.ImageUrlToUri(UriKind.Absolute),
                        SoundUrl = "@://Sound/NewMails.wav".SoundUrlToUri(UriKind.Absolute),
                        Title = string.Format("{0} new Eve-Mails for character {1}",args.NewMails.Length,args.ForAccount.UserName)                        
                    };

                    notify.NotificationActivated += (o,e) => mClient.Commands.SendCommand("ShowWindow", args.ForAccount.Id.ToString());
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

                        mClient.SaveMailMetaData(mail);
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
            MessageBox.Show(string.Format("Invalid commandline argument {0}", e.Command.Command));
        }
                        

        private void Command_CheckMail(object sender, CommandEventArgs e)
        {
            if (mMailTimer != null)
                return;

            mClient.UpdateAccounts(false);
            if (Settings.Default.ShowNotifications)
                mClient.CheckMails();
        }

        private void Command_ShowWindow(object sender, CommandEventArgs e)
        {
            StartTimer();

            if(e.Command.Args.Length == 1)
            {
                ViewAccount account = null;
                foreach(var i in mClient.ViewAccounts)
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
            StartTimer();
            DraftMessageSource msg = mClient.CreateDraft();

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

        public EveMailClient Client
        {
            get { return mClient; }
        }

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
            mClient.Dispose();
            ImageCache.Clear();
#if NDEBUG
            if(mShellPath != null)
                File.Delete(mShellPath);
#endif
        }
        
    }

    
}
