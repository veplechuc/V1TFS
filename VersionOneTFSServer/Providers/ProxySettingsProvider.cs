using System;
using System.Collections.Generic;
using Integrations.Core.Interfaces;
using VersionOneTFSServer.Collections;

namespace VersionOneTFSServer.Providers
{
    public class ProxySettingsProvider : IProxyConnectionSettings
    {

        private readonly IProxyConnectionSettings _defaults;
        private readonly Dictionary<string, string> _savedSettings; 

        public ProxySettingsProvider(Dictionary<string, string> savedSettings)
        {
            _defaults = new DefaultProxySettingsProvider();
            _savedSettings = savedSettings;
        }

        public bool ProxyIsEnabled
        {
            get { return ProviderUtilities.GetSetting(_savedSettings, AppSettingKeys.ProxyIsEnabled, _defaults.ProxyIsEnabled); }
        }

        public Uri Url
        {
            get { return ProviderUtilities.GetUri(_savedSettings, AppSettingKeys.ProxyUrl, _defaults.Url); }
        }

        public string Domain
        {
            get { return ProviderUtilities.GetSetting(_savedSettings, AppSettingKeys.ProxyDomain, _defaults.Domain); }
        }

        public string Username
        {
            get { return ProviderUtilities.GetSetting(_savedSettings, AppSettingKeys.ProxyUserName, _defaults.Username); }
        }

        public string Password
        {
            get { return ProviderUtilities.GetSetting(_savedSettings, AppSettingKeys.ProxyPassword, _defaults.Password); }
        }

    }
}