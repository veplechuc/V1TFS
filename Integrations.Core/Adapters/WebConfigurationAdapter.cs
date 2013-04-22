using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Web.Configuration;

namespace Integrations.Core.Adapters
{
    public static class WebConfigurationAdapter
    {

        /// <summary>
        /// Returns all appSettings from the root web config.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetAllAppSettings()
        {
            var configuration = GetRootWebConfig();
            var keys = configuration.AppSettings.Settings.AllKeys;
            return keys.ToDictionary(key => key, GetAppSetting);
        } 

        /// <summary>
        /// Retrieves settings from the appSettings section of the root web.config file.
        /// </summary>
        /// <param name="keyNames"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetAppSettings(params string[] keyNames)
        {
            
            if (keyNames.Length == 0) return null;

            var settings = new Dictionary<string, string>();

            foreach (var key in keyNames)
            {
                settings.Add(key, GetAppSetting(key));
            }

            return settings;

        }

        public static string GetAppSetting(string key)
        {
            var configuration = GetRootWebConfig();
            var nameValuePair = configuration.AppSettings.Settings[key];
            return nameValuePair == null ? null : nameValuePair.Value;
        }

        /// <summary>
        /// Generic method to retrieve settings of type T from a web.config.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T GetAppSetting<T>(string key, T defaultValue)
        {
            var type = typeof(T);
            var storedValue = GetAppSetting(key);
            if (string.IsNullOrEmpty(storedValue)) return defaultValue;
            return (T)Convert.ChangeType(storedValue, type);
        }

        /// <summary>
        /// Can't cast a string directly to a URI through the generic <seealso cref="GetSettings">GetSettings</seealso> method.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Uri GetUri(string key, Uri defaultValue)
        {
            var storedValue = GetAppSetting(key);
            return string.IsNullOrEmpty(storedValue) ? defaultValue : new Uri(storedValue);
        }

        /// <summary>
        /// Saves a set of keyvalue pairs to the appSettings element of a web.config.
        /// </summary>
        /// <param name="keyValuePairs"></param>
        public static void SaveAppSettings(Dictionary<string,string> keyValuePairs)
        {

            var configuration = GetRootWebConfig();

            foreach (var keyValuePair in keyValuePairs)
            {
                if (!configuration.AppSettings.Settings.AllKeys.Contains(keyValuePair.Key))
                {
                    configuration.AppSettings.Settings.Add(new KeyValueConfigurationElement(keyValuePair.Key, keyValuePair.Value));
                }
                else
                {
                    configuration.AppSettings.Settings[keyValuePair.Key].Value = keyValuePair.Value;
                }
            }

            SaveConfiguration(configuration);

        }

        /// <summary>
        /// Clears all appSettings from the web.config.
        /// </summary>
        public static void ClearAllAppSettings()
        {
            var configuration = GetRootWebConfig();
            ClearSettings(configuration.AppSettings.Settings.AllKeys, configuration);
        }

        public static void ClearAppSettings(params string[] keys)
        {
            ClearSettings(keys, GetRootWebConfig());
        }

        public static void ClearAppSettings(IEnumerable<string> keys)
        {
            ClearSettings(keys, GetRootWebConfig());
        }

        private static void ClearSettings(IEnumerable<string> keys, Configuration configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");

            var save = false;
            foreach (var key in keys)
            {
                configuration.AppSettings.Settings.Remove(key);
                save = true;
            }

            if (save == true) SaveConfiguration(configuration);

        }

        private static Configuration GetRootWebConfig()
        {
            return WebConfigurationManager.OpenWebConfiguration(null);
        }

        private static void SaveConfiguration(Configuration configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            configuration.Save(ConfigurationSaveMode.Modified);
        }
    }
}