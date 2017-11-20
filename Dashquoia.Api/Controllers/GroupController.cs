using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Dashquoia.Api.Configuration;
using Dashquoia.Api.Managers;

namespace Dashquoia.Api.Controllers
{
    [RoutePrefix("groups")]
    public class GroupController : ApiController
    {
        [Route("all"), HttpGet]
        public IHttpActionResult All()
        {
            var settings = SettingsManager.Get();
            var groups = settings.Select(x => x.Group).Distinct();
            return Ok(groups);
        }


        [Route("getConfig"), HttpGet]
        public IHttpActionResult GetConfig()
        {
            var groups = SettingsManager.GetGroups();
            return Ok(groups);
        }

        [Route("setConfig"), HttpPut]

        public void SetConfig([FromBody] IList<GroupConfig> groups)
        {
            SettingsManager.SetGroups(groups);
        }
    }
}