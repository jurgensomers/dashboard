using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashquoia.Api.Models
{
    public class GenericResult
    {
        public string Environment { get; set; }

        public string Owner { get; set; }

        public string Group { get; set; }

        public TestType Type { get; set; }

        public string Name { get; set; }
        public StatusType Status { get; set; }
    }
}
