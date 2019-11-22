using Hyperwave.ViewModel;
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

namespace Hyperwave.MailWriter
{
    /// <summary>
    /// Interaction logic for EnterUrlDialog.xaml
    /// </summary>
    public partial class EnterUrlDialog : Window
    {
        public EnterUrlDialog()
        {
            InitializeComponent();
            Url = new URLSource();
        }

        internal URLSource Url
        {
            get { return DataContext as URLSource; }
            set { DataContext = value; }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            cUrl.Focus();
        }

        private void cOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }

    class URLSource : UIObject
    {
        string mUrl;
        string mDescription;

        public string Url
        {
            get { return mUrl; }
            set
            {
                if (mUrl == value)
                    return;
                mUrl = value;
                OnPropertyChanged("Url");
                OnPropertyChanged("IsValid");
            }
        }

        public string Description
        {
            get { return mDescription; }
            set
            {
                if (mDescription == value)
                    return;
                mDescription = value;
                OnPropertyChanged("Description");
                OnPropertyChanged("IsValid");
            }
        }

        public bool IsValid
        {
            get
            {
                return Url != null && !string.IsNullOrWhiteSpace(mDescription) && IsValidUrl();
            }
        }

        private bool IsValidUrl()
        {
            Uri uri;
            if (!Uri.TryCreate(mUrl, UriKind.Absolute, out uri))
                return false;

            return uri.Scheme == "http" || uri.Scheme == "https";
            
        }
    }
}
