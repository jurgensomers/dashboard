using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dashquoia.Api.Assets;
using Dashquoia.Api.Configuration;
using Dashquoia.Api.Models;
using Newtonsoft.Json;
using Serilog;

namespace Dashquoia.Api.Managers
{
    public static class SettingsManager
    {
        public static IList<HostConfig> GetHosts()
        {
            var hosts = Load<Configuration.HostConfig>(AppSettings.ConfigurationPath("hosts.json"));
            return hosts;
        }

        public static IList<ServiceConfig> GetServices()
        {
            var services = Load<ServiceConfig>(AppSettings.ConfigurationPath("services.json"));
            return services;
        }

        public static IList<GroupConfig> GetGroups()
        {
            var groups = Load<GroupConfig>(AppSettings.ConfigurationPath("groups.json"));
            return groups;
        }

        public static IList<TfsBuild> GetTfsBuilds()
        {
            var builds = Load<TfsBuild>(AppSettings.ConfigurationPath("tfsbuilds.json"));
            return builds;
        }

        public static IList<EndpointInfo> GetEndpoints()
        {
            var settings = Get();
            var endpoints = new List<EndpointInfo>();
            foreach (var setting in settings)
            {
                var endpoint = new EndpointInfo
                {
                    Environment = setting.Environment,
                    Name = setting.Name,
                    Endpoint = setting.Service,
                    Type = setting.Type
                };
                endpoints.Add(endpoint);
            }

            return endpoints;
        }

        public static IList<Setting> Get()
        {
            try
            {
                var hosts = GetHosts();
                var services = GetServices();
                var groups = GetGroups();

                var all = new List<Setting>();

                foreach (var host in hosts)
                {
                    foreach (var group in groups)
                    {
                        foreach (var serviceName in group.Services)
                        {
                            var groupServices = services.Where(x => x.Name == serviceName && x.Host == host.Name);
                            foreach (var groupService in groupServices)
                            {
                                var setting = new Setting
                                {
                                    Environment = host.Environment,
                                    Owner = group.Owner,
                                    Group = group.Name,
                                    BindingType = groupService.BindingType,
                                    BindingName = groupService.BindingName,
                                    Action = groupService.Action,
                                    Service = GetHostPrefix(groupService.BindingType) + host.Address + "/" + groupService.Address,
                                    Identity = host.Identity,
                                    Type = groupService.Type,
                                    NameSpace = groupService.NameSpace,
                                    Name = groupService.Name
                                };
                                all.Add(setting);
                            }
                        }
                    }
                }
                return all;
            }
            catch (Exception exc)
            {
                Log.Logger.Fatal(exc, "Fatal error has occured : {exc}");
                throw;
            }
        }

        public static void SetHosts(IList<HostConfig> hosts)
        {
            Save(hosts, AppSettings.ConfigurationPath("hosts.json"));
        }

        public static void SetGroups(IList<GroupConfig> groups)
        {
            Save(groups, AppSettings.ConfigurationPath("groups.json"));
        }

        public static void SetTfsBuilds(IList<TfsBuild> tfsbuilds)
        {
            Save(tfsbuilds, AppSettings.ConfigurationPath("tfsbuilds.json"));
        }

        public static void SetServices(IList<ServiceConfig> services)
        {
            Save(services, AppSettings.ConfigurationPath("services.json"));
        }

        private static string GetHostPrefix(string bindingType)
        {
            switch (bindingType)
            {
                case "netTcpBinding": return "net.tcp://";
                case "wsHttpBinding": return "https://";
                default: return "http://";
            }
        }

        private static void Save<T>(T content, string source)
        {
            try
            {
                var rawContent = JsonConvert.SerializeObject(content);
                CreateBackup(source);
                File.WriteAllText(source, rawContent);
            }
            catch (Exception exc)
            {
                Log.Logger.Fatal(exc, "Fatal error has occured :  {exc}");
                throw;
            }
        }

        private static void CreateBackup(string source)
        {
            var timeStamp = DateTime.Now.ToString("yyyy-MM-dd HHmmss");
            var folder = AppSettings.BackupLocation;
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            var backupFileName = $"{folder}\\{Path.GetFileNameWithoutExtension(source)}-{timeStamp}{Path.GetExtension(source)}";
            File.Copy(source, backupFileName, true);
        }

        private static IList<T> Load<T>(string source)
        {
            try
            {
                using (StreamReader r = new StreamReader(source))
                {
                    string json = r.ReadToEnd();
                    var items = JsonConvert.DeserializeObject<List<T>>(json);
                    return items;
                }
            }
            catch (Exception exc)
            {
                Log.Logger.Fatal(exc, "Fatal error has occured :  {exc}");
                throw;
            }
        }
    }
}