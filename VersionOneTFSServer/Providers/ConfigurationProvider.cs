using System;
using Integrations.Core.Adapters;
using Integrations.Core.Interfaces;
using VersionOneTFSServer.Collections;

namespace VersionOneTFSServer.Providers
{

    /// <summary>
    /// Provides access to V1 specific settings in the appSettings section of the root web config.
    /// </summary>
    public class ConfigurationProvider : IConfigurationProvider
    {

        private readonly IConfigurationProvider _configurationDefaults;

        public ConfigurationProvider()
        {
            _configurationDefaults = new DefaultConfigurationProvider();
        }

        /// <summary>
        /// Generic method to retrieve settings of type T from a web.config.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private static T GetSetting<T>(string key, T defaultValue)
        {
            var type = typeof (T);
            var storedValue = WebConfigurationAdapter.GetAppSetting(key);
            if (string.IsNullOrEmpty(storedValue)) return defaultValue;
            return (T)Convert.ChangeType(storedValue, type);
        }

        /// <summary>
        /// Can't cast a string directly to a URI through the generic <seealso cref="GetSettings">GetSettings</seealso> method.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private static Uri GetUri(string key, Uri defaultValue)
        {
            var storedValue = WebConfigurationAdapter.GetAppSetting(key);
            return string.IsNullOrEmpty(storedValue) ? defaultValue : new Uri(storedValue);
        }

        public bool IsWindowsIntegratedSecurity
        {
            get { return GetSetting(AppSettingKeys.WindowsIntegratedSecurity, _configurationDefaults.IsWindowsIntegratedSecurity); }
        }

        public Uri VersionOneUrl
        {
            get { return GetUri(AppSettingKeys.VersionOneUrl, _configurationDefaults.VersionOneUrl); }
        }

        public string UserName
        {
            get { return GetSetting(AppSettingKeys.UserName, _configurationDefaults.UserName); }
        }

        public string Password
        {
            get { return GetSetting(AppSettingKeys.Password, _configurationDefaults.Password); }
        }

        public IProxyConnectionSettings ProxySettings
        {
            get { return null; }
        }

        public void ResetDefaults()
        {
            WebConfigurationAdapter.ClearAppSettings(new AppSettingKeyCollection().Values);
        }
    }
}