using Newtonsoft.Json;

namespace Dashquoia.Api.Configuration
{
    [JsonObject("host")]
    public class HostConfig
    {
        [JsonProperty("environment")]
        public string Environment { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("identity")]
        public string Identity { get; set; }
    }
}