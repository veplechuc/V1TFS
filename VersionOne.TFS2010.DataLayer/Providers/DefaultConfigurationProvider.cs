using System;
using Integrations.Core.Interfaces;
using Integrations.Core.Structures;

namespace VersionOneTFS2010.DataLayer.Providers
{
    public sealed class DefaultConfigurationProvider : IConfigurationProvider  
    {
        public bool IsWindowsIntegratedSecurity
        {
            get { return false; }
        }

        public Uri VersionOneUrl
        {
            get { return new Uri("https://www14.v1host.com/v1sdktesting/"); }
        }

        public string VersionOneUserName
        {
            get { return "remote"; }
        }

        public string VersionOnePassword
        {
            get { return "remote"; }
        }

        public Uri TfsUrl
        {
            get { return new Uri("http://localhost:8080/tfs/DefaultCollection/"); }
        }

        public Uri BaseListenerUrl
        {
            get{ return new Uri("http://localhost:9090/");}
        }

        public Uri TfsListenerUrl
        {
            get{ return new Uri(new Uri(BaseListenerUrl.ToString()), UriElements.ServiceName);}
        }

        public Uri ConfigurationUrl
        {
            get { return new Uri(new Uri(BaseListenerUrl.ToString()), UriElements.ConfigurationPath); }
        }

        public string TfsUserName
        {
            get { return "Administrator"; }
        }
        public string TfsPassword
        {
            get { return string.Empty; }
        }
        public string TfsWorkItemRegex
        {
            get { return "[A-Z]{1,2}-[0-9]+"; }
        }
        public bool DebugMode
        {
            get { return true; }
        }

        public IProxyConnectionSettings ProxySettings
        {
            get
            {
                return new DefaultProxySettingsProvider();
            }
        }

        public void ClearAllSettings()
        {
            throw new NotImplementedException();
        }
    }
}