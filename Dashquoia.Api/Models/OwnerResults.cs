using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Dashquoia.Api.Models
{
    public class OwnerResults
    {
        public OwnerResults()
        {
            Owners = new List<Owner>();
            Environments = new List<OwnerResult>();
        }

        public Owner this[string name]
        {
            get { return Owners?.FirstOrDefault(x => x.Name == name); }
        }

        public IList<Owner> Owners { get; set; }
        public IList<OwnerResult> Environments { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }
    }
}