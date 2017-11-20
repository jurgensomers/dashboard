using System;
using System.Collections.Generic;

namespace Dashquoia.Api.Models
{
    public class GroupResults
    {
        public GroupResults()
        {
            Groups = new List<Group>();
            Environments = new List<GroupResult>();
        }

        public IList<Group> Groups { get; set; }
        public IList<GroupResult> Environments { get; set; }
        public DateTime Date { get; set; }
    }
}