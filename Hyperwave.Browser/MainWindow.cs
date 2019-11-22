using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hyperwave.Browser
{
    public partial class MainWindow : Form
    {
        private ChromiumWebBrowser cBrowser;

        public MainWindow()
        {
            InitializeComponent();
            Text = "Chromium Browser";
            cBrowser = new ChromiumWebBrowser(Program.GetParamString("url", "about:blank"));
            cBrowser.Dock = DockStyle.Fill;
            cBrowser.AddressChanged += cBrowser_AddressChanged;
            cBrowser.TitleChanged += cBrowser_TitleChanged;
            cUrl.Text = cBrowser.Address;
            cBrowserContainer.Controls.Add(cBrowser);
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            cWaiter.RunWorkerAsync();
        }

        private void cBrowser_TitleChanged(object sender, CefSharp.TitleChangedEventArgs e)
        { 
            this.InvokeIfRequired(() =>
            {
                Text = e.Title;
            });
        }

        private void cBrowser_AddressChanged(object sender, CefSharp.AddressChangedEventArgs e)
        {
            if (e.Browser != cBrowser.GetBrowser())
                return;

            this.InvokeIfRequired(() =>
            {
                cUrl.Text = e.Address;
            });
        }

        private void cWaiter_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result != null)
                Close();
        }

        private void cWaiter_DoWork(object sender, DoWorkEventArgs e)
        {
            EventWaitHandle wait;
            if (!EventWaitHandle.TryOpenExisting("Zukatech.Hyperwave.Events.Browser", out wait))
                return;
            using (wait)
            {
                e.Result = this;
                wait.WaitOne();
            }
        }
    }
}
