using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using Integrations.Core.DTO;
using Newtonsoft.Json;

namespace VersionOneTFSServer.ModelBinders
{
    public class TfsServerConfigurationModelBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            var json = actionContext.Request.Content.ReadAsStringAsync().Result;
            bindingContext.Model = JsonConvert.DeserializeObject<TfsServerConfiguration>(json);
            return true;
        }
    }
}