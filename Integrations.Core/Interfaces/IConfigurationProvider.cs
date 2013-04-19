using System;

namespace Integrations.Core.Interfaces
{
    public interface IConfigurationProvider
    {
        bool IsWindowsIntegratedSecurity { get; }
        Uri VersionOneUrl { get; }
        string UserName { get; }
        string Password { get; }
        IProxyConnectionSettings ProxySettings { get; }
    }
}
