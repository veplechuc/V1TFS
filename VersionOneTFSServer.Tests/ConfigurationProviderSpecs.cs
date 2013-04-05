using System;
using NSpec;
using VersionOneTFSServer.Interfaces;
using VersionOneTFSServer.Providers;

namespace VersionOneTFSServer.Tests
{
    public class ConfigurationProviderSpecs : nspec
    {
        //context methods require an underscore. For more info see DefaultConventions.cs.

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
