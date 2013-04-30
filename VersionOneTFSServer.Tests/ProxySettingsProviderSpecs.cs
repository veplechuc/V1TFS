using System.Collections.Generic;
using Integrations.Core.Adapters;
using Integrations.Core.Interfaces;
using NSpec;
using VersionOneTFSServer.Collections;
using VersionOneTFSServer.Providers;

namespace VersionOneTFSServer.Tests
{
    public class ProxySettingsProviderSpecs : nspec
    {

        private IProxyConnectionSettings _target = null;
        private IProxyConnectionSettings _defaults = null;

        public void given_versionOne_specific_settings_are_not_yet_saved_in_the_web_config()
        {
            before = () =>
            {
                _target = new ProxySettingsProvider(null);
                _defaults = new DefaultProxySettingsProvider();
                WebConfigurationAdapter.ClearAllAppSettings();
            };

            context["when i retrieve versionone specific settings"] = () =>
            {
                it["then the defaults are returned"] = () =>
                {
                    _target.ProxyIsEnabled.should_be(_defaults.ProxyIsEnabled);
                    _target.Url.ToString().should_be(_defaults.Url.ToString());
                    _target.Domain.should_be(_defaults.Domain);
                    _target.Username.should_be(_defaults.Username);
                    _target.Password.should_be(_defaults.Password);
                };
            };
        }

        public void given_versionOne_specific_settings_are_saved_in_the_web_config()
        {

            const string proxyIsEnabled = "false";
            const string proxyDomain = "DOMAIN5000";
            const string proxyUrl = "http://myProxyUrl:9192/login/";
            const string proxyUserName = "ProxyAdmin1";
            const string proxyPassword = "ProxyPass1";

            before = () =>
                {

                    var settings = new Dictionary<string, string>()
                        {
                            {AppSettingKeys.ProxyIsEnabled, proxyIsEnabled},
                            {AppSettingKeys.ProxyDomain, proxyDomain},
                            {AppSettingKeys.ProxyUrl, proxyUrl},
                            {AppSettingKeys.ProxyUserName, proxyUserName},
                            {AppSettingKeys.ProxyPassword, proxyPassword}
                        };

                    _target = new ProxySettingsProvider(settings);
                    _defaults = new DefaultProxySettingsProvider();
                    new ConfigurationProvider().ClearAllSettings();

                    SettingsFileAdapter.SaveSettings(settings, Paths.ConfigurationDirectory, Paths.ConfigurationFileName);

                };

            context["when i retrieve versionone specific settings"] = () =>
                {
                    it["then the saved values are returned"] = () =>
                        {
                            _target.ProxyIsEnabled.should_be(bool.Parse(proxyIsEnabled));
                            _target.Domain.should_be(proxyDomain);
                            _target.Url.ToString().should_be(proxyUrl.ToLower()); //uri object stores url string in all lowercase
                            _target.Username.should_be(proxyUserName);
                            _target.Password.should_be(proxyPassword);
                        };
                };

        }

    }
}
