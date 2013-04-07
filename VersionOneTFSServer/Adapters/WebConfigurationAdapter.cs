using System.Collections.Generic;
using System.Linq;
using System.Configuration;

namespace VersionOneTFSServer.Adapters
{
    public static class WebConfigurationAdapter
    {
        /// <summary>
        /// Retrieves settings from the appSettings section of the root web.config file.
        /// </summary>
        /// <param name="keyNames"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetAppSettings(params string[] keyNames)
        {
            if (keyNames.Length == 0) return null;

            var settings = new Dictionary<string, string>();
            var rootWebConfig = GetRootWebConfig();

            keyNames.ToList().ForEach(key =>
                {
                    var configuredSetting = rootWebConfig.AppSettings.Settings[key];
                    string valueToAdd = null;
                    if (configuredSetting != null) valueToAdd = configuredSetting.Value;
                    settings.Add(key, valueToAdd);
                });

            return settings;
        }

        /// <summary>
        /// Saves a set of keyvalue pairs to the appSettings element of a web.config.
        /// </summary>
        /// <param name="keyValuePairs"></param>
        public static void SaveAppSettings(Dictionary<string,string> keyValuePairs)
        {

            var rootWebConfig = GetRootWebConfig();

            foreach (var keyValuePair in keyValuePairs)
            {
                if (rootWebConfig.AppSettings.Settings[keyValuePair.Key] == null)
                {
                    rootWebConfig.AppSettings.Settings.Add(new KeyValueConfigurationElement(keyValuePair.Key, keyValuePair.Value));
                }
                else
                {
                    rootWebConfig.AppSettings.Settings[keyValuePair.Key].Value = keyValuePair.Value;
                }
            }

            rootWebConfig.Save(ConfigurationSaveMode.Modified);
        }

        private static Configuration GetRootWebConfig()
        {
            return System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(null);
        }

        /// <summary>
        /// Clears only V1 relevant settings from the web.config.
        /// </summary>
        public static void ClearV1Settings()
        {
            
        }
    }
}