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
        Uri BaseListenerUrl { get; }
        Uri TfsListenerUrl { get; }
        Uri ConfigurationUrl { get; }
        string TfsUserName { get; }
        string TfsPassword { get; }
        string TfsWorkItemRegex { get; }
        bool DebugMode { get; }
        IProxyConnectionSettings ProxySettings { get; }
        void ClearAllSettings();
    }
}
