using Integrations.Core.DTO;
using NSpec;
using Newtonsoft.Json;

namespace VersionOneTFSServerConfig.Tests
{
    public class ConfiguationSerializerSpecs : nspec
    {
        public void given_configuration_is_being_serialized()
        {

            var serializationTarget = new TfsServerConfiguration();

            before = () =>
                {
                    serializationTarget.DebugMode = false;
                    serializationTarget.IsWindowsIntegratedSecurity = true;
                    serializationTarget.VersionOnePassword = "admin";
                    serializationTarget.ProxyDomain = "AD";
                    serializationTarget.ProxyIsEnabled = true;
                    serializationTarget.ProxyPassword = "123456";
                    serializationTarget.ProxyUrl = "http://myproxy:9191/proxy/";
                    serializationTarget.ProxyUsername = "admin1";
                    serializationTarget.TfsPassword = "abc123";
                    serializationTarget.TfsUrl = "http://localhost/tfs/defaultcollection/";
                    serializationTarget.TfsUserName = "admin3";
                    serializationTarget.TfsWorkItemRegex = null;
                    serializationTarget.VersionOneUserName = "admin2";
                    serializationTarget.VersionOneUrl = "http://www.versionone.com/";
                };

            context["when i serialize the configuration to json"] = () =>
                {

                    var json = JsonConvert.SerializeObject(serializationTarget);

                    it["then the json is valid"] = () =>
                        {
                            //should we be testing the json serializer?
                        };
                };

        }

        public void given_configuration_is_being_deserialized()
        {
            before = () =>
                {

                };

            context["when i deserialize the configuration from json"] = () =>
                {
                    it["then a valid .net object results"] = () =>
                        {

                        };
                };

        }

    }

    

}