using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Dashquoia.Api.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TestType
    {
        IsAlive,
        GetProcesses
    }
}