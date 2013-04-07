using System.Collections.Generic;
using NSpec;
using VersionOneTFSServer.Adapters;

namespace VersionOneTFSServer.Tests
{
    public class WebConfigurationAdapterSpecs : nspec
    {

        public void given_app_settings_are_being_retrieved_from_a_web_config()
        {

            context["when i retrieve a setting that does not exist"] = () =>
                {
                    it["then a null value is returned for the key specified"] = () =>
                        {
                            const string keyNames = "SomeSettingThatDoesntExist";
                            var values = WebConfigurationAdapter.GetAppSettings(keyNames);
                            values.should_contain(x => x.Key == keyNames);
                            values[keyNames].should_be(null);
                        };
                };

            context["when i retrieve multiple settings that do exist"] = () =>
                {
                    before = () =>
                        {
                            //setup multiple settings
                        };
                    it["then each value is returned successfully"] = () =>
                        {

                        };

                };
        }

        public void given_app_settings_are_being_written_to_a_web_config()
        {

            context["when i save a new setting the web config"] = () =>
                {
                    var key = "MySettings";
                    it["then the value is saved successfully"] = () =>
                        {
                            var settingsToSave = new Dictionary<string, string> {{key, "MyValue"}};
                            WebConfigurationAdapter.Save(settingsToSave);
                            var appSettings = WebConfigurationAdapter.GetAppSettings(key);
                            appSettings[key].should_be("MyValue");
                        };
                };
        }

        public void given_relevant_app_settings_are_cleared_from_web_config()
        {

            context["when settings are cleared"] = () =>
            {
                it["then there are no relevant values retained in the web config"] = () =>
                    {

                        WebConfigurationAdapter.ClearV1Settings();


                        it["and no other settings are not disturbed"] = () =>
                            {

                            };
                    };
            };
        }

    }
}
