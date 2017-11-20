using System.Linq;
using System.Web.Http;
using Dashquoia.Api.Managers;

namespace Dashquoia.Api.Controllers
{
    [RoutePrefix("owners")]
    public class OwnerController : ApiController
    {
        [Route("all"), HttpGet]
        public IHttpActionResult All()
        {
            var settings = SettingsManager.Get();
            var owners = settings.Select(x => x.Owner).Distinct();
            return Ok(owners);
        }
        
    }
}