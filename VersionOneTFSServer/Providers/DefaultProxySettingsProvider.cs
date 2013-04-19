using System;
using Integrations.Core.Interfaces;

namespace VersionOneTFSServer.Providers
{
    public class DefaultProxySettingsProvider : IProxyConnectionSettings
    {
        public bool UseProxy
        {
            get { return false; }
        }
        public Uri Url
        {
            get { return new Uri("http://192.168.1.1/"); }
        }
        public string Domain
        {
            get { return "AD"; }
        }
        public string Username
        {
            get { return "Administrator"; }
        }
        public string Password
        {
            get { return "123456"; }
        }
    }
}