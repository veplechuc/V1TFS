using System;
using System.Net;
using Microsoft.TeamFoundation.Client;
using VersionOne.ServiceHost.Core.Configuration;
using VersionOneTFS2010.DataLayer.Providers;

namespace VersionOneTFSServer
{
    /// <summary>
    /// Summary description for Utils
    /// </summary>
    public class Utils
    {
        public static TfsTeamProjectCollection ConnectToTfs()
        {

            var config = new ConfigurationProvider();

            var url = config.TfsUrl;
            var user = config.TfsUserName;
            var password = config.TfsPassword;

            var domain = string.Empty;
            var pos = user.IndexOf('\\');

            if (pos >= 0)
            {
                domain = user.Substring(0, pos);
                user = user.Substring(pos + 1);
            }

            var creds = new NetworkCredential(user, password, domain);
            var tfsServer = new TfsTeamProjectCollection(url, creds);
            tfsServer.Authenticate();
            return tfsServer;
        }

        /// <summary>
        /// Yuck.  Why?
        /// </summary>
        /// <returns></returns>
        public static VersionOne.TFS2010.DataLayer.VersionOneSettings GetV1Settings()
        {

            var config = new ConfigurationProvider();

            return new VersionOne.TFS2010.DataLayer.VersionOneSettings 
                {
                    Path = config.VersionOneUrl.ToString(),
                    Username = config.VersionOneUserName,
                    Password = config.VersionOnePassword,
                    Integrated = config.IsWindowsIntegratedSecurity,
                    ProxySettings = config.ProxySettings
                };
        }


    }
}