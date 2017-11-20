using System.Collections.Generic;

namespace Dashquoia.Api.Models
{
    public class Owner
    {
        public Owner()
        {
            Groups = new List<OwnerGroup>();
        }

        public string Name { get; set; }
        public IList<OwnerGroup> Groups { get; set; }
        public TfsBuildResults TfsResults { get; set; }
    }
}