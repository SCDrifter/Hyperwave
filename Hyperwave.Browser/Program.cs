using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;

namespace Hyperwave.Browser
{
    static class Program
    {
        static Dictionary<string, string> mParams = new Dictionary<string, string>();
        static Regex mRegEx_ArgValue = new Regex(@"^--(?<Name>[^=\s]+)=(?<Value>.*)$");
        static Regex mRegEx_Arg = new Regex(@"^--(?<Name>(?<Value>[^=\s]+))$");
        static void ParseArgs(string[] args)
        {
            foreach(var arg in args)
            {
                Match match;
                if (!mRegEx_ArgValue.Match(arg, out match) && !mRegEx_Arg.Match(arg, out match))
                    continue;
                mParams.Replace(match.Groups["Name"].Value, match.Groups["Value"].Value);
            }
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            ParseArgs(args);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            CefSharpSettings.SubprocessExitIfParentProcessClosed = true;

            var settings = new CefSettings()
            {
                CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CefSharp\\Cache")
            };

            Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);


            Application.Run(new MainWindow());
        }

        public static string GetParamString(string name,string defvalue = "")
        {
            string ret;
            if (!mParams.TryGetValue(name, out ret))
                ret = defvalue;

            return ret;
        }

        public static bool GetParamBool(string name,bool defvalue = false)
        {
            string ret = GetParamString(name, defvalue ? name : "");
            return ret == name;
        }

        public static IntPtr GetParamHwnd(string name,IntPtr defvalue = default(IntPtr))
        {
            string ret = GetParamString(name);
            long convert;
            if (!long.TryParse(name, out convert))
                return defvalue;
            return (IntPtr)convert;
        }
    }
}
