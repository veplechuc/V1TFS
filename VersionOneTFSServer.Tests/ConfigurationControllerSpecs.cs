using Integrations.Core.Adapters;
using Integrations.Core.DTO;
using NSpec;
using Newtonsoft.Json;
namespace VersionOneTFSServer.Tests
{

    public class ConfigurationControllerSpecs : nspec
    {
        public void given_settings_are_being_retrieved_from_the_controller()
        {
            context["when a parameterless http get is issued"] = () =>
                {
                    var config = new ConfigurationController().Get();
                    it["then valid json is returned"] = () => config.should_not_be(null);
                };
        }

        public void given_settings_are_being_sent_to_the_controller()
        {

            before = WebConfigurationAdapter.ClearAllAppSettings;

            context["when a valid object is sent to the server"] = () =>
                {
                    //min acceptable data
                    var postData = new TfsServerConfiguration()
                    {
                        TfsUrl = "http://www.mytfsserver.com/default/",
                        TfsUserName = "admin1",
                        TfsPassword = "password1",
                        VersionOneUrl = "http://www.versionone.com/",
                        VersionOneUserName = "admin2",
                        VersionOnePassword = "password2",
                        ProxyIsEnabled = false
                    };

                    it["then the data is saved accurately"] = () =>
                        {
                            new ConfigurationController().Post(postData);
                            var getData = new ConfigurationController().Get();
                            //the data retrieved should equal the data posted
                            getData.should_be(postData);
                        };
                };

            context["when an invalid object is sent to the server"] = () =>
                {
                    it["then a proper exception message is recieved"] = () =>
                        {

                        };
                };

        }

    }

}