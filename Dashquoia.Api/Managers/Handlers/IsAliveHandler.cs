using Dashquoia.Api.Configuration;
using Dashquoia.Api.Converters;
using Dashquoia.Api.Models;

namespace Dashquoia.Api.Managers.Handlers
{
    public class IsAliveHandler : IHandler
    {
        public IsAliveHandler()
        {
            _serviceAgent = new ServiceAgent();

        }

        private readonly ServiceAgent _serviceAgent;

        public void Handle(Setting test, GenericResults genericResults)
        {
            genericResults.Results.Add(IsAlive(test));
        }

        private GenericResult IsAlive(Setting setting)
        {
            var result = setting.AsGenericResult();
            var converter = new IsAliveResponseConverter();
            result.Status = _serviceAgent.Send($"<IsAlive xmlns=\"{setting.NameSpace}\"></IsAlive>", setting, converter.Convert);
            return result;
        }
    }
}