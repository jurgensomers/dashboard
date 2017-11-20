using Newtonsoft.Json;

namespace Dashquoia.Api.Models
{
    [JsonObject]
    public class TfsBuildStatus
    {
        [JsonProperty("owner")]
        public string Owner { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}