using Hyperwave.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperwave.Notifications
{
    public class Notification : UIObject
    {
        string mTitle = null;
        string mLine1 = null;
        string mLine2 = null;
        private Uri mImageUri;
        private Uri mSoundUri;

        public object ContextData { get; set; }

        static NotificationWindow mWindow = null;

        
        public string Title
        {
            get
            {
                return mTitle;
            }
            set
            {
                if (value == mTitle)
                    return;
                mTitle = value;
                OnPropertyChanged("Title");
                OnPropertyChanged("HasTitle");
            }
        }

        public string Line1
        {
            get
            {
                return mLine1;
            }
            set
            {
                if (value == mLine1)
                    return;
                mLine1 = value;
                OnPropertyChanged("Line1");
                OnPropertyChanged("HasLine1");
            }
        }
        public string Line2
        {
            get
            {
                return mLine2;
            }
            set
            {
                if (value == mLine2)
                    return;
                mLine2 = value;
                OnPropertyChanged("Line2");
                OnPropertyChanged("HasLine2");
            }
        }
        public bool HasTitle => string.IsNullOrWhiteSpace(mTitle);
        public bool HasLine1 => string.IsNullOrWhiteSpace(mLine1);
        public bool HasLine2 => string.IsNullOrWhiteSpace(mLine2);

        public Uri ImageUrl
        {
            get
            {
                return mImageUri;
            }
            set
            {
                if (value == mImageUri)
                    return;
                mImageUri = value;
                OnPropertyChanged("ImageUrl");
                OnPropertyChanged("HasImageUrl");
            }
        }
        public bool HasImageUrl => mImageUri != null;

        public Uri SoundUrl
        {
            get
            {
                return mSoundUri;
            }
            set
            {
                if (value == mSoundUri)
                    return;
                mSoundUri = value;
                OnPropertyChanged("SoundUrl");
            }
        }

        public void Show()
        {
            if(mWindow != null)
                mWindow.AddToQueue(this, NotificationFinished);
            else
            {
                ((App)App.Current).AddProcessCount();
                mWindow = new NotificationWindow();
                mWindow.Closed += mWindow_Closed;
                mWindow.AddToQueue(this, NotificationFinished);
                mWindow.Show();
            }
            
        }

        public EventHandler NotificationDismissed;
        
        public EventHandler NotificationActivated;

        void NotificationFinished(bool activated)
        {
            if (activated)
                NotificationActivated?.Invoke(this, new EventArgs());
            else
                NotificationDismissed?.Invoke(this, new EventArgs());
        }

        private static void mWindow_Closed(object sender, EventArgs e)
        {
            mWindow = null;
            ((App)App.Current).RemoveProcessCount();
        }
    }
}
