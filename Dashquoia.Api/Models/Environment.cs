using System.Collections.Generic;

namespace Dashquoia.Api.Models
{
    public class Environment
    {
        public Environment()
        {
            Groups = new List<EnvironmentGroup>();
        }

        public string Name { get; set; }
        public IList<EnvironmentGroup> Groups { get; set; }
    }
}