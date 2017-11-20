using System.Linq;
using Dashquoia.Api.Models;

namespace Dashquoia.Api.Converters
{
    public static class OwnerResultConverter
    {
        public static OwnerResults AsOwnerResults(this GenericResults results)
        {
            var ownerResult = new OwnerResults { Date = results.Date };
            foreach (var genericResult in results.Results)
            {
                ownerResult.GetOrAddOwner(genericResult.Owner)
                    .GetOrAddGroup(genericResult.Group)
                    .GetOrAddService(genericResult.Name)
                    .GetOrAddResult(genericResult.Environment, genericResult.Status);
            }
            ownerResult.SetEnvironmentSummary();
            return ownerResult;
        }

        private static Owner GetOrAddOwner(this OwnerResults information, string name)
        {
            Owner owner = information.Owners.FirstOrDefault(o => o.Name == name);
            if (owner == null)
            {
                owner = new Owner { Name = name };
                information.Owners.Add(owner);
            }

            return owner;
        }

        private static OwnerGroup GetOrAddGroup(this Owner owner, string name)
        {
            OwnerGroup group = owner.Groups.FirstOrDefault(g => g.Name == name);
            if (group == null)
            {
                group = new OwnerGroup { Name = name };
                owner.Groups.Add(group);
            }
            return group;
        }

        private static OwnerService GetOrAddService(this OwnerGroup group, string name)
        {
            OwnerService service = group.Services.FirstOrDefault(s => s.Name == name);
            if (service == null)
            {
                service = new OwnerService { Name = name };
                group.Services.Add(service);
            }
            return service;
        }

        private static OwnerResult GetOrAddResult(this OwnerService service, string environment, StatusType status)
        {
            OwnerResult result = service.Results.FirstOrDefault(r => r.Environment == environment);
            if (result == null)
            {
                result = new OwnerResult { Environment = environment, Status = status};
                service.Results.Add(result);
            }
            return result;
        }

        private static void SetEnvironmentSummary(this OwnerResults result)
        {
            var all = result.Owners.SelectMany(o => o.Groups.SelectMany(g => g.Services.SelectMany(s => s.Results)));
            var groupedByEnvironment = all.GroupBy(r => r.Environment);

            foreach (var e in groupedByEnvironment)
            {
                if (e.Any(x => x.Status == StatusType.Down))
                {
                    result.Environments.Add(
                        new OwnerResult
                        {
                            Environment = e.Key,
                            Status = StatusType.Down
                        });
                }
                else
                {
                    result.Environments.Add(
                        new OwnerResult
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
        private static void Cleanup(OwnerResults result)
        {
            var allEnvironments = result.Owners.SelectMany(o => o.Groups.SelectMany(g => g.Services.SelectMany(s => s.Results.Select(r => r.Environment)))).Distinct();
            foreach (var owner in result.Owners)
            {
                foreach (var group in owner.Groups)
                {
                    foreach (var service in group.Services)
                    {
                        foreach (var e in allEnvironments)
                        {
                            if (service.Results.All(r => r.Environment != e))
                            {
                                service.Results.Add(new OwnerResult { Environment = e, Status = StatusType.Unknown });
                            }
                        }
                    }
                }
            }
        }
    }
}