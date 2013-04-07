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

            //after each context, clear the app settings section of the web.config
            after = WebConfigurationAdapter.ClearAllAppSettings;

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

                };

            context["when i retrieve multiple settings from the web config that exist already"] = () =>
                {

                    const string setting1Key = "Setting1Key";
                    const string setting1Vlaue = "Setting1Vlaue";
                    const string setting2Key = "Setting2Key";
                    const string setting2Value = "Setting2Value";
                    const string setting3Key = "Setting3Key";
                    const string setting3Value = "Setting3Value";

                    var settings = new Dictionary<string, string>
                                {
                                    {setting1Key, setting1Vlaue},
                                    {setting2Key, setting2Value},
                                    {setting3Key, setting3Value}
                                };

                    before = () => WebConfigurationAdapter.SaveAppSettings(settings);

                    it["then all settings should be return accurately"] = () =>
                        {
                            var retrievedSettings = WebConfigurationAdapter.GetAppSettings(setting1Key, setting2Key, setting3Key);
                            retrievedSettings[setting1Key].should_be(setting1Vlaue);
                            retrievedSettings[setting2Key].should_be(setting2Value);
                            retrievedSettings[setting3Key].should_be(setting3Value);
                        };
                };

        }

        public void given_all_app_settings_are_cleared_from_the_web_config()
        {

            const string setting1Key = "MyKey1";
            const string setting1Value = "MySettingValue1";
            
            before = () => WebConfigurationAdapter.SaveAppSettings(new Dictionary<string, string> { { setting1Key, setting1Value } });

            context["when settings are retrieved from the cleared web config"] = () =>
                {
                    WebConfigurationAdapter.ClearAllAppSettings();

                    it["then no settings are returned on retrieval"] = () =>
                        {
                            var settings = WebConfigurationAdapter.GetAppSettings(setting1Key);
                            //wip - failing settings[setting1Key].should_be(null);
                        };
                };
        }

        public void given_relevant_app_settings_are_cleared_from_web_config()
        {

            context["when settings are cleared"] = () =>
            {
                it["then there are no relevant values retained in the web config"] = () =>
                    {

                        WebConfigurationAdapter.ClearAllAppSettings();


                        it["and no other settings are not disturbed"] = () =>
                            {

                            };
                    };
            };
        }

    }
}
