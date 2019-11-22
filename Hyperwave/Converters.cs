
using Hyperwave.ViewModel;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Hyperwave
{
    public class BoolNegationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool val = (bool)value;
            return !val;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool val = (bool)value;
            return !val;
        }
    }

    public class ColorToBrushConverter : DependencyObject, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color color = (Color)value;
            return new SolidColorBrush(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as SolidColorBrush).Color;
        }
    }

    public class ToIntConverter : DependencyObject, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }

    public class BoolMapConverter : IValueConverter
    {
        object mTrueItem, mFalseItem;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value) ? mTrueItem : mFalseItem;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }

        public object TrueItem
        {
            get
            {
                return mTrueItem;
            }
            set
            {
                mTrueItem = value;
            }
        }

        public object FalseItem
        {
            get
            {
                return mFalseItem;
            }
            set
            {
                mFalseItem = value;
            }
        }
    }

    public class BoolToImageConverter : DependencyObject, IValueConverter
    {
        BitmapImage mTrueImage,mFalseImage;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value) ? mTrueImage : mFalseImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }

        public string TrueImage
        {
            get
            {
                return mTrueImage != null ? mTrueImage.BaseUri.ToString() : null;
            }
            set
            {
                Uri uri = new Uri(value, UriKind.Relative);
                mTrueImage = new BitmapImage(uri);
            }
        }
        public string FalseImage
        {
            get
            {
                return mFalseImage != null ? mFalseImage.BaseUri.ToString() : null;
            }
            set
            {
                Uri uri = new Uri(value, UriKind.Relative);
                mFalseImage = new BitmapImage(uri);
            }
        }
    }
    public class BoolToFontWeightConverter : DependencyObject, IValueConverter
    {
        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
            return ((bool)value) ? FontWeights.Bold : FontWeights.Normal;
        }

        public object ConvertBack(object value, Type targetType,
                                  object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }

    public class BoolToVisiblityConverter : DependencyObject, IValueConverter
    {
        Visibility mHidden = Visibility.Hidden;
        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
            return ((bool)value != IsInverted) ? Visibility.Visible : mHidden;
        }

        public object ConvertBack(object value, Type targetType,
                                  object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }

        public bool IsInverted { get; set; }

        public bool Collapse
        {
            get
            {
                return mHidden == Visibility.Collapsed;
            }
            set
            {
                mHidden = value ? Visibility.Collapsed : Visibility.Hidden;
            }
        }
    }

    public class StringToImageConverter : DependencyObject, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string text = value as string;

            if (text == null)
                return null;

            Uri uri = text.ImageUrlToUri(UriType);
            
            BitmapImage img = new BitmapImage(uri);
            return img;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((BitmapImage)value).BaseUri.ToString();
        }

        public UriKind UriType { get; set; }
    }



    public class UriToImageConverter : DependencyObject, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Uri uri = value as Uri;

            if (uri == null)
                return null;

            
            BitmapImage img = new BitmapImage(uri);
            return img;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((BitmapImage)value).BaseUri;
        }
    }

    public class LabelTypeToImageConverter : DependencyObject, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Uri uri;

            switch((LabelType)value)
            {
                case LabelType.Inbox:
                    uri = new Uri("/Images/Icons/16/Inbox.png",UriKind.Relative);
                    break;
                case LabelType.Outbox:
                    uri = new Uri("/Images/Icons/16/Outbox.png", UriKind.Relative);
                    break;
                case LabelType.Drafts:
                    uri = new Uri("/Images/Icons/16/Drafts.png", UriKind.Relative);
                    break;
                case LabelType.CorpMail:
                    uri = new Uri("/Images/Icons/16/Corp.png", UriKind.Relative);
                    break;
                case LabelType.AllianceMail:
                    uri = new Uri("/Images/Icons/16/Alliance.png", UriKind.Relative);
                    break;
                case LabelType.MailingList:
                    uri = new Uri("/Images/Icons/16/MailinglistFilter.png", UriKind.Relative);
                    break;
                case LabelType.Label:
                default:
                    uri = new Uri("/Images/Icons/16/Filter.png", UriKind.Relative);
                    break;
            }

            BitmapImage img = new BitmapImage(uri);
            return img;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }

    public class IntToReadCountConverter : DependencyObject, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int val = (int)value;
            if (val > 99)
                return "99+";
            else
                return val.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }

    public class AccountStateToBrushConverter : DependencyObject, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            switch ((AccountState)value)
            {
                case AccountState.Offline:
                    return OfflineColor;
                case AccountState.Online:
                    return OnlineColor;
                case AccountState.Failed:
                    return FailedColor;
                default:
                    return Brushes.Purple;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }

        public Brush OfflineColor { get; set; }
        public Brush OnlineColor { get; set; }
        public Brush FailedColor { get; set; }
    }

    public class AccountStateToVisibleConverter : DependencyObject, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((AccountState)value)
            {
                default:
                case AccountState.Offline:
                case AccountState.Online:
                    return Visibility.Collapsed;
                case AccountState.Failed:
                    return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }

    public class ObjectFlagConverter : DependencyObject,IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value) ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }

        public object TrueValue { get; set; }
        public object FalseValue { get; set; }
    }

    public class DateTimeFormatter : DependencyObject, IValueConverter
    {
        string GetDateFormatString()
        {
            List<string> ret = new List<string>(2);
            if (Display.HasFlag(DateDislayFormat.Date))
                ret.Add(Properties.Settings.Default.DateFormat);

            if (Display.HasFlag(DateDislayFormat.Time))
                ret.Add(Properties.Settings.Default.TimeFormat);

            return string.Join(" ", ret);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((DateTime)value).ToString(GetDateFormatString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DateTime.ParseExact(value.ToString(), GetDateFormatString(), CultureInfo.CurrentCulture);
        }

        public DateDislayFormat Display
        {
            get; set;
        }

    }

    [Flags]
    public enum DateDislayFormat
    {
        None = 0,
        Date = 1 << 0,
        Time = 1 << 1,
        Both = Date | Time
    }


    public class ColorLevelConverter : DependencyObject, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double level = (double)value;

            foreach(var i in Levels)
            {
                if (level < i.Level)
                    return i.ColorBrush;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }

        public List<LevelColor> Levels { get; set; } = new List<LevelColor>();
    }

    public class LevelColor
    {
        public double Level { get; set; }
        public Brush ColorBrush { get; set; }
    }

    public class BoolExpressionMultiConverter : DependencyObject, IMultiValueConverter
    {
        public BoolExpressionMultiConverter()
        {
        }
        public virtual object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string exp = parameter != null ? parameter.ToString() : Expression;

            return BoolEvaluator.Evaluate(exp, values, culture);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public string Expression { get; set; }
    }

    public class BoolExpressionToVisibilityConverter : BoolExpressionMultiConverter
    {
        private Visibility mHidden = Visibility.Hidden;

        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)base.Convert(values, targetType, parameter, culture)) ? Visibility.Visible : mHidden;
        }

        public bool Collapse
        {
            get
            {
                return mHidden == Visibility.Collapsed;
            }
            set
            {
                mHidden = value ? Visibility.Collapsed : Visibility.Hidden;
            }
        }
    }

   

    
}
