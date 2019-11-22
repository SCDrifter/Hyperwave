using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hyperwave.Common
{
    /// <summary>
    /// Interaction logic for SortOrderTicker.xaml
    /// </summary>
    public partial class SortOrderTicker : UserControl
    {
        public SortOrderTicker()
        {
            InitializeComponent();
        }

        [Bindable(true)]
        public bool AscendingOrder
        {
            get { return (bool)GetValue(AscendingOrderProperty); }
            set { SetValue(AscendingOrderProperty, value); }
        }

        public static readonly DependencyProperty AscendingOrderProperty =
         DependencyProperty.Register("AscendingOrder", typeof(bool), typeof(SortOrderTicker), new UIPropertyMetadata(false));


    }
}
