using System.Collections.Generic;
using System.Web.Http;
using Integrations.Core.Adapters;
using Integrations.Core.DTO;
using VersionOneTFSServer.Collections;
using VersionOneTFSServer.Providers;

namespace VersionOneTFSServer
{
    public class ConfigurationController : ApiController
    {
        // GET <controller>
        public TfsServerConfiguration Get()
        {

            var configProvider = new ConfigurationProvider();

            var config = new TfsServerConfiguration
                {
                    VersionOneUrl = configProvider.VersionOneUrl.ToString(),
                    VersionOnePassword = configProvider.VersionOnePassword,
                    VersionOneUserName = configProvider.VersionOneUserName,
                    TfsUrl = configProvider.TfsUrl.ToString(),
                    TfsUserName = configProvider.TfsUserName,
                    TfsPassword = configProvider.TfsPassword,
                    IsWindowsIntegratedSecurity = configProvider.IsWindowsIntegratedSecurity,
                    DebugMode = configProvider.DebugMode
                };
            
            if (configProvider.ProxySettings.ProxyIsEnabled)
            {
                config.ProxyDomain = configProvider.ProxySettings.Domain;
                config.ProxyIsEnabled = configProvider.ProxySettings.ProxyIsEnabled;
                config.ProxyUrl = configProvider.ProxySettings.Url.ToString();
                config.ProxyUsername = configProvider.ProxySettings.Username;
                config.ProxyPassword = configProvider.ProxySettings.Password;
            }
        
            return config;

        }

        // POST <controller>
        public void Post([FromBody]TfsServerConfiguration config)
        {
            var configToSave = new Dictionary<string, string>
                {
                    {AppSettingKeys.VersionOneUrl, config.VersionOneUrl},
                    {AppSettingKeys.VersionOnePassword, config.VersionOnePassword},
                    {AppSettingKeys.VersionOneUserName, config.VersionOneUserName},
                    {AppSettingKeys.TfsUrl, config.TfsUrl},
                    {AppSettingKeys.TfsUserName, config.TfsUserName},
                    {AppSettingKeys.TfsPassword, config.TfsPassword},
                    {AppSettingKeys.IsWindowsIntegratedSecurity, config.IsWindowsIntegratedSecurity.ToString()},
                    {AppSettingKeys.DebugMode, config.DebugMode.ToString()}
                };

            WebConfigurationAdapter.SaveAppSettings(configToSave);
        }

    }
}