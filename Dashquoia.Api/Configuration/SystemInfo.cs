using Newtonsoft.Json;

namespace Dashquoia.Api.Configuration
{
    [JsonObject("group")]
    public class SystemInfo
    {
        public string ServerCacheSleep { get; set; }
        public string ServerCacheRefreshInterval { get; set; }
        public string ServerAddress { get; set; }
    }
}