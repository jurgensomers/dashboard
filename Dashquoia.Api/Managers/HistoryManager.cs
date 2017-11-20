using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Dashquoia.Api.Assets;
using Dashquoia.Api.Infrastructure;
using Dashquoia.Api.Models;
using Newtonsoft.Json;
using Serilog;

namespace Dashquoia.Api.Managers
{
    public class HistoryManager
    {
        public void Store(GenericResults results)
        {
            var fileName = ResolveFile(results.Date);
            var rawContent = JsonConvert.SerializeObject(results);
            File.WriteAllText(fileName, rawContent);
        }

        public GenericResults Get(string date, string owner = null, string group = null, string environment = null)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(date))
                {
                    DateTime requestedDate;
                    if (DateTime.TryParseExact(date, "yyyy-MM-dd HHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out requestedDate))
                    {
                        var results = Load(requestedDate);
                        return results.Filter(owner, group, environment);
                    }
                }
                return new GenericResults();
            }
            catch (Exception e)
            {
                Log.Logger.Fatal(e, String.Empty);
                return new GenericResults();
            }
        }


        public IList<HistoryResult> Service(string name, string environment)
        {
            var results = new List<HistoryResult>();
            var histories = List();
            foreach (var entry in histories)
            {
                DateTime requestedDate;
                if (DateTime.TryParseExact(entry, "yyyy-MM-dd HHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out requestedDate))
                {
                    var result = Load(requestedDate);
                    result = result.Filter(environment: environment, service:name);
                    results.AddRange(result.Results.Select(x => new HistoryResult { Date = requestedDate, Status = x.Status}));
                }  
            }

            return results;
        }

        public GenericResults Load(DateTime date)
        {
            var fileName = ResolveFile(date);
            if (File.Exists(fileName))
            {
                using (StreamReader r = new StreamReader(fileName))
                {
                    string json = r.ReadToEnd();
                    var result = JsonConvert.DeserializeObject<GenericResults>(json);
                    return result;
                }
            }

            return null;
        }

        private string ResolveFile(DateTime date)
        {
            var path = AppSettings.HistoryLocation;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return $"{path}\\results-{date:yyyy-MM-dd HHmmss}.json";
        }

        public IList<string> List()
        {
            if (Directory.Exists(AppSettings.HistoryLocation))
            {
                var list = Directory.GetFiles(AppSettings.HistoryLocation, "results-*.json");
                return list.Select(x => x.Replace($"{AppSettings.HistoryLocation}\\results-", "").Replace(".json", "")).ToList();
            }
            return new List<string>();
        }

    }
}