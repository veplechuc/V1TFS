using System;
using System.Security.Policy;
using Integrations.Core.Interfaces;

namespace VersionOneTFSServer.Providers
{
    public sealed class DefaultConfigurationProvider : IConfigurationProvider  
    {
        public bool WindowsIntegratedSecurity
        {
            get { return false; }
        }

        public Uri VersionOneUrl
        {
            get { return new Uri("http://localhost/VersionOne/"); }
        }

        public string UserName
        {
            get { return "remote"; }
        }

        public string Password
        {
            get { return "remote"; }
        }
        public IProxyConnectionSettings ProxySettings
        {
            get
            {
                return new ProxySettingsProvider();
            }
        }

    }
}