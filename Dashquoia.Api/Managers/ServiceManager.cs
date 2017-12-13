using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dashquoia.Api.Configuration;
using Dashquoia.Api.Infrastructure;
using Dashquoia.Api.Managers.Handlers;
using Dashquoia.Api.Models;
using Serilog;

namespace Dashquoia.Api.Managers
{
    public class ServiceManager
    {
        private readonly CacheManager _cacheManager;
        private readonly HistoryManager _historyManager;
        private readonly IDictionary<TestType, IHandler> _handlers;

        public ServiceManager()
        {
            _cacheManager = new CacheManager();
            _historyManager = new HistoryManager();
            _handlers = new Dictionary<TestType, IHandler>()
            {
                {TestType.IsAlive, new IsAliveHandler()},
                {TestType.GetProcesses, new GetProcessesHandler()}
            };

            Observer.StartObservingServices(Load);
        }

        public GenericResults Get(string owner = null, string group = null, string environment = null)
        {
            try
            {
                var locked = _cacheManager.GetLockServices();
                if (locked != null)
                {
                    Log.Logger.Information($"Waiting...");
                    Task.WaitAll(locked);
                    Log.Logger.Information($"...and done");
                }

                var results = _cacheManager.GetServices() ?? Load();

                // filter the results
                return results.Filter(owner, group, environment);
            }
            catch (Exception e)
            {
                _cacheManager.ClearServices();
                Log.Logger.Fatal(e, String.Empty);
                return new GenericResults();
            }
        }

        private GenericResults Load()
        {
            try
            {
                //_cacheManager.Lock();
                var settings = SettingsManager.Get();
                var task = Task.Factory.StartNew(() => LoadAsync(settings));
                _cacheManager.LockServices(task);
                var result = task.GetAwaiter().GetResult();
                _historyManager.Store(result);
                _cacheManager.ReleaseServices();
                _cacheManager.Store(result);
                return result;
            }
            catch (Exception exc)
            {
                Log.Logger.Error(exc, "An error has occured");
            }
            return null;
        }

        private static int _break = 2;

        private GenericResults LoadAsync(IList<Setting> settings)
        {
            try
            {
                Log.Logger.Information("Getting states...");
                var result = new GenericResults() { Date = DateTime.Now };
                Parallel.ForEach(settings, test => Execute(test, result));
                Log.Logger.Information("All states retreived");
                return result;
            }
            catch
            {
                Log.Logger.Error("Something went wrong...");
            }
            return null;
        }

        private void Execute(Setting setting, GenericResults results)
        {
            _handlers[setting.Type].Handle(setting, results);
        }
    }
}