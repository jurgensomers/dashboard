using Dashquoia.Api.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Dashquoia.Api.Configuration
{
    public class Setting
    {
        [JsonProperty("environment")]
        public string Environment { get; set; }

        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("group")]
        public string Group { get; set; }

        [JsonProperty("type")]
        public TestType Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("service")]
        public string Service { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("namespace")]
        public string NameSpace { get; set; }

        [JsonProperty("bindingtype")]
        public string BindingType { get; set; }

        [JsonProperty("bindingname")]
        public string BindingName { get; set; }

        [JsonProperty("identity")]
        public string Identity { get; set; }
    }
}