using System.Collections.Generic;

namespace Dashquoia.Api.Models
{
    public class Group
    {
        public Group()
        {
            Services = new List<GroupService>();
        }

        public string Name { get; set; }
        public IList<GroupService> Services { get; set; }
    }
}