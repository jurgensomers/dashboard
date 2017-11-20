using System.Collections.Generic;

namespace Dashquoia.Api.Models
{
    public class OwnerService
    {
        public OwnerService()
        {
            Results = new List<OwnerResult>();
        }

        public string Name { get; set; }
        public IList<OwnerResult> Results { get; set; }
    }
}
