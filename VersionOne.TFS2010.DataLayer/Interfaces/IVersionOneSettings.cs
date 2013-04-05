using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VersionOne.TFS2010.DataLayer.Interfaces
{
    public interface IVersionOneSettings
    {
        bool Integrated { get; set; }
        string Path { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        IProxyConnectionSettings ProxySettings { get; set; }
    }
}
