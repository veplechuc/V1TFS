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

        public Url VersionOneUrl
        {
            get { return new Url("http://localhost/VersionOne/"); }
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