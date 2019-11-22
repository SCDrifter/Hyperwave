using Hyperwave.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;
using Hyperwave.Properties;
using System.Windows.Threading;

namespace Hyperwave.Admin.DataModel
{
    class MailReadPage : ConfigPage
    {

        EmailReadAction mMailReadAction;
        string mDateFormat;
        string mTimeFormat;

        DispatcherTimer mTimer = new DispatcherTimer()
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        
        public MailReadPage(ConfigPage parent)
            :base(parent)
        {
            mMailReadAction = Settings.Default.MailReadAction;
            mDateFormat = Settings.Default.DateFormat;
            mTimeFormat = Settings.Default.TimeFormat;
            mTimer.Tick += delegate (object sender, EventArgs e)
            {
                OnPropertyChanged("DateTimeSample");
            };
        }

        public override string Name
        {
            get
            {
                return "Mail Display";
            }
        }

        protected override Uri ImageUri
        {
            get
            {
                return new Uri("pack://application:,,,/Images/Icons/32/MarkRead.png");
            }
        }

        public override void Apply()
        {
            Settings.Default.MailReadAction = mMailReadAction;
            Settings.Default.DateFormat = mDateFormat;
            Settings.Default.TimeFormat = mTimeFormat;
        }

        protected override void OnSelected()
        {
            base.OnSelected();
            mTimer.Start();
            OnPropertyChanged("DateTimeSample");
        }

        protected override void OnUnselected()
        {
            base.OnUnselected();
            mTimer.Stop();
        }

        protected override void OnDisposed()
        {
            base.OnDisposed();
            mTimer.Stop();
        }

        public string DateTimeSample
        {
            get
            {
                string sdate, stime;
                DateTime dt = DateTime.Now;
                try
                {
                    sdate = dt.ToString(DateFormat);
                }
                catch(FormatException)
                {
                    return "(Invalid Date Format)";
                }

                try
                {
                    stime = dt.ToString(TimeFormat);
                }
                catch (FormatException)
                {
                    return "(Invalid Time Format)";
                }

                return string.Format("{0} {1}", sdate, stime);
            }
        }

        public EmailReadAction[] MailReadValues
        {
            get
            {
                return Enum.GetValues(typeof(EmailReadAction)) as EmailReadAction[];
            }
        }

        public EmailReadAction MailReadAction
        {
            get
            {
                return mMailReadAction;
            }
            set
            {
                if (value == mMailReadAction)
                    return;
                mMailReadAction = value;
                OnPropertyChanged("MailReadAction");
            }
        }

        public string DateFormat
        {
            get
            {
                return mDateFormat;
            }
            set
            {
                if (value == mDateFormat)
                    return;
                mDateFormat = value;
                OnPropertyChanged("DateFormat");
                OnPropertyChanged("DateTimeSample");
            }
        }

        public string TimeFormat
        {
            get
            {
                return mTimeFormat;
            }
            set
            {
                if (value == mTimeFormat)
                    return;
                mTimeFormat = value;
                OnPropertyChanged("TimeFormat");
                OnPropertyChanged("DateTimeSample");
            }
        }

        public override bool HasChanged
        {
            get
            {
                return Settings.Default.MailReadAction != mMailReadAction
                    || Settings.Default.DateFormat != mDateFormat
                    || Settings.Default.TimeFormat != mTimeFormat;
            }
        }
    }

    public class EmailReadActionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch((EmailReadAction)value)
            {
                case EmailReadAction.Manually:
                    return value.ToString();
                case EmailReadAction.AfterMessageLeave:
                    return "After Leaving Message";
                case EmailReadAction.BeforeMessageLoad:
                    return "Before Loading Message";
                default:
                    return "??";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
