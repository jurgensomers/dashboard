using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dashquoia.Api.Assets;
using Dashquoia.Api.Configuration;
using Dashquoia.Api.Infrastructure;
using Dashquoia.Api.Models;
using Microsoft.TeamFoundation.Build.WebApi;
using Microsoft.VisualStudio.Services.Client;
using Microsoft.VisualStudio.Services.WebApi;
using Serilog;

namespace Dashquoia.Api.Managers
{
    public class TfsManager
    {
        public TfsManager()
        {
            _cacheManager = new CacheManager();
            Observer.StartObservingTfs(Load);
        }

        private readonly CacheManager _cacheManager;

        public TfsBuildResults Get(string owner = null, string build = null)
        {
            try
            {
                var locked = _cacheManager.GetLockTfs();
                if (locked != null)
                {
                    Log.Logger.Information($"Waiting...");
                    Task.WaitAll(locked);
                    Log.Logger.Information($"...and done");
                }

                var results = _cacheManager.GetTfs() ?? Load();

                // filter the results
                return results.Filter(owner, build);
            }
            catch (Exception e)
            {
                _cacheManager.ClearTfs(); 
                Log.Logger.Fatal(e, String.Empty);
                return new TfsBuildResults();
            }
        }

        public TfsBuildResults Load()
        {
            var builds = SettingsManager.GetTfsBuilds();
            var connection = new VssConnection(new Uri(AppSettings.TfsAddress), new VssClientCredentials(true));
            var client = connection.GetClient<BuildHttpClient>();
            //var buildDefinisions = client.GetDefinitionsAsync(project: "sequoia").GetAwaiter().GetResult();
            var buildResults = client.GetBuildsAsync(AppSettings.TfsProject).GetAwaiter().GetResult();

            var result = new TfsBuildResults {Date = DateTime.Now};

            foreach (var build in builds)
            {
                //var tfsbuild = buildDefinisions.FirstOrDefault(x => x.Name == build.Name);
                var tfsbuildResult = buildResults.OrderBy(x => x.BuildNumber).FirstOrDefault(x => x.Definition.Name == build.Name);
                if (tfsbuildResult != null)
                {
                    result.Results.Add(new TfsBuildStatus
                    {
                        Name = build.Name,
                        Owner = build.Owner,
                        Status = tfsbuildResult.Result?.ToString() ?? tfsbuildResult.Status?.ToString() ?? "Unknown"
                    });
                }
            }

            _cacheManager.ReleaseTfs();
            _cacheManager.Store(result);

            return result;
        }

        public IList<string> GetBuildDefinitions()
        {
            var connection = new VssConnection(new Uri(AppSettings.TfsAddress), new VssClientCredentials(true));
            var client = connection.GetClient<BuildHttpClient>();
            var buildDefinisions = client.GetDefinitionsAsync(project: AppSettings.TfsProject).GetAwaiter().GetResult();
            return buildDefinisions.Select(x => x.Name).ToList();
        }
    }
}