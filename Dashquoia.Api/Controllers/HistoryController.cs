using System;
using System.Globalization;
using System.Web.Http;
using Dashquoia.Api.Converters;
using Dashquoia.Api.Managers;

namespace Dashquoia.Api.Controllers
{
    [RoutePrefix("history")]
    public class HistoryController : ApiController
    {
        public HistoryController()
        {
            _historyManager = new HistoryManager();
        }


        private readonly HistoryManager _historyManager;

        [Route("all")]
        [HttpGet]
        public IHttpActionResult GetAll([FromUri] string date)
        {
            var d = DateTime.ParseExact(date, "yyyy-MM-dd HHmmss", CultureInfo.InvariantCulture);
            var all = _historyManager.Load(d);
            return Ok(all);
        }

        [Route("list"), HttpGet]
        public IHttpActionResult List()
        {
            var all = _historyManager.List();
            return Ok(all);
        }

        [Route("owners")]
        [HttpGet]
        public IHttpActionResult GetByOwners([FromUri] string date)
        {
            var all =  _historyManager.Get(date);
            var result = all.AsOwnerResults();
            return Ok(result);
        }

        [Route("owner/{key}"), HttpGet]
        public IHttpActionResult ByOwner(string key, [FromUri] string date)
        {
            var all = _historyManager.Get(date, owner: key);
            var result = all.AsOwnerResults();
            return Ok(result);
        }

        [Route("groups")]
        [HttpGet]
        public IHttpActionResult GetByGroups([FromUri] string date)
        {
            var all = _historyManager.Get(date);
            var result = all.AsGroupResults();
            return Ok(result);
        }

        [Route("group/{key}"), HttpGet]
        public IHttpActionResult ByGroup(string key, [FromUri] string date)
        {
            var all =  _historyManager.Get(date, group: key);
            var result = all.AsGroupResults();
            return Ok(result);
        }

        [Route("environments"), HttpGet]
        public IHttpActionResult GetByEnvironments([FromUri] string date = null)
        {
            var all =  _historyManager.Get(date);
            var result = all.AsEnvironmentResults();
            return Ok(result);
        }

        [Route("environment/{key}"), HttpGet]
        public IHttpActionResult ByEnvironment(string key, [FromUri] string date = null)
        {
            var all = _historyManager.Get(date, environment: key);
            var result = all.AsEnvironmentResults();
            return Ok(result);
        }

        [Route("service"), HttpGet]
        public IHttpActionResult ByService([FromUri]  string name, [FromUri]  string environment, [FromUri] string date = null)
        {
            var result = _historyManager.Service(name, environment); 
            return Ok(result);
        }
    }
}