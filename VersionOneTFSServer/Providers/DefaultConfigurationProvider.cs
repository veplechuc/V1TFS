using System.Security.Policy;
using VersionOne.TFS2010.DataLayer;
using VersionOne.TFS2010.DataLayer.Interfaces;
using VersionOneTFSServer.Interfaces;

namespace VersionOneTFSServer.Providers
{
    sealed class DefaultConfigurationProvider : IConfigurationProvider  
    {
        public bool Integrated
        {
            get { return false; }
        }

        public Url V1Url
        {
            get { return new Url("http://localhost/VersionOne/"); }
        }

        public string Username
        {
            get { return "admin"; }
        }

        public string Password
        {
            get { return "admin"; }
        }
        public IProxyConnectionSettings ProxySettings
        {
            get
            {
                return new ProxyConnectionSettings();
            }
        }

        public void ResetDefaults()
        {
            new WebConfigProvider().ClearV1Settings();
        }
    }
}