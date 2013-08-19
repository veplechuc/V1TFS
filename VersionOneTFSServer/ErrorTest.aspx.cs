using System;
using System.Web.UI;

namespace VersionOneTFSServer
{
    public partial class ErrorTest : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            throw new Exception("Test exception message.");
        }
    }
}