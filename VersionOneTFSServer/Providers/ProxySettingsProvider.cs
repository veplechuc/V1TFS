using System;
using Integrations.Core.Interfaces;

namespace VersionOneTFSServer.Providers
{
    public class ProxySettingsProvider : IProxyConnectionSettings
    {
        public bool UseProxy { get; set; }
        public Uri Url { get; set; }
        public string Domain { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}