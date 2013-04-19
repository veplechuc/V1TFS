using Integrations.Core.Adapters;
using Integrations.Core.Interfaces;
using NSpec;
using VersionOneTFSServer.Providers;

namespace VersionOneTFSServer.Tests
{
    public class ProxySettingsProviderSpecs : nspec
    {
        private IProxyConnectionSettings _target = null;
        private IProxyConnectionSettings _defaults = null;

        private void given_versionOne_specific_settings_are_not_yet_saved_in_the_web_config()
        {
            before = () =>
            {
                _target = new ProxySettingsProvider();
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

    }
}
