using Integrations.Core.DTO;
using NSpec;

namespace VersionOneTFSServerConfig.Tests
{
    public class ConfigFormSpecs : nspec
    {

        private ConfigForm _target;

        public void given_configuration_data_is_being_retrieved()
        {
            context["when configuration settings are retrieved"] = () =>
                {
                    _target = new ConfigForm();
                    var configuration = _target.RetrieveConfigurationData();
                    it["then the configuration data should be available"] = () => configuration.should_not_be(null);
                };

            context["when configuration settings are being stored"] = () =>
                {
                    _target = new ConfigForm();
                    var savedConfig = new TfsServerConfiguration()
                        {
                            DebugMode = false,
                            IsWindowsIntegratedSecurity = true,
                            VersionOnePassword = "admin",
                            ProxyDomain = "AD",
                            ProxyIsEnabled = true,
                            ProxyPassword = "123456",
                            ProxyUrl = "http://myproxy:9191/proxy/",
                            ProxyUsername = "admin1",
                            TfsPassword = "abc123",
                            TfsUrl = "http://localhost/tfs/defaultcollection/",
                            TfsUserName = "admin3",
                            TfsWorkItemRegex = null,
                            VersionOneUserName = "admin2",
                            VersionOneUrl = "http://www.versionone.com/"
                        };

                    _target.StoreConfigurationData(savedConfig);

                    var retrievedConfig = _target.RetrieveConfigurationData();
                    retrievedConfig.should_be(savedConfig);

                };

        }
    }
}
