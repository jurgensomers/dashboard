using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashquoia.Api.Models;

namespace Dashquoia.Api.Converters
{
    public static class GroupResultConverter
    {
        public static GroupResults AsGroupResults(this GenericResults results)
        {
            var groupResults = new GroupResults { Date = results.Date };
            foreach (var genericResult in results.Results)
            {
                groupResults.GetOrAddGroup(genericResult.Group)
                    .GetOrAddService(genericResult.Name)
                    .GetOrAddResult(genericResult.Environment, genericResult.Status);
            }
            groupResults.SetEnvironmentSummary();
            return groupResults;
        }

        private static Group GetOrAddGroup(this GroupResults groupResults, string name)
        {
            Group group = groupResults.Groups.FirstOrDefault(g => g.Name == name);
            if (group == null)
            {
                group = new Group { Name = name };
                groupResults.Groups.Add(group);
            }
            return group;
        }

        private static GroupService GetOrAddService(this Group group, string name)
        {
            GroupService service = group.Services.FirstOrDefault(s => s.Name == name);
            if (service == null)
            {
                service = new GroupService { Name = name };
                group.Services.Add(service);
            }
            return service;
        }

        private static GroupResult GetOrAddResult(this GroupService service, string environment, StatusType status)
        {
            GroupResult result = service.Results.FirstOrDefault(r => r.Environment == environment);
            if (result == null)
            {
                result = new GroupResult { Environment = environment, Status = status };
                service.Results.Add(result);
            }
            return result;
        }

        private static void SetEnvironmentSummary(this GroupResults result)
        {
            var all = result.Groups.SelectMany(g => g.Services.SelectMany(s => s.Results));
            var groupedByEnvironment = all.GroupBy(r => r.Environment);

            foreach (var e in groupedByEnvironment)
            {
                if (e.Any(x => x.Status == StatusType.Down))
                {
                    result.Environments.Add(
                        new GroupResult
                        {
                            Environment = e.Key,
                            Status = StatusType.Down
                        });
                }
                else
                {
                    result.Environments.Add(
                        new GroupResult
                        {
                            Environment = e.Key,
                            Status = StatusType.Up
                        });
                }
            }

            Cleanup(result);
        }

        /// <summary>
        /// Adds missing information in per environment in case results are missing due to exceptions
        /// </summary>
        /// <param name="result"></param>
        private static void Cleanup(GroupResults result)
        {
            var allEnvironments = result.Groups
                .SelectMany(g => g.Services.SelectMany(s => s.Results.Select(r => r.Environment))).Distinct();
            foreach (var group in result.Groups)
            {
                foreach (var service in group.Services)
                {
                    foreach (var e in allEnvironments)
                    {
                        if (service.Results.All(r => r.Environment != e))
                        {
                            service.Results.Add(new GroupResult() {Environment = e, Status = StatusType.Unknown});
                        }
                    }

                }
            }
        }
    }
}
