using System;
using NSpec;
using VersionOneTFSServer.Interfaces;
using VersionOneTFSServer.Providers;

namespace VersionOneTFSServer.Tests
{
    public class ConfigurationProviderSpecs : nspec
    {

        void given_app_setting_keys_are_needed_for_recursion()
        {

            context["when i request a collection of app setting keys"] = () =>
                {
                    it["then i receive a collection to iterate over"] = () =>
                        {
                            var keys = AppSettingKeysCollection.Retrieve();
                            keys.Count.should_not_be(0);
                            foreach (var key in keys)
                            {
                                key.should_not_be(null);
                                key.should_not_be(string.Empty);
                                key.Value.should_not_be(null);
                                key.Value.should_not_be(string.Empty);
                            }
                        };
                };
        }

        void given_configuration_settings_are_not_yet_saved()
        {

            IConfigurationProvider _target = null;

            //runs before each context
            before = () =>
            {
                _target = new ConfigurationProvider();
                _target.ResetDefaults();
            };

            //context["when settings are retrieved for the first time"] = () =>
            //    {
            //        it["then the default settings are returned"] = () =>
            //        {
                        
            //        };
            //};

            //context["when settings are saved for the first time"] = () =>
            //    {
            //        it["then the save completes successfully"] = () =>
            //            {
            //                //throw new NotImplementedException();
            //            };
            //};

        }
    }
}
