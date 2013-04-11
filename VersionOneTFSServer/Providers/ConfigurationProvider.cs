using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using VersionOne.TFS2010.DataLayer;
using VersionOne.TFS2010.DataLayer.Interfaces;
using VersionOneTFSServer.Adapters;
using VersionOneTFSServer.Collections;
using VersionOneTFSServer.Interfaces;

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
            if (storedValue == null || string.IsNullOrEmpty(storedValue)) return defaultValue;
            var setting = (T)Convert.ChangeType(storedValue, type);
            return setting;
        }

        public bool WindowsIntegratedSecurity
        {
            get { return GetSetting(AppSettingKeys.WindowsIntegratedSecurity, _configurationDefaults.WindowsIntegratedSecurity); }
        }

        public Url VersionOneUrl
        {
            get { return GetSetting(AppSettingKeys.VersionOneUrl, _configurationDefaults.VersionOneUrl); }
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