using System;
using System.Collections.Generic;

namespace Dashquoia.Api.Models
{
    public class TfsBuildResults
    {
        public TfsBuildResults()
        {
            Results = new List<TfsBuildStatus>();
        }

        public DateTime Date { get; set; }
        public IList<TfsBuildStatus> Results { get; internal set; }
    }
}