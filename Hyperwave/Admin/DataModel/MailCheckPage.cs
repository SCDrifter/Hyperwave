using Hyperwave.Properties;
using Hyperwave.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperwave.Admin.DataModel
{
    class MailCheckPage : ConfigPage
    {
        bool mShowNotifications;
        decimal mMailCheckInterval;
        MailCheckIntervalUnit mMailCheckUnit;
        bool mSupressNotificationsClient;
        bool mBackgroundEnabled;
        decimal mBackgroundMailCheckInterval;
        MailCheckIntervalUnit mBackgroundMailCheckUnit;

        public MailCheckPage(ConfigPage parent)
            :base(parent)
        {
            mShowNotifications = Settings.Default.ShowNotifications;
            mMailCheckUnit = Settings.Default.MailCheckUnit;
            mMailCheckInterval = ConvertUnits(Settings.Default.MailCheckInterval,MailCheckIntervalUnit.Seconds,mMailCheckUnit);
            mSupressNotificationsClient = Settings.Default.SupressNotificationsClient;
            mBackgroundEnabled = Settings.Default.BackgroundEnabled;
            mBackgroundMailCheckUnit = Settings.Default.BackgroundMailCheckUnit;
            mBackgroundMailCheckInterval = ConvertUnits(Settings.Default.BackgroundMailCheckInterval,MailCheckIntervalUnit.Seconds,mBackgroundMailCheckUnit);
            
        }

        public override string Name
        {
            get
            {
                return "Mail Update";
            }
        }

        protected override Uri ImageUri
        {
            get
            {
                return new Uri("pack://application:,,,/Images/Icons/32/SyncMails.png");
            }
        }

        public override void Apply()
        {
            Settings.Default.ShowNotifications = mShowNotifications;
            Settings.Default.MailCheckUnit = mMailCheckUnit;
            Settings.Default.MailCheckInterval = ConvertUnits(mMailCheckInterval,mMailCheckUnit,MailCheckIntervalUnit.Seconds);
            Settings.Default.SupressNotificationsClient = mSupressNotificationsClient;
            Settings.Default.BackgroundEnabled = mBackgroundEnabled;
            Settings.Default.BackgroundMailCheckUnit = mBackgroundMailCheckUnit;
            Settings.Default.BackgroundMailCheckInterval = ConvertUnits(mBackgroundMailCheckInterval,mBackgroundMailCheckUnit,MailCheckIntervalUnit.Seconds);
            //Settings.Default.BackgroundSettingUpdate = CalculateCanarySetting();
        }

        decimal ConvertUnits(decimal value,MailCheckIntervalUnit from,MailCheckIntervalUnit to)
        {
            if (from == to)
                return value;

            switch(from)
            {
                case MailCheckIntervalUnit.Seconds:
                    break;
                case MailCheckIntervalUnit.Minutes:
                    value = value * 60;
                    break;
                case MailCheckIntervalUnit.Hours:
                    value = value * (60 * 60);
                    break;
            }

            switch (to)
            {
                case MailCheckIntervalUnit.Seconds:
                    break;
                case MailCheckIntervalUnit.Minutes:
                    value = value / 60;
                    break;
                case MailCheckIntervalUnit.Hours:
                    value = value / (60 * 60);
                    break;
            }

            return Math.Round(value);
        }

        public bool ShowNotifications
        {
            get
            {
                return mShowNotifications;
            }
            set
            {
                if (value == mShowNotifications)
                    return;
                mShowNotifications = value;
                if (!value)
                    BackgroundEnabled = false;

                OnPropertyChanged("ShowNotifications");
            }
        }

        public MailCheckIntervalUnit MailCheckUnit
        {
            get
            {
                return mMailCheckUnit;
            }
            set
            {
                if (value == mMailCheckUnit)
                    return;
                MailCheckInterval = ConvertUnits(MailCheckInterval, mMailCheckUnit, value);

                mMailCheckUnit = value;
                OnPropertyChanged("MailCheckUnit");
                OnPropertyChanged("MailCheckMin");
            }
        }

        public decimal MailCheckMin
        {
            get
            {
                return ConvertUnits(30, MailCheckIntervalUnit.Seconds, MailCheckUnit);
            }
        }

        public decimal BackgroundMailCheckMin
        {
            get
            {
                return ConvertUnits(30, MailCheckIntervalUnit.Seconds, BackgroundMailCheckUnit);
            }
        }

        public decimal MailCheckInterval
        {
            get
            {
                return mMailCheckInterval;
            }
            set
            {
                if (value == mMailCheckInterval)
                    return;
                mMailCheckInterval = value;
                OnPropertyChanged("MailCheckInterval");
            }
        }

        public bool SupressNotificationsClient
        {
            get
            {
                return mSupressNotificationsClient;
            }
            set
            {
                if (value == mSupressNotificationsClient)
                    return;
                mSupressNotificationsClient = value;
                OnPropertyChanged("SupressNotificationsClient");
            }
        }

        private int CalculateCanarySetting()
        {
            StringBuilder compiler = new StringBuilder();
            compiler.Append(mBackgroundEnabled.ToString());
            if (mBackgroundEnabled)
            {
                compiler.Append(mBackgroundMailCheckUnit.ToString());
                compiler.Append(mBackgroundMailCheckInterval.ToString());
            }
            return compiler.ToString().GetHashCode();
        }

        public bool BackgroundEnabled
        {
            get
            {
                return mBackgroundEnabled;
            }
            set
            {
                if (value == mBackgroundEnabled)
                    return;
                mBackgroundEnabled = value;
                OnPropertyChanged("BackgroundEnabled");
            }
        }

        public MailCheckIntervalUnit BackgroundMailCheckUnit
        {
            get
            {
                return mBackgroundMailCheckUnit;
            }
            set
            {
                if (value == mBackgroundMailCheckUnit)
                    return;
                BackgroundMailCheckInterval = ConvertUnits(BackgroundMailCheckInterval, mBackgroundMailCheckUnit, value);

                mBackgroundMailCheckUnit = value;
                OnPropertyChanged("BackgroundMailCheckUnit");
                OnPropertyChanged("BackgroundMailCheckMin");
            }
        }

        public decimal BackgroundMailCheckInterval
        {
            get
            {
                return mBackgroundMailCheckInterval;
            }
            set
            {
                if (value == mBackgroundMailCheckInterval)
                    return;
                mBackgroundMailCheckInterval = value;
                OnPropertyChanged("BackgroundMailCheckInterval");
            }
        }

        public override bool HasChanged
        {
            get
            {
                return Settings.Default.ShowNotifications != mShowNotifications
                    || ConvertUnits(Settings.Default.MailCheckInterval,MailCheckIntervalUnit.Seconds,MailCheckUnit) != mMailCheckInterval
                    || Settings.Default.MailCheckUnit != mMailCheckUnit
                    || Settings.Default.SupressNotificationsClient != mSupressNotificationsClient
                    || Settings.Default.BackgroundEnabled != mBackgroundEnabled
                    || ConvertUnits(Settings.Default.BackgroundMailCheckInterval, MailCheckIntervalUnit.Seconds, BackgroundMailCheckUnit) != mBackgroundMailCheckInterval
                    || Settings.Default.BackgroundMailCheckUnit != mBackgroundMailCheckUnit;
            }
        }
    }
}
