using System;
using System.Security.Policy;
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

        private static T GetSetting<T>(string key, T defaultValue)
        {
            var type = typeof (T);
            var storedValue = WebConfigurationAdapter.GetAppSetting(key);
            if (string.IsNullOrEmpty(storedValue)) return defaultValue;
            return (T)Convert.ChangeType(storedValue, type);
        }

        private static Uri GetUri(string key, Uri defaultValue)
        {
            var storedValue = WebConfigurationAdapter.GetAppSetting(key);
            return string.IsNullOrEmpty(storedValue) ? defaultValue : new Uri(storedValue);
        }

        public bool WindowsIntegratedSecurity
        {
            get { return GetSetting(AppSettingKeys.WindowsIntegratedSecurity, _configurationDefaults.WindowsIntegratedSecurity); }
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