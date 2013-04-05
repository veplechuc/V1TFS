using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VersionOne.TFS2010.DataLayer.Interfaces
{
    public interface IProxyConnectionSettings
    {
        bool UseProxy { get; set; }
        string Url { get; set; }
        string Domain { get; set; }
        string Username { get; set; }
        string Password { get; set; }
    }
}
