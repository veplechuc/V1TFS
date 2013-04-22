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

        public bool IsWindowsIntegratedSecurity
        {
            get { return WebConfigurationAdapter.GetAppSetting(AppSettingKeys.WindowsIntegratedSecurity, _configurationDefaults.IsWindowsIntegratedSecurity); }
        }

        public Uri VersionOneUrl
        {
            get { return WebConfigurationAdapter.GetUri(AppSettingKeys.VersionOneUrl, _configurationDefaults.VersionOneUrl); }
        }

        public string UserName
        {
            get { return WebConfigurationAdapter.GetAppSetting(AppSettingKeys.UserName, _configurationDefaults.UserName); }
        }

        public string Password
        {
            get { return WebConfigurationAdapter.GetAppSetting(AppSettingKeys.Password, _configurationDefaults.Password); }
        }

        public Uri TfsUrl
        {
            get { return WebConfigurationAdapter.GetUri(AppSettingKeys.TfsUrl, _configurationDefaults.TfsUrl); }
        }
        
        public string TfsUserName
        {
            get{ return WebConfigurationAdapter.GetAppSetting(AppSettingKeys.TfsUserName, _configurationDefaults.TfsUserName); }
        }

        public string TfsPassword
        {
            get { return WebConfigurationAdapter.GetAppSetting(AppSettingKeys.TfsPassword, _configurationDefaults.TfsPassword); }
        }

        public string WorkItemRegex
        {
            get { return WebConfigurationAdapter.GetAppSetting(AppSettingKeys.WorkItemRegex); }
        }

        public bool DebugMode
        {
            get { return WebConfigurationAdapter.GetAppSetting(AppSettingKeys.DebugMode, _configurationDefaults.DebugMode); }
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