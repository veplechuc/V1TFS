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

            context["when i create a single new setting in the web config"] = () =>
                {
                    const string key = "MySetting";
                    const string value = "MyValue";
                    it["then the value is saved successfully"] = () =>
                        {
                            var settingsToSave = new Dictionary<string, string> {{key, value}};
                            WebConfigurationAdapter.SaveAppSettings(settingsToSave);
                            var appSettings = WebConfigurationAdapter.GetAppSettings(key);
                            appSettings[key].should_be(value);
                        };



                };

            context["when i update an existing setting in the web config"] = () =>
                {
                    const string key = "MySetting";
                    const string value = "MyValue";

                    before = () => WebConfigurationAdapter.SaveAppSettings(new Dictionary<string, string>{{key, value}});

                    it["then the updated setting is persisted successfully"] = () =>
                        {
                            const string myNewValue = "MyNewValue";
                            var settingToUpdate = new Dictionary<string, string>{{key, myNewValue}};
                            WebConfigurationAdapter.SaveAppSettings(settingToUpdate);
                            var persistedValue = WebConfigurationAdapter.GetAppSettings(key);
                            persistedValue[key].should_be(myNewValue);
                        };

                    after = WebConfigurationAdapter.ClearV1Settings;
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
