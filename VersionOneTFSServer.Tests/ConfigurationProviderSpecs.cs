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
        private const string FileName = "settings.ini";

        public void given_versionOne_specific_settings_are_not_yet_saved_in_the_web_config()
        {
            before = () =>
                {
                    _target = new ConfigurationProvider();
                    _defaults = new DefaultConfigurationProvider();
                    _target.ClearAllSettings();
                };

            context["when i retrieve versionone specific settings"] = () =>
                {
                    it["then the defaults are returned"] = () =>
                        {
                            _target = new ConfigurationProvider();
                            _target.VersionOneUserName.should_be(_defaults.VersionOneUserName);
                            _target.VersionOnePassword.should_be(_defaults.VersionOnePassword);
                            _target.VersionOneUrl.should_be(_defaults.VersionOneUrl);
                            _target.IsWindowsIntegratedSecurity.should_be(_defaults.IsWindowsIntegratedSecurity);
                            _target.TfsUrl.should_be(_defaults.TfsUrl);
                            _target.TfsListenerUrl.should_be(_defaults.TfsListenerUrl);
                            _target.TfsUserName.should_be(_defaults.TfsUserName);
                            _target.TfsPassword.should_be(_defaults.TfsPassword);
                        };
                };
        }

        public void given_versionOne_specific_settings_are_saved_in_the_web_config()
        {
            const string userName = "User1";
            const string password = "P@ssword1";
            const string v1url = "https://www14.v1host.com/v1sdktesting/";
            const string tfsurl = "http://vsts2012:8080/tfs/DefaultCollection/";
            const string tfsListenerUrl = "http://localhost:5050/servicesRus.svc";
            const string tfsuser = "TfsUser1";
            const string tfspass = "MySecretPw";
            const bool useWindowsSecurity = false;

            before = () =>
                {
                    _target = new ConfigurationProvider();
                    _target.ClearAllSettings();

                    SettingsFileAdapter.SaveSettings(new Dictionary<string, string>()
                        {
                            {AppSettingKeys.VersionOneUserName, userName},
                            {AppSettingKeys.VersionOnePassword, password},
                            {AppSettingKeys.VersionOneUrl, v1url},
                            {AppSettingKeys.IsWindowsIntegratedSecurity, useWindowsSecurity.ToString()},
                            {AppSettingKeys.TfsUrl, tfsurl.ToString()},
                            {AppSettingKeys.TfsListenerUrl, tfsListenerUrl.ToString()},
                            {AppSettingKeys.TfsUserName, tfsuser},
                            {AppSettingKeys.TfsPassword, tfspass}
                        }, Paths.ConfigurationDirectory, FileName);
                };

            context["when i retrieve versionone specific settings"] = () =>
                {
                    it["then the expected settings are returned"] = () =>
                        {
                            _target = new ConfigurationProvider();
                            _target.VersionOneUserName.should_be(userName);
                            _target.VersionOnePassword.should_be(password);
                            _target.VersionOneUrl.ToString().should_be(v1url);
                            _target.IsWindowsIntegratedSecurity.should_be(useWindowsSecurity);
                            _target.TfsUrl.ToString().should_be(tfsurl);
                            _target.TfsListenerUrl.ToString().should_be(tfsListenerUrl);
                            _target.TfsUserName.should_be(tfsuser);
                            _target.TfsPassword.should_be(tfspass);
                        };
                };
        }
    }
}