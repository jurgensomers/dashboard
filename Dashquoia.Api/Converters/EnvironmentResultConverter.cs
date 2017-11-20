
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashquoia.Api.Models;

namespace Dashquoia.Api.Converters
{
    public static class EnvironmentResultConverter
    {
        public static EnvironmentResults AsEnvironmentResults(this GenericResults results)
        {
            var groupResults = new EnvironmentResults { Date = results.Date };
            foreach (var genericResult in results.Results)
            {
                groupResults.GetOrAddEnvironment(genericResult.Environment)
                    .GetOrAddGroup(genericResult.Group)
                    .GetOrAddService(genericResult.Name, genericResult.Status);
            }
            return groupResults;
        }

        private static Environment GetOrAddEnvironment(this EnvironmentResults environmentResults, string name)
        {
            Environment environment = environmentResults.Environments.FirstOrDefault(g => g.Name == name);
            if (environment == null)
            {
                environment = new Environment { Name = name };
                environmentResults.Environments.Add(environment);
            }
            return environment;
        }

        private static EnvironmentGroup GetOrAddGroup(this Environment owner, string name)
        {
            EnvironmentGroup group = owner.Groups.FirstOrDefault(g => g.Name == name);
            if (group == null)
            {
                group = new EnvironmentGroup { Name = name };
                owner.Groups.Add(group);
            }
            return group;
        }

        private static EnvironmentService GetOrAddService(this EnvironmentGroup group, string name, StatusType status)
        {
            EnvironmentService service = group.Services.FirstOrDefault(s => s.Name == name);
            if (service == null)
            {
                service = new EnvironmentService { Name = name, Status =  status};
                group.Services.Add(service);
            }
            return service;
        }
    }
}
