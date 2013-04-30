using System;
using System.IO;

namespace VersionOneTFSServer
{
    public static class Paths
    {

        public static string ConfigurationDirectory
        {
            get{return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "V1TFSServer");}
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