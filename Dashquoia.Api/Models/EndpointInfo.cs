using Newtonsoft.Json;

namespace Dashquoia.Api.Models
{
    [JsonObject]
    public class EndpointInfo
    {
        public string Name { get; set; }
        public string Environment { get; set; }
        public TestType Type { get; set; }
        public string Endpoint { get; set; }
    }
}