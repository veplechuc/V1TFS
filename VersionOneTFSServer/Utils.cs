using System;
using System.Net;
using Microsoft.TeamFoundation.Client;
using VersionOne.TFS2010.DataLayer;

namespace VersionOneTFSServer
{
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

        public static VersionOneSettings GetV1Settings()
        {
            return new VersionOneSettings 
                {
                    Path = RegistryProcessor.GetString(RegistryProcessor.V1UrlParameter, string.Empty),
                    Username = RegistryProcessor.GetString(RegistryProcessor.V1UsernameParameter, string.Empty),
                    Password = RegistryProcessor.GetPassword(RegistryProcessor.V1PasswordParameter, string.Empty),
                    Integrated = RegistryProcessor.GetBool(RegistryProcessor.V1WindowsAuthParameter, false),
                    ProxySettings = GetProxySettings()
                };
        }

        private static ProxyConnectionSettings GetProxySettings() 
        {
            if(!RegistryProcessor.GetBool(RegistryProcessor.V1UseProxyParameter, false)) 
            {
                return null;
            }

            return new ProxyConnectionSettings 
            { 
                ProxyIsEnabled = RegistryProcessor.GetBool(RegistryProcessor.V1UseProxyParameter, false), 
                Url = new Uri(RegistryProcessor.GetString(RegistryProcessor.V1ProxyUrlParameter, string.Empty)), 
                Username = RegistryProcessor.GetString(RegistryProcessor.V1ProxyUsernameParameter, string.Empty), 
                Password = RegistryProcessor.GetString(RegistryProcessor.V1ProxyPasswordParameter, string.Empty),
                Domain = RegistryProcessor.GetString(RegistryProcessor.V1ProxyDomainParameter, string.Empty)
            };            
        }
    }
}