using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using VersionOneTFSServer.Interfaces;

namespace VersionOneTFSServer.Providers
{
    public class WebConfigProvider : IWebConfigProvider
    {
        /// <summary>
        /// Retrieves settings from the appSettings section of the root web.config file.
        /// </summary>
        /// <param name="keyNames"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetAppSettings(params string[] keyNames)
        {
            if (keyNames.Length == 0) return null;

            var settings = new Dictionary<string, string>();
            var rootWebConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(null);
            //if (rootWebConfig.AppSettings.Settings.Count == 0) return null;

            keyNames.ToList().ForEach(key =>
                {
                    var configuredSetting = rootWebConfig.AppSettings.Settings[key];
                    string valueToAdd = null;
                    if (configuredSetting != null) valueToAdd = configuredSetting.Value;
                    settings.Add(key, valueToAdd);
                });

            return settings;
        }

    }
}