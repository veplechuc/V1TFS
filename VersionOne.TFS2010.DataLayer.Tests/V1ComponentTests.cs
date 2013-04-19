using System;
using NSpec;
using VersionOneProcessorSettings = VersionOne.ServiceHost.Core.Configuration.VersionOneSettings;

namespace VersionOne.TFS2010.DataLayer.Tests
{
    public class V1ComponentTests : nspec
    {
        public void given_settings_are_being_converted()
        {
            context["when i convert VersionOneSettings to VersionOneProcessorSettings"] = () =>
                {

                    var source = new VersionOneSettings()
                        {
                            Integrated = false,
                            Username = "Administrator",
                            Password = "abcd123456",
                            Path = "//file/path",
                            ProxySettings = new ProxyConnectionSettings()
                                {
                                    Domain = "AD",
                                    Username = "ProxyAdmin",
                                    Password = "ProxyPass",
                                    Url = new Uri("http://localProxy:9999/"),
                                    UseProxy = true
                                }
                        };

                    var destination = V1Component.ConvertSettings(source);

                it["then all settings are converted successfully"] = () =>
                    {
                        destination.Url.should_be(source.Path);
                        destination.Username.should_be(source.Username);
                    };
            };
        }
    }
}
