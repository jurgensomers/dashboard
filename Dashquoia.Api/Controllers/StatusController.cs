using System.Web.Http;
using Dashquoia.Api.Converters;
using Dashquoia.Api.Managers;

namespace Dashquoia.Host.Controllers
{
    [RoutePrefix("status")]
    public class StatusController : ApiController
    {
        public StatusController()
        {
            _serviceManager = new ServiceManager(); 
        }

        private readonly ServiceManager _serviceManager; 

        [Route("all")]
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var all = _serviceManager.Get();
            return Ok(all);
        }

        [Route("owners")]
        [HttpGet]
        public IHttpActionResult GetByOwners()
        {
            var all = _serviceManager.Get();
            var result = all.AsOwnerResults();
            return Ok(result);
        }

        [Route("owner/{key}"), HttpGet]
        public IHttpActionResult ByOwner(string key)
        {
            var all = _serviceManager.Get(owner: key);
            var result = all.AsOwnerResults(); 
            return Ok(result);
        }

        [Route("groups")]
        [HttpGet]
        public IHttpActionResult GetByGroups()
        {
            var all = _serviceManager.Get();
            var result = all.AsGroupResults();
            return Ok(result);
        }

        [Route("group/{key}"), HttpGet]
        public IHttpActionResult ByGroup(string key)
        {
            var all =  _serviceManager.Get(group: key);
            var result = all.AsGroupResults();
            return Ok(result);
        }

        [Route("environments"), HttpGet]
        public IHttpActionResult GetByEnvironments()
        {
            var all = _serviceManager.Get();
            var result = all.AsEnvironmentResults();
            return Ok(result);
        }

        [Route("environment/{key}"), HttpGet]
        public IHttpActionResult ByEnvironment(string key)
        {
            var all = _serviceManager.Get(environment: key);
            var result = all.AsEnvironmentResults();
            return Ok(result);
        }
    }
}