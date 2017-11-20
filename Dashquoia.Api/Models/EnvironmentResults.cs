using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashquoia.Api.Models
{
    public class EnvironmentResults
    {
        public EnvironmentResults()
        {
            Environments = new List<Environment>();
        }
        

        public IList<Environment> Environments { get; set; }
        public DateTime Date { get; set; }
    }
}
