using Integrations.Core.Adapters;
using Integrations.Core.DTO;
using Integrations.Core.Structures;
using NSpec;

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

            context["when a valid object is sent to the server"] = () =>
                {
                    //min acceptable valid set of data
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

                            WebConfigurationAdapter.ClearAllAppSettings();
                            var result = new ConfigurationController().Post(postData);
                            var getData = new ConfigurationController().Get();
                            
                            //the data retrieved should equal the data posted
                            getData.should_be(postData);

                            it["and the status of 'ok' is in the result data"] = () =>
                                {
                                    result.should_contain(kvp => kvp.Key == StatusKey.Status);
                                    result[StatusKey.Status].should_be(StatusCode.Ok);
                                };
                        };
                };

            context["when an invalid object is sent to the server"] = () =>
                {
                    
                    //invalid data missing v1url
                    var postData = new TfsServerConfiguration()
                    {
                        TfsUrl = "http://www.mytfsserver.com/default/",
                        TfsUserName = "admin1",
                        TfsPassword = "password1",
                        VersionOneUserName = "admin2",
                        VersionOnePassword = "password2",
                        ProxyIsEnabled = false
                    };

                    it["then a proper list of errors is received"] = () =>
                        {
                            WebConfigurationAdapter.ClearAllAppSettings();
                            var result = new ConfigurationController().Post(postData);
                            result.should_contain(x => x.Key == "VersionOneUrl");
                            result["VersionOneUrl"].should_be(StatusCode.Required);
                            result[StatusKey.Status].should_be(StatusCode.Exception);
                        };
                };

        }

    }

}