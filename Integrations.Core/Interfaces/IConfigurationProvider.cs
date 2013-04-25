using System;

namespace Integrations.Core.Interfaces
{
    public interface IConfigurationProvider
    {
        bool IsWindowsIntegratedSecurity { get; }
        Uri VersionOneUrl { get; }
        string VersionOneUserName { get; } 
        string VersionOnePassword { get; }
        Uri TfsUrl { get; }
        string TfsUserName { get; }
        string TfsPassword { get; }
        string WorkItemRegex { get; }
        bool DebugMode { get; }
        IProxyConnectionSettings ProxySettings { get; }
    }
}
