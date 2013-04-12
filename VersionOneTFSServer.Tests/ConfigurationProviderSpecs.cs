using Integrations.Core.Adapters;
using Integrations.Core.Interfaces;
using NSpec;
using VersionOneTFSServer.Providers;

namespace VersionOneTFSServer.Tests
{
    public class ConfigurationProviderSpecs : nspec
    {
        private IConfigurationProvider _target = null;
        private IConfigurationProvider _defaults = null;

        void given_versionOne_specific_settings_are_cleared_from_the_web_config()
        {

            before = () =>
                {
                    _target = new ConfigurationProvider();
                    _defaults = new DefaultConfigurationProvider();
                    WebConfigurationAdapter.ClearAllAppSettings();
                };

            context["when i retrieve versionone specific settings"] = () =>
                {

                    it["then the defaults are returned"] = () =>
                        {
                            _target.UserName.should_be(_defaults.UserName);
                            _target.Password.should_be(_defaults.Password);
                            _target.VersionOneUrl.should_be(_defaults.VersionOneUrl);
                            _target.WindowsIntegratedSecurity.should_be(_defaults.WindowsIntegratedSecurity);
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
                WebConfigurationAdapter.ClearAllAppSettings();
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
