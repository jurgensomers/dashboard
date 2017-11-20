using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dashquoia.Api.Assets;
using Dashquoia.Api.Models;
using Serilog;
using Timer = System.Timers.Timer;

namespace Dashquoia.Api.Managers
{
    public static class Observer
    {
        private static Timer _serviceTimer;
        private static Timer _tfsTimer;
        private static CacheManager _cacheManager;
        private static Func<GenericResults> _onObserve;
        private static Func<TfsBuildResults> _onObserveTfs;

        static Observer()
        {
            _cacheManager = new CacheManager();
        }

        public static void StartObservingServices(Func<GenericResults> onObserve)
        {
            if (_serviceTimer == null)
            {
                _serviceTimer?.Stop();

                _onObserve = onObserve; 
                _serviceTimer = new Timer(AppSettings.RefreshRate);
                _serviceTimer.Elapsed += async (sender, e) => await HandleServiceTimer();
                _serviceTimer.AutoReset = true;
                _serviceTimer.Start();
                Log.Logger.Information("Timer has started");
            }
        }

        public static void StartObservingTfs(Func<TfsBuildResults> onObserveTfs)
        {
            if (_tfsTimer == null)
            {
                _tfsTimer?.Stop();
                 
                _onObserveTfs = onObserveTfs;
                _tfsTimer = new Timer(AppSettings.RefreshRate);
                _tfsTimer.Elapsed += async (sender, e) => await HandleTfsTimer();
                _tfsTimer.AutoReset = true;
                _tfsTimer.Start();
                Log.Logger.Information("Timer has started");
            }
        }

        private static async Task HandleServiceTimer()
        {
            if (!IsActive())
            {
                Log.Logger.Information("Sleeping...");
                _cacheManager.ClearServices();
            }
            else
            {
                var locked = _cacheManager.GetLockServices();
                if (locked == null)
                {
                    await Task.Factory.StartNew(() =>
                    {
                        var results = _onObserve();
                        _cacheManager.Store(results); 
                    });
                }
            }
        }

        private static async Task HandleTfsTimer()
        {
            if (!IsActive())
            {
                Log.Logger.Information("Sleeping...");
                _cacheManager.ClearTfs();
            }
            else
            {
                var locked = _cacheManager.GetLockTfs();
                if (locked == null)
                {
                    await Task.Factory.StartNew(() =>
                    { 

                        var tfs = _onObserveTfs();
                        _cacheManager.Store(tfs);
                    });
                }
            }
        }


        public static bool IsActive()
        {
            return !(DateTime.Now.TimeOfDay < AppSettings.SleepUntil && DateTime.Now.TimeOfDay > AppSettings.SleepFrom);
        }
    }
}