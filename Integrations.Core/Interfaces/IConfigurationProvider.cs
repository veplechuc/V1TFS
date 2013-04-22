using System;

namespace Integrations.Core.Interfaces
{
    public interface IConfigurationProvider
    {
        bool IsWindowsIntegratedSecurity { get; }
        Uri VersionOneUrl { get; }
        string UserName { get; } 
        string Password { get; }
        Uri TfsUrl { get; }
        string TfsUserName { get; }
        string TfsPassword { get; }
        string WorkItemRegex { get; }
        bool DebugMode { get; }
        IProxyConnectionSettings ProxySettings { get; }
    }
}
