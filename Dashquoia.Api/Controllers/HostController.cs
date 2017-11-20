using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Dashquoia.Api.Configuration;
using Dashquoia.Api.Managers;

namespace Dashquoia.Api.Controllers
{
    [RoutePrefix("hosts")]
    public class HostController : ApiController
    {
        [Route("all"), HttpGet]
        public IHttpActionResult GetAll()
        {
            var services = SettingsManager.GetHosts().Select(x => x.Name).Distinct();
            return Ok(services);
        }

        [Route("getconfig"), HttpGet]
        public IHttpActionResult GetConfig()
        {
            var hosts = SettingsManager.GetHosts();
            return Ok(hosts);
        }

        [Route("setConfig"),HttpPut]

        public void SetConfig([FromBody] IList<HostConfig> hosts)
        {
            SettingsManager.SetHosts(hosts);
        }
    }
}