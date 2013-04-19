using System;
using Integrations.Core.Adapters;
using Integrations.Core.Interfaces;
using VersionOneTFSServer.Collections;

namespace VersionOneTFSServer.Providers
{
    public class ProxySettingsProvider : IProxyConnectionSettings
    {
        private readonly IProxyConnectionSettings _defaults;

        public ProxySettingsProvider()
        {
            _defaults = new DefaultProxySettingsProvider();
        }

        public bool ProxyIsEnabled
        {
            get { return WebConfigurationAdapter.GetAppSetting(AppSettingKeys.ProxyIsEnabled, _defaults.ProxyIsEnabled); }
        }

        public Uri Url
        {
            get { return WebConfigurationAdapter.GetUri(AppSettingKeys.ProxyUrl, _defaults.Url); }
        }
        public string Domain
        {
            get { return WebConfigurationAdapter.GetAppSetting(AppSettingKeys.ProxyDomain, _defaults.Domain); }
        }
        public string Username
        {
            get { return WebConfigurationAdapter.GetAppSetting(AppSettingKeys.ProxyUserName, _defaults.Username); }
        }
        public string Password
        {
            get { return WebConfigurationAdapter.GetAppSetting(AppSettingKeys.ProxyPassword, _defaults.Password); }
        }
    }
}