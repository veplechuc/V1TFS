using Integrations.Core.Interfaces;

namespace VersionOneTFSServer.Providers
{
    public class ProxySettingsProvider : IProxyConnectionSettings
    {
        public bool UseProxy { get; set; }
        public string Url { get; set; }
        public string Domain { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}