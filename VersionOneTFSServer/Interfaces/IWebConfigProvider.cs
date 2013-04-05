using System.Collections.Generic;

namespace VersionOneTFSServer.Interfaces
{
    public interface IWebConfigProvider
    {
        Dictionary<string, string> GetAppSettings(params string[] keyNames);
    }
}