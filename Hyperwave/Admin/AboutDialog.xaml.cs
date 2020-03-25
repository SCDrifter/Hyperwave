using Hyperwave.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class AboutDialog : Window
    {
        class AboutModel : UIObject
        {
            int mTabIndex = 0;
            public AboutModel()
            {
                AppInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
            }
            public FileVersionInfo AppInfo { get; private set; }
            public int TabIndex
            {
                get { return mTabIndex; }
                set
                {
                    if (mTabIndex == value)
                        return;
                    mTabIndex = value;
                    OnPropertyChanged("TabIndex");
                }
            }
        }
        public AboutDialog(int tab_index = 0)
        {
            InitializeComponent();
            DataContext = new AboutModel()
            {
                TabIndex = tab_index
            };
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var link = e.Source as Hyperlink;
            ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = link.NavigateUri.ToString(),
                UseShellExecute = true
            };

            Process.Start(psi);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var content = new TextRange(cLicense.ContentStart, cLicense.ContentEnd);
            SaveFileDialog dlg = new SaveFileDialog()
            {
                Filter="RTF File|*.rtf"
            };

            if (!(dlg.ShowDialog(this) ?? false))
                return;
            using (var stream = new FileStream(dlg.FileName, FileMode.Create, FileAccess.Write))
            {
                content.Save(stream, DataFormats.Rtf);
            }
        }
    }
}
