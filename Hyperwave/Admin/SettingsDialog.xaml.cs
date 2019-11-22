using Hyperwave.Admin.DataModel;
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

namespace Hyperwave.Admin
{
    /// <summary>
    /// Interaction logic for SettingsDialog.xaml
    /// </summary>
    public partial class SettingsDialog : Window
    {
        public SettingsDialog()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Config.Dispose();
        }

        Configuration Config
        {
            get
            {
                return (Configuration)cRoot.DataContext;
            }
        }

        private void cOk_Click(object sender, RoutedEventArgs e)
        {
            Config.Apply();
            Close();
        }

        private void cCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void cApply_Click(object sender, RoutedEventArgs e)
        {
            Config.Apply();
        }
    }
}
