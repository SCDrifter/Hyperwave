using System.Configuration;

namespace Hyperwave.Config
{
    public static class ESIConfiguration
    {
        public static string DataSource
        {
            get
            {
                return ConfigurationManager.AppSettings["ESI.DataSource"];
            }
        }

        public static string UserAgent
        {
            get
            {
                return ConfigurationManager.AppSettings["ESI.UserAgent"];
            }
        }
    }
}
