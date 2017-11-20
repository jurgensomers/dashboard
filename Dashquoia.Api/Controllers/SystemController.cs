using System.Configuration;
using System.Web.Http;
using Dashquoia.Api.Assets;
using Dashquoia.Api.Configuration;
using Dashquoia.Api.Converters;
using Dashquoia.Api.Managers;

namespace Dashquoia.Host.Controllers
{
    [RoutePrefix("system")]
    public class SystemController : ApiController
    {
        public SystemController()
        {
        }



        [Route("info"), HttpGet]
        public IHttpActionResult GetSystemInfo()
        {
            var systemInfo = new SystemInfo
            {
                ServerAddress = AppSettings.ServerAddress,
                ServerCacheRefreshInterval = $"{AppSettings.RefreshRate / 60000} minute(s)",
                ServerCacheSleep = $"{AppSettings.SleepFrom} until {AppSettings.SleepUntil}"
            };

            return Ok(systemInfo);
        }

        [Route("endpoints")]
        public IHttpActionResult GetEndpoints()
        {
            return Ok(SettingsManager.GetEndpoints());
        }

    }
}