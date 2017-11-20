using System.Linq;
using System.Web.Http;
using Dashquoia.Api.Managers;

namespace Dashquoia.Api.Controllers
{
    [RoutePrefix("environments")]
    public class EnvironmentController : ApiController
    {
        [Route("all"), HttpGet]
        public IHttpActionResult All()
        {
            var settings = SettingsManager.Get();
            var owners = settings.Select(x => x.Environment).Distinct();
            return Ok(owners);
        }
    }
}