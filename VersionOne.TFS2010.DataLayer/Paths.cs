using System;
using System.IO;
using System.Web;

namespace VersionOneTFS2010.DataLayer
{
    public static class Paths
    {

        public static string ConfigurationDirectory
        {
            get
            {
                string directory;

                try
                {
                    directory = string.Concat(HttpRuntime.AppDomainAppPath, "/App_Data/");
                }
                catch(Exception)
                {
                    directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "V1TFSServer");
                }

                return directory;

            }
        }

        public static string ConfigurationPath
        {
            get { return Path.Combine(ConfigurationDirectory, ConfigurationFileName); }
        }

        public static string ConfigurationFileName
        {
            get { return "settings.ini"; }
        }
    }
}