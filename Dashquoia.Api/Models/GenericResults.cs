using System;
using System.Collections.Generic;

namespace Dashquoia.Api.Models
{
    public class GenericResults
    {
        public GenericResults()
        {
            Results = new List<GenericResult>();
        }

        public DateTime Date { get; set; }
        public IList<GenericResult> Results { get; internal set; }
    }
}