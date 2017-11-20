using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Configuration;
using System.Web.Http;
using Dashquoia.Api.Configuration;
using Dashquoia.Api.Managers;

namespace Dashquoia.Api.Controllers
{
    [RoutePrefix("services")]
    public class ServiceController : ApiController
    {
        [Route("all"), HttpGet]
        public IHttpActionResult GetAll()
        {
            var services = SettingsManager.GetServices().Select(x => x.Name).Distinct();
            return Ok(services);
        }

        [Route("getconfig"), HttpGet]
        public IHttpActionResult GetConfig()
        {
            return Ok(SettingsManager.GetServices());
        }

        [Route("getbindings"), HttpGet]
        public IHttpActionResult GetBindings([FromUri] string bindingType)
        {
            var serviceModel = ServiceModelSectionGroup.GetSectionGroup(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None));
            IList<string> result = serviceModel.Bindings[bindingType].ConfiguredBindings.Select(x => x.Name).ToList();

            return Ok(result);
        }

        [Route("getbindingtypes"), HttpGet]
        public IHttpActionResult GetBindingTypes()
        {
            var serviceModel = ServiceModelSectionGroup.GetSectionGroup(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None));
            var types = serviceModel.Bindings.BindingCollections.Select(x => x.BindingName);

            return Ok(types);
        }

        [Route("setConfig"), HttpPut]

        public void SetConfig([FromBody] IList<ServiceConfig> services)
        {
            SettingsManager.SetServices(services);
        }
    }
}