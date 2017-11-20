using System.Collections.Generic;

namespace Dashquoia.Api.Models
{
    public class GroupService
    {
        public GroupService()
        {
            Results = new List<GroupResult>();
        }

        public string Name { get; set; }
        public IList<GroupResult> Results { get; set; }
    }
}
