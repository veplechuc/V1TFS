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

        private string GetStoredSetting(string key)
        {

            if (_savedSettings == null || _savedSettings.ContainsKey(key) == false) return null;
            return _savedSettings[key].Trim();
        }

        private T GetSetting<T>(string key, T defaultValue)
        {
            var type = typeof(T);
            var storedValue = GetStoredSetting(key);
            if (string.IsNullOrEmpty(storedValue)) return defaultValue;
            return (T)Convert.ChangeType(storedValue, type);
        }

        private Uri GetUri(string key, Uri defaultValue)
        {
            var storedValue = GetStoredSetting(key);
            return string.IsNullOrEmpty(storedValue) ? defaultValue : new Uri(storedValue);
        }

        public bool ProxyIsEnabled
        {
            get { return GetSetting(AppSettingKeys.ProxyIsEnabled, _defaults.ProxyIsEnabled); }
        }

        public Uri Url
        {
            get { return GetUri(AppSettingKeys.ProxyUrl, _defaults.Url); }
        }

        public string Domain
        {
            get { return GetSetting(AppSettingKeys.ProxyDomain, _defaults.Domain); }
        }

        public string Username
        {
            get { return GetSetting(AppSettingKeys.ProxyUserName, _defaults.Username); }
        }

        public string Password
        {
            get { return GetSetting(AppSettingKeys.ProxyPassword, _defaults.Password); }
        }

    }
}