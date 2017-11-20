using System;
using Newtonsoft.Json;

namespace Dashquoia.Api.Models
{
    [JsonObject]
    public class HistoryResult
    {
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("status")]
        public StatusType Status { get; set; }
    }
}