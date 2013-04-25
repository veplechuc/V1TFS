using Integrations.Core.DTO;
using NSpec;
using Newtonsoft.Json;

namespace VersionOneTFSServer.Tests
{
    public class ConfigurationSerializerSpecs : nspec
    {

        readonly TfsServerConfiguration _serializationTarget = new TfsServerConfiguration()
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

        public void given_configuration_settings_are_being_serialized()
        {
            before = () =>
                {
                };

            context["when i serialize configuration settings"] = () =>
                {
                    it["then the json is valid"] = () =>
                        {
                            var json = JsonConvert.SerializeObject(_serializationTarget);
                            var o = JsonConvert.DeserializeObject<TfsServerConfiguration>(json);
                            o.should_be(_serializationTarget);
                        };
                };
        }
    }
}