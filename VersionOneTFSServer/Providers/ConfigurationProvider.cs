using System;
using System.Collections.Generic;
using System.IO;
using Integrations.Core.Adapters;
using Integrations.Core.Interfaces;
using VersionOne.TFS2010.DataLayer;
using VersionOneTFSServer.Collections;

namespace VersionOneTFSServer.Providers
{
    /// <summary>
    /// Provides access to V1 specific settings in the appSettings section of the root web config.
    /// </summary>
    public class ConfigurationProvider : IConfigurationProvider
    {
        private readonly IConfigurationProvider _configurationDefaults;
        private readonly Dictionary<string, string> _savedSettings; 

        public ConfigurationProvider()
        {
            _configurationDefaults = new DefaultConfigurationProvider();
            _savedSettings = GetExistingSettings();
        }

        private static Dictionary<string, string> GetExistingSettings()
        {
            if (Directory.Exists(Paths.ConfigurationDirectory) == false
                || File.Exists(Paths.ConfigurationPath) == false) return null;

            var returnValue = new Dictionary<string, string>();

            using (var reader = new StreamReader(Paths.ConfigurationPath))
            {
                string commaDelimitedLine;
                while ((commaDelimitedLine = reader.ReadLine()) != null)
                {
                    var parsedValues = commaDelimitedLine.Split(',');
                    returnValue.Add(parsedValues[0], parsedValues[1]);
                }
            }

            return returnValue;

        }

        public void ClearAllSettings()
        {
            if (File.Exists(Paths.ConfigurationPath)) File.Delete(Paths.ConfigurationPath);
        }

        private string GetStoredSetting(string key)
        {

            if (_savedSettings == null || _savedSettings.ContainsKey(key) == false) return null;
            return _savedSettings[key].Trim();
        }

        private T GetAppSetting<T>(string key, T defaultValue)
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

        public bool IsWindowsIntegratedSecurity
        {
            get { return GetAppSetting(AppSettingKeys.IsWindowsIntegratedSecurity, _configurationDefaults.IsWindowsIntegratedSecurity); }
        }

        public Uri VersionOneUrl
        {
            get { return GetUri(AppSettingKeys.VersionOneUrl, _configurationDefaults.VersionOneUrl); }
        }

        public string VersionOneUserName
        {
            get { return GetAppSetting(AppSettingKeys.VersionOneUserName, _configurationDefaults.VersionOneUserName); }
        }

        public string VersionOnePassword
        {
            get { return GetAppSetting(AppSettingKeys.VersionOnePassword, _configurationDefaults.VersionOnePassword); }
        }

        public Uri TfsUrl
        {
            get { return GetUri(AppSettingKeys.TfsUrl, _configurationDefaults.TfsUrl); }
        }
        
        public string TfsUserName
        {
            get{ return GetAppSetting(AppSettingKeys.TfsUserName, _configurationDefaults.TfsUserName); }
        }

        public string TfsPassword
        {
            get { return GetAppSetting(AppSettingKeys.TfsPassword, _configurationDefaults.TfsPassword); }
        }

        public string TfsWorkItemRegex
        {
            get { return GetAppSetting(AppSettingKeys.TfsWorkItemRegex, _configurationDefaults.TfsWorkItemRegex); }
        }

        public bool DebugMode
        {
            get { return GetAppSetting(AppSettingKeys.DebugMode, _configurationDefaults.DebugMode); }
        }

        public IProxyConnectionSettings ProxySettings
        {
            get { return new ProxySettingsProvider(); }
        }

    }
}