using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Hyperwave.Common
{
    /// <summary>
    /// Interaction logic for NumericEntry.xaml
    /// </summary>
    public partial class NumericEntry : UserControl
    {
        public NumericEntry()
        {
            InitializeComponent();
        }

        [Bindable(true)]
        public decimal Value
        {
            get { return (decimal)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
         DependencyProperty.Register("Value", typeof(decimal), typeof(NumericEntry), new UIPropertyMetadata(defaultValue: 0.0M,coerceValueCallback:Value_Coerce,propertyChangedCallback:null));

        [Bindable(true)]
        public decimal MinimumValue
        {
            get { return (decimal)GetValue(MinimumValueProperty); }
            set { SetValue(MinimumValueProperty, value); }
        }

        public static readonly DependencyProperty MinimumValueProperty =
         DependencyProperty.Register("MinimumValue", typeof(decimal), typeof(NumericEntry), new UIPropertyMetadata(defaultValue: 0.0M,propertyChangedCallback:MinMaxValue_Changed));

        [Bindable(true)]
        public decimal MaximumValue
        {
            get { return (decimal)GetValue(MaximumValueProperty); }
            set { SetValue(MaximumValueProperty, value); }
        }

        public static readonly DependencyProperty MaximumValueProperty =
         DependencyProperty.Register("MaximumValue", typeof(decimal), typeof(NumericEntry), new UIPropertyMetadata(defaultValue: 100.0M, propertyChangedCallback: MinMaxValue_Changed));

        [Bindable(true)]
        public decimal Step
        {
            get { return (decimal)GetValue(StepProperty); }
            set { SetValue(StepProperty, value); }
        }

        public static readonly DependencyProperty StepProperty =
         DependencyProperty.Register("Step", typeof(decimal), typeof(NumericEntry), new UIPropertyMetadata(1.0M));

        [Bindable(true)]
        public string NumberFormat
        {
            get { return (string)GetValue(NumberFormatProperty); }
            set { SetValue(NumberFormatProperty, value); }
        }

        public static readonly DependencyProperty NumberFormatProperty =
         DependencyProperty.Register("NumberFormat", typeof(string), typeof(NumericEntry), new UIPropertyMetadata("0"));

        private static object Value_Coerce(DependencyObject dobject, object value)
        {
            NumericEntry entry = (NumericEntry)dobject;
            decimal current = (decimal)value;
            if (current < entry.MinimumValue) current = entry.MinimumValue;
            else if (current > entry.MaximumValue) current = entry.MaximumValue;
            return current;
        }

        static void MinMaxValue_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.CoerceValue(ValueProperty);
        }

        private void cStepUp_Click(object sender, RoutedEventArgs e)
        {
            Value += Step;
        }

        private void cStepDown_Click(object sender, RoutedEventArgs e)
        {
            Value -= Step;
        }

        private void cText_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        static Regex mRegEx_Numeric = new Regex(@"^[0-9\-\.]$");
        private bool IsTextAllowed(string text)
        {
            return mRegEx_Numeric.IsMatch(text);
        }

        private void cText_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!IsTextAllowed(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }
    }

    class NumericEntryConverter : IMultiValueConverter
    {        
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2)
                return "";

            decimal val = (values[0] is decimal) ? (decimal)values[0] : 0M;
            string format = values[1].ToString();
            return val.ToString(format ?? "0");
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            decimal val;

            decimal.TryParse(value.ToString(), out val);

            return new object[]
            {
                val,
                Binding.DoNothing
            };
        }
        
    }
}
