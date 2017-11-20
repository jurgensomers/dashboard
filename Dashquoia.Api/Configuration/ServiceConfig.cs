using Dashquoia.Api.Models;
using Newtonsoft.Json;

namespace Dashquoia.Api.Configuration
{
    [JsonObject("service")]
    public class ServiceConfig
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("namespace")]
        public string NameSpace { get; set; }

        [JsonProperty("bindingtype")]
        public string BindingType { get; set; }

        [JsonProperty("bindingname")]
        public string BindingName { get; set; }

        [JsonProperty("type")]
        public TestType Type { get; set; }

        [JsonProperty("host")]
        public string Host { get; set; }

    }
}