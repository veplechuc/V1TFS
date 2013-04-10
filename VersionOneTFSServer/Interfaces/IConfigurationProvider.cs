using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using VersionOne.TFS2010.DataLayer.Interfaces;

namespace VersionOneTFSServer.Interfaces
{
    public interface IConfigurationProvider
    {
        bool Integrated { get; }
        Url V1Url { get; }
        string UserName { get; }
        string Password { get; }
        IProxyConnectionSettings ProxySettings { get; }
    }
}
