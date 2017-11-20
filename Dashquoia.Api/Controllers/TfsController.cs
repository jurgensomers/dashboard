using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Dashquoia.Api.Configuration;
using Dashquoia.Api.Managers;

namespace Dashquoia.Api.Controllers
{
    [RoutePrefix("tfs")]
    public class TfsController : ApiController
    {
        public TfsController()
        {
            _tfsManager = new TfsManager();
        }

        private readonly TfsManager _tfsManager;

        [Route("all"), HttpGet]
        public IHttpActionResult GetAll()
        {
            var results = _tfsManager.Get();
            return Ok(results);
        }

        [Route("owner"), HttpGet]
        public IHttpActionResult GetByOwner([FromUri] string owner)
        {
            var results = _tfsManager.Get(owner:owner);
            return Ok(results);
        }

        [Route("build"), HttpGet]
        public IHttpActionResult GetByBuild([FromUri] string build)
        {
            var results = _tfsManager.Get(build: build);
            return Ok(results);
        }

        [Route("getConfig"), HttpGet]
        public IHttpActionResult GetConfig()
        {
            var groups = SettingsManager.GetTfsBuilds();
            return Ok(groups);
        }

        [Route("setConfig"), HttpPut]

        public void SetConfig([FromBody] IList<TfsBuild> groups)
        {
            SettingsManager.SetTfsBuilds(groups);
        }

        [Route("definitions"), HttpGet]

        public IHttpActionResult GetBuildDefinitions()
        {
            var results = _tfsManager.GetBuildDefinitions();
            return Ok(results);
        }
    }
}
