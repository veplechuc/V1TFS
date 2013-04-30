using System;
using System.Collections.Generic;

namespace VersionOneTFSServer.Providers
{
    public class ProviderUtilities
    {

        public static string GetStoredSetting(IDictionary<string, string> settings, string key)
        {
            if (settings == null || settings.ContainsKey(key) == false) return null;
            return settings[key].Trim();
        }

        public static T GetSetting<T>(IDictionary<string, string> settings, string key, T defaultValue)
        {
            var type = typeof(T);
            var storedValue = GetStoredSetting(settings, key);
            if (string.IsNullOrEmpty(storedValue)) return defaultValue;
            return (T)Convert.ChangeType(storedValue, type);
        }

        public static Uri GetUri(IDictionary<string, string> settings, string key, Uri defaultValue)
        {
            var storedValue = GetStoredSetting(settings, key);
            return string.IsNullOrEmpty(storedValue) ? defaultValue : new Uri(storedValue);
        }

    }
}