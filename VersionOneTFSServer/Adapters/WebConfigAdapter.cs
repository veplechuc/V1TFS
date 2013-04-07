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
            var rootWebConfig = GetWebConfig(null);

            keyNames.ToList().ForEach(key =>
                {
                    var configuredSetting = rootWebConfig.AppSettings.Settings[key];
                    string valueToAdd = null;
                    if (configuredSetting != null) valueToAdd = configuredSetting.Value;
                    settings.Add(key, valueToAdd);
                });

            return settings;
        }

        private static Configuration GetWebConfig(string path)
        {
            return System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(path);
        }

        /// <summary>
        /// Clears only V1 relevant settings from the web.config.
        /// </summary>
        public static void ClearV1Settings()
        {
            
        }
    }
}