using System;
using System.Web;
using System.Web.Http;
using VersionOneTFSServer.App_Start;

namespace VersionOneTFSServer
{
    public class Global : HttpApplication
    {
        protected void Application_Start()
        {
            WebApiConfig.Register(GlobalConfiguration.Configuration);
        }
    }
}