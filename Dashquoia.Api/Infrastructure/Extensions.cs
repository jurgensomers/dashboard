using System;
using System.Collections.Generic;
using System.Linq;
using Dashquoia.Api.Models;

namespace Dashquoia.Api.Infrastructure
{
    public static class Extensions
    {
        public static  GenericResults Filter(this GenericResults results, string owner = null, string group = null,string environment = null, string service = null)
        {
            try
            {

                if (results != null && results.Results != null)
                {
                    var filteredResults = results.Results.Where(
                            x => x != null && (x.Owner == owner || string.IsNullOrWhiteSpace(owner)) &&
                                 (x.Group == group || string.IsNullOrWhiteSpace(group)) &&
                                 (x.Environment == environment || string.IsNullOrWhiteSpace(environment)) &&
                                 (x.Name == service || string.IsNullOrWhiteSpace(service)))
                        .ToList();

                    return new GenericResults
                    {
                        Date = results?.Date ?? DateTime.Now,
                        Results = filteredResults
                    };
                }
                return results;
            }
            catch
            {
                return null;
            }
        }

        public static TfsBuildResults Filter(this TfsBuildResults results, string owner = null, string build = null)
        {
            try
            {

                if (results != null && results.Results != null)
                {
                    var filteredResults = results.Results.Where(
                            x => x != null && (x.Owner == owner || string.IsNullOrWhiteSpace(owner)) &&
                                 (x.Name == build || string.IsNullOrWhiteSpace(build)) )
                        .ToList();

                    return new TfsBuildResults
                    {
                        Date = results?.Date ?? DateTime.Now,
                        Results = filteredResults
                    };
                }
                return results;
            }
            catch
            {
                return null;
            }
        }
    }
}