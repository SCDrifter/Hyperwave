using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Hyperwave.Config
{
    public static class SSOConfiguration
    {
        public static Uri LoginPortalUrl
        {
            get
            {
#if TEST_LOCALHOST
                return new Uri("http://localhost:5000",UriKind.Absolute);
#else
                return new Uri(ConfigurationManager.AppSettings["SSO.LoginPortal"],UriKind.Absolute);
#endif
            }
        }

        public static string ClientId
        {
            get
            {
#if TEST_LOCALHOST
                return "404c10fe4cae4a3c9030c335f4ec9c6f";
#else
                return ConfigurationManager.AppSettings["SSO.ClientId"];
#endif
            }
        }
    }
}
