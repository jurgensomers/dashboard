using System.Collections.Generic;

namespace Dashquoia.Api.Models
{
    public class EnvironmentGroup
    {
        public EnvironmentGroup()
        {
            Services = new List<EnvironmentService>();
        }

        public string Name { get; set; }
        public IList<EnvironmentService> Services { get; set; }
    }
}