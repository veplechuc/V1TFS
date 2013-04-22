using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Integrations.Core.DTO
{
    public class TfsServerConfiguration
    {
        public bool IsWindowsIntegratedSecurity;
        public string VersionOneUrl;
        public string UserName;
        public string Password;
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
