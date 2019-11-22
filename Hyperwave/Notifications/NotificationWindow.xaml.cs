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
using System.Windows.Threading;

namespace Hyperwave.Notifications
{
    /// <summary>
    /// Interaction logic for NotificationWindow.xaml
    /// </summary>
    public partial class NotificationWindow : Window
    {
        class NotificationData
        {
            public Notification Notification;
            public NotificationCallback Callback;
        }

        Queue<NotificationData> mQueue = new Queue<NotificationData>();

        NotificationData mCurrent = null;

        DispatcherTimer mTimer = new DispatcherTimer();
        MediaPlayer mSounds = new MediaPlayer();
        public NotificationWindow()
        {
            InitializeComponent();
            mTimer.Tick += mTimer_Tick;
            mTimer.Interval = TimeSpan.FromSeconds(10);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadNotification();
        }

        private async void mTimer_Tick(object sender, EventArgs e)
        {
            await LoadNextNotification(false);
        }


        public void AddToQueue(Notification notification, NotificationCallback callback)
        {
            mQueue.Enqueue(new NotificationData()
            {
                Notification = notification,
                Callback = callback
            });

        }

        bool LoadNotification()
        {
            if(mQueue.Count == 0)
            {
                Close();
                return false;
            }
            mCurrent = mQueue.Dequeue();
            DataContext = mCurrent.Notification;
            if (mCurrent.Notification.SoundUrl != null)
            {
                mSounds.Open(mCurrent.Notification.SoundUrl);
                mSounds.Play();
            }
            mTimer.Start();
            return true;
        }
        private async Task LoadNextNotification(bool activated)
        {
            mSounds.Stop();
            mSounds.Close();
            if (mCurrent != null)
                mCurrent.Callback(activated);

            mTimer.Stop();
            Visibility = Visibility.Collapsed;
            
            if(mQueue.Count > 0)
                await Task.Delay(TimeSpan.FromSeconds(1));

            if(LoadNotification())
                Visibility = Visibility.Visible;
        }

        private async void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            await LoadNextNotification(false);
        }

        private void Window_LayoutUpdated(object sender, EventArgs e)
        {
            Left = SystemParameters.WorkArea.Right - ActualWidth;
            Top = SystemParameters.WorkArea.Bottom - ActualHeight;
        }

        private async void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            await LoadNextNotification(true);
        }
    }

    public delegate void NotificationCallback(bool activated);
}
