using Newtonsoft.Json;

namespace Dashquoia.Api.Configuration
{
    public class Settings
    {
        [JsonProperty("hosts")]
        public HostConfig[] Hosts { get; set; }

        [JsonProperty("services")]
        public ServiceConfig[] Services { get; set; }

        [JsonProperty("groups")]
        public GroupConfig[] Groups { get; set; }
    }
}