using System;
using System.Net;
using Microsoft.TeamFoundation.Client;
using VersionOne.TFS2010.DataLayer;

namespace VersionOneTFSServerConfig {
    /// <summary>
    /// Summary description for Utils
    /// </summary>
    public class Utils
    {
        public static TfsTeamProjectCollection ConnectToTFS()
        {
            var url = RegistryProcessor.GetString(RegistryProcessor.TfsUrlParameter, string.Empty);
            var user = RegistryProcessor.GetString(RegistryProcessor.TfsUsernameParameter, string.Empty);
            var password = RegistryProcessor.GetPassword(RegistryProcessor.TfsPasswordParameter, string.Empty);

            var domain = string.Empty;
            var pos = user.IndexOf('\\');
            
            if (pos >= 0)
            {
                domain = user.Substring(0, pos);
                user = user.Substring(pos + 1);
            }
            
            var creds = new NetworkCredential(user, password, domain);
            var tfsServer = new TfsTeamProjectCollection(new Uri(url), creds);
            tfsServer.Authenticate();
            return tfsServer;
        }
    }
}
