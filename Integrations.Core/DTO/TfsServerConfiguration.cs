namespace Integrations.Core.DTO
{
    public struct TfsServerConfiguration
    {
        public bool IsWindowsIntegratedSecurity;
        public string VersionOneUrl;
        public string VersionOneUserName;
        public string VersionOnePassword;
        public string TfsUrl;
        public string TfsUserName;
        public string TfsPassword;
        public string TfsWorkItemRegex;
        public bool DebugMode;
        public bool ProxyIsEnabled;
        public string ProxyUrl;
        public string ProxyDomain;
        public string ProxyUsername;
        public string ProxyPassword;

    }
}
