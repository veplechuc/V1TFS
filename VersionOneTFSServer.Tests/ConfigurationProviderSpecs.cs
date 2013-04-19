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

        private void given_versionOne_specific_settings_are_not_yet_saved_in_the_web_config()
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
                            _target.IsWindowsIntegratedSecurity.should_be(_defaults.IsWindowsIntegratedSecurity);
                        };
                };
        }

        private void given_versionOne_specific_settings_are_saved_in_the_web_config()
        {
            const string userName = "User1";
            const string password = "P@ssword1";
            const string url = "https://www14.v1host.com/v1sdktesting/";
            const bool useWindowsSecurity = false;

            before = () =>
                {
                    _target = new ConfigurationProvider();
                    WebConfigurationAdapter.ClearAllAppSettings();

                    WebConfigurationAdapter.SaveAppSettings(new Dictionary<string, string>()
                        {
                            {AppSettingKeys.UserName, userName},
                            {AppSettingKeys.Password, password},
                            {AppSettingKeys.VersionOneUrl, url},
                            {AppSettingKeys.WindowsIntegratedSecurity, useWindowsSecurity.ToString()}
                        });
                };

            context["when i retrieve versionone specific settings"] = () =>
                {
                    it["then the expected settings are returned"] = () =>
                        {
                            _target.UserName.should_be(userName);
                            _target.Password.should_be(password);
                            _target.VersionOneUrl.ToString().should_be(url);
                            _target.IsWindowsIntegratedSecurity.should_be(useWindowsSecurity);
                        };
                };
        }
    }
}