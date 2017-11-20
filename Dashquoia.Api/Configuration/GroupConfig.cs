using Newtonsoft.Json;

namespace Dashquoia.Api.Configuration
{
    [JsonObject("group")]
    public class GroupConfig
    {
        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("services")]
        public string[] Services { get; set; }
    }
}