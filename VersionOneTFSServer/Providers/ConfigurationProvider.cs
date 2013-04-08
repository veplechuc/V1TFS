using System.Collections.Generic;
using System.Security.Policy;
using VersionOne.TFS2010.DataLayer.Interfaces;
using VersionOneTFSServer.Adapters;
using VersionOneTFSServer.Interfaces;

namespace VersionOneTFSServer.Providers
{

    public struct AppSettingKeys
    {
        public const string Integrated = "Integrated";
        public const string V1Url = "V1Url";
        public const string UserName = "UserName";
        public const string Password = "Password";
        public const string ProxyEnabled = "ProxyEnabled";
        public const string ProxyUrl = "ProxyUrl";
        public const string ProxyDomain = "ProxyDomain";
        public const string ProxyUserName = "ProxyUserName";
        public const string ProxyPassword = "ProxyPassword";
    }

    /// <summary>
    /// Provided access to a collectionization of the AppSettingsKeys structure.
    /// </summary>
    public static class AppSettingKeysCollection
    {

        public static Dictionary<string, string> Retrieve()
        {

            var keysCollection = new Dictionary<string, string>();
            var instance = new AppSettingKeys();
            foreach (var field in typeof(AppSettingKeys).GetFields())
            {
                if (field.IsPrivate) continue;
                keysCollection.Add(field.Name, field.GetValue(instance) as string);
            }

            return keysCollection;

        }
    }

    /// <summary>
    /// Provides configuration information for the 
    /// </summary>
    public class ConfigurationProvider : IConfigurationProvider
    {

        public bool Integrated { get; private set; }
        public Url V1Url { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public IProxyConnectionSettings ProxySettings { get; private set; }

        public void ResetDefaults()
        {

            
            WebConfigurationAdapter.ClearAllAppSettings();
        }
    }
}