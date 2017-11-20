using System.Collections.Generic;

namespace Dashquoia.Api.Models
{
    public class OwnerGroup
    {
        public OwnerGroup()
        {
            Services = new List<OwnerService>();
        }

        public string Name { get; set; }
        public IList<OwnerService> Services { get; set; }
    }
}