using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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

        private readonly IConfigurationProvider _defaults;

        public ConfigurationProvider()
        {
            _defaults = new DefaultConfigurationProvider();
        }

        private T GetSetting<T>(T storedSetting)
        {
            var returnValue = default(T);
            return returnValue;
        }

        public bool Integrated
        {
            get { return false; }
        }

        public Url V1Url { get; private set; }
        public string UserName
        {
            get
            {
                var userName = WebConfigurationAdapter.GetAppSettings(AppSettingKeys.UserName)[AppSettingKeys.UserName];
                if (!string.IsNullOrEmpty(userName)) return userName;
                return _defaults.UserName;
            }
        }
        public string Password { get; private set; }
        public IProxyConnectionSettings ProxySettings { get; private set; }

        public void ResetDefaults()
        {
            WebConfigurationAdapter.ClearAppSettings(new AppSettingKeyCollection().Values);
        }
    }
}