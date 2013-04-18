using System.Collections.Generic;
using Integrations.Core.Adapters;
using Integrations.Core.Interfaces;
using NSpec;
using VersionOneTFSServer.Collections;
using VersionOneTFSServer.Providers;

namespace VersionOneTFSServer.Tests
{
    public class ConfigurationProviderSpecs : nspec
    {
        private IConfigurationProvider _target = null;
        private IConfigurationProvider _defaults = null;

        void given_versionOne_specific_settings_are_not_yet_saved_in_the_web_config()
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

        void given_versionOne_specific_settings_are_saved_in_the_web_config()
        {

            const string userName = "User1";
            const string password = "P@ssword1";
            const string url = "https://www14.v1host.com/v1sdktesting/";
            const string useWindowsSecurity = "false";

            before = () =>
                {
                    _target = new ConfigurationProvider();
                    WebConfigurationAdapter.ClearAllAppSettings();
                    
                    WebConfigurationAdapter.SaveAppSettings(new Dictionary<string, string>()
                        {
                            {AppSettingKeys.UserName, userName}, 
                            {AppSettingKeys.Password, password},
                            {AppSettingKeys.VersionOneUrl, url},
                            {AppSettingKeys.WindowsIntegratedSecurity, useWindowsSecurity}
                        });
                };

            context["when i retrieve versionone specific settings"] = () =>
                {
                    var settings = WebConfigurationAdapter.GetAppSettings(
                        AppSettingKeys.UserName,
                        AppSettingKeys.Password,
                        AppSettingKeys.VersionOneUrl,
                        AppSettingKeys.WindowsIntegratedSecurity
                        );

                    it["then the expected settings are returned"] = () =>
                        {
                            settings[AppSettingKeys.UserName].should_be(userName);
                            settings[AppSettingKeys.Password].should_be(password);
                            settings[AppSettingKeys.VersionOneUrl].should_be(url);
                            settings[AppSettingKeys.WindowsIntegratedSecurity].should_be(useWindowsSecurity);
                        };
                };

        }

    }
}
