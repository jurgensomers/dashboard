using Dashquoia.Api.Configuration;
using Dashquoia.Api.Models;

namespace Dashquoia.Api.Managers
{
    public static class ResultFormatter
    {
        public static GenericResult AsGenericResult(this Setting setting)
        {
            return new GenericResult
            {
                Owner = setting.Owner,
                Environment = setting.Environment,
                Group = setting.Group,
                Type = setting.Type,
                Status = StatusType.Unknown,
                Name = setting.Name
            };
        }
    }
}