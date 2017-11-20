using Dashquoia.Api.Configuration;
using Dashquoia.Api.Models;

namespace Dashquoia.Api.Managers.Handlers
{
    public interface IHandler
    {
        void Handle(Setting test, GenericResults genericResults);
    }
}