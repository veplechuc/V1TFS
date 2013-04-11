using System.Collections.Generic;
using NSpec;
using VersionOneTFSServer.Adapters;

namespace VersionOneTFSServer.Tests
{
    public class WebConfigurationAdapterSpecs : nspec
    {

        public void given_app_settings_are_being_retrieved_from_a_web_config()
        {

            context["when i retrieve multiple settings that do not exist"] = () =>
                {

                    const string key1 = "SomeSettingThatDoesntExist1";
                    const string key2 = "SomeSettingThatDoesntExist2";
                    var values = WebConfigurationAdapter.GetAppSettings(key1, key2);

                    it["then a null value is returned for each key specified"] = () =>
                        {
                            values.should_contain(x => x.Key == key1);
                            values[key1].should_be(null);
                            values.should_contain(x => x.Key == key2);
                            values[key2].should_be(null);
                        };
                };

            context["when i retrieve a single value that doesn't exist"] = () =>
                {

                    it["then a null value is returned for the key specified"] = () =>
                        {
                            var result = WebConfigurationAdapter.GetAppSetting("LookAwayIDontExist");
                            result.should_be(null);
                        };
                };

            context["when i retrieve a single value that exists"] = () =>
                {
                    const string key1 = "MySetting1";
                    const string val1 = "MyValue1";

                    before = () =>
                        {
                            var settingsToSave = new Dictionary<string, string> {{key1, val1}};
                            WebConfigurationAdapter.SaveAppSettings(settingsToSave);
                        };

                    it["then the value is retrieved successfully"] = () =>
                        {
                            var result = WebConfigurationAdapter.GetAppSetting("MySetting1");
                            result.should_be(val1);
                        };

                    after = WebConfigurationAdapter.ClearAllAppSettings;

                };

            context["when i retrieve multiple settings that do exist"] = () =>
                {

                    const string key1 = "Key1";
                    const string key2 = "Key2";
                    const string val1 = "Value1";
                    const string val2 = "Value2";

                    before = () =>
                        {
                            var settingsToSave = new Dictionary<string, string>
                                {
                                    {key1, val1},
                                    {key2, val2}
                                };

                            WebConfigurationAdapter.ClearAllAppSettings();
                            WebConfigurationAdapter.SaveAppSettings(settingsToSave);

                        };
                    it["then each value is returned successfully"] = () =>
                        {
                            var settings = WebConfigurationAdapter.GetAppSettings(key1, key2);
                            settings.Count.should_be(2);
                            settings[key1].should_be(val1);
                            settings[key2].should_be(val2);
                        };

                };

            context["when i retrieve all settings from the appSettings section of the web config"] = () =>
                {
                    
                    const string key1 = "Key1";
                    const string key2 = "Key2";
                    const string val1 = "Value1";
                    const string val2 = "Value2";

                    before = () =>
                        {

                            var settingsToSave = new Dictionary<string, string>
                                {
                                    {key1, val1},
                                    {key2, val2}
                                };

                            WebConfigurationAdapter.ClearAllAppSettings();
                            WebConfigurationAdapter.SaveAppSettings(settingsToSave);

                        };

                    it["then all settings are returned properly"] = () =>
                        {
                            var settings = WebConfigurationAdapter.GetAllAppSettings();
                            settings.Count.should_be(2);
                            settings[key1].should_be(val1);
                            settings[key2].should_be(val2);
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

            context["when i retrieve multiple settings from the web config that already exist"] = () =>
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

                    it["then all settings should be returned accurately"] = () =>
                        {
                            var retrievedSettings = WebConfigurationAdapter.GetAppSettings(setting1Key, setting2Key, setting3Key);
                            retrievedSettings[setting1Key].should_be(setting1Vlaue);
                            retrievedSettings[setting2Key].should_be(setting2Value);
                            retrievedSettings[setting3Key].should_be(setting3Value);
                        };
                };

        }

        public void given_app_settings_are_cleared_from_the_web_config()
        {

            const string setting1Key = "MyKey1";
            const string setting1Value = "MySettingValue1";

            context["when all settings are cleared from the app settings section of the web config"] = () =>
                {

                    before = () => WebConfigurationAdapter.SaveAppSettings(new Dictionary<string, string> { { setting1Key, setting1Value } });
                    
                    it["then no settings are returned on retrieval"] = () =>
                    {
                            var settings = WebConfigurationAdapter.GetAppSettings(setting1Key);
                            settings[setting1Key].should_be(setting1Value);
                            WebConfigurationAdapter.ClearAllAppSettings();
                            settings = WebConfigurationAdapter.GetAppSettings(setting1Key);
                            settings[setting1Key].should_be(null);
                     };

                    after = WebConfigurationAdapter.ClearAllAppSettings;

                };

            context["when specific settings are cleared from the app settings section of the web config"] = () =>
                {

                    //save four key value pairs to the web configs app settings section

                    const string key1 = "UserName";
                    const string key2 = "Password";
                    const string key3 = "WindowsIntegratedSecurity";
                    const string key4 = "VersionOneUrl";
                    const string value1 = "admin";
                    const string value2 = "adminpw";
                    const string value3 = "false";
                    const string value4 = "http://www14.v1host.com/v1sdktesting/";

                    var tempSettings = new Dictionary<string, string>
                                {
                                    {key1, value1},
                                    {key2, value2},
                                    {key3, value3},
                                    {key4, value4}
                                };

                    before = () =>
                        {
                            
                            WebConfigurationAdapter.ClearAllAppSettings();
                            WebConfigurationAdapter.SaveAppSettings(tempSettings);

                        };

                    it["then the settings cleared are no longer available on retrieval"] = () =>
                        {

                            //clear two settings previously created
                            WebConfigurationAdapter.ClearAppSettings(key3, key4);

                            //test that the two settings are not available on retrieval 
                            var settings = WebConfigurationAdapter.GetAllAppSettings();
                            settings.Count.should_be(2);
                            settings.Keys.should_not_contain(key3);
                            settings.Keys.should_not_contain(key4);

                            it["and the settings NOT cleared are still present and properly valued"] = () =>
                                {
                                    settings[key1].should_be(value1);
                                    settings[key2].should_be(value2);
                                };
                    };

                    it["(overload) then the settings cleared are no longer available on retrieval"] = () =>
                        {

                            WebConfigurationAdapter.ClearAppSettings(tempSettings.Keys);
                            var settings = WebConfigurationAdapter.GetAllAppSettings();
                            settings.Count.should_be(0);

                        };

                };
        }
    }
}
