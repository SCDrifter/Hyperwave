using Hyperwave.UserCache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Hyperwave
{
    static class Program
    {
        [STAThread]
        public static void NotMain()
        {            
            App app = new App();
            app.InitializeComponent();
            app.Run();
        }
    }
}
