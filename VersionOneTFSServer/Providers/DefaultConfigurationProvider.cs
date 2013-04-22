using System;
using System.Security.Policy;
using Integrations.Core.Interfaces;

namespace VersionOneTFSServer.Providers
{
    public sealed class DefaultConfigurationProvider : IConfigurationProvider  
    {
        public bool IsWindowsIntegratedSecurity
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

        public Uri TfsUrl
        {
            get
            {
                return new Uri("http://localhost:8080/tfs/DefaultCollection/");
            }
        }

        public string TfsUserName
        {
            get { return "Administrator"; }
        }
        public string TfsPassword
        {
            get { return string.Empty; }
        }
        public string WorkItemRegex
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
                return new ProxySettingsProvider();
            }
        }

    }
}