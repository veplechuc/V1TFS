using System.Security.Policy;
using VersionOne.TFS2010.DataLayer;
using VersionOne.TFS2010.DataLayer.Interfaces;
using VersionOneTFSServer.Adapters;
using VersionOneTFSServer.Interfaces;

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
                return new ProxyConnectionSettings();
            }
        }

    }
}