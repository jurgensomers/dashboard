using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashquoia.Api.Configuration;
using Dashquoia.Api.Models;

namespace Dashquoia.Api.Managers.Handlers
{
    public class GetProcessesHandler : IHandler
    {
        public GetProcessesHandler()
        {
            _serviceAgent = new ServiceAgent();
        }

        private readonly ServiceAgent _serviceAgent;

        public void Handle(Setting test, GenericResults genericResults)
        {
            try
            {
                var list = GetProcesses(test);
                if (list != null)
                {
                    foreach (var processInformation in list)
                    {
                        var result = test.AsGenericResult();
                        result.Name = processInformation.Identifier;
                        result.Status = processInformation.State == "Idle" | processInformation.State == "Running"
                            ? StatusType.Up
                            : StatusType.Down;
                        genericResults.Results.Add(result);
                    }
                }
            }
            catch 
            {
                //Log.Error($"Error while trying to execute on {test.Environment}-{test.Service}");
            }
        }

        private List<dynamic> GetProcesses(Setting setting)
        {
            //Log.Information($"GetProcesses on {setting.Environment} - {setting.Name}");
            var converter = new GetProcessResponseConverter();
            var response = _serviceAgent.Send($"<GetProcesses xmlns=\"{setting.NameSpace}\"></GetProcesses>", setting, converter.Convert);
            return response;
        }

    }
}
