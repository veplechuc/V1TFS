using System.Web;
using System.Web.Http;
using System.Web.Routing;
using VersionOneTFSServer.App_Start;

namespace VersionOneTFSServer
{
    public class Global : HttpApplication
    {
        protected void Application_Start()
        {
            WebApiConfig.RegisterRoutesIgnores(RouteTable.Routes);
            WebApiConfig.Register(GlobalConfiguration.Configuration);
        }
    }
}