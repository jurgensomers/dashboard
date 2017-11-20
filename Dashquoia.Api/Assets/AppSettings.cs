using System;
using System.Configuration;

namespace Dashquoia.Api.Assets
{
    public static class AppSettings
    {
        public static string ConfigurationPath(string fileName)
        {
            return $"{ConfigurationManager.AppSettings.Get(ConfigurationKeys.ConfigLocation)}\\" + fileName;
        }

        public static string BackupLocation => ConfigurationManager.AppSettings.Get(ConfigurationKeys.BackupLocation);
        public static string HistoryLocation => ConfigurationManager.AppSettings.Get(ConfigurationKeys.HistoryLocation);

        public static int RefreshRate => int.Parse(ConfigurationManager.AppSettings.Get(ConfigurationKeys.RefreshRate)) * 60 * 1000;
        public static TimeSpan SleepFrom => TimeSpan.Parse(ConfigurationManager.AppSettings.Get(ConfigurationKeys.SleepFrom));
        public static TimeSpan SleepUntil => TimeSpan.Parse(ConfigurationManager.AppSettings.Get(ConfigurationKeys.SleepUntil));
        public static string ServerAddress => ConfigurationManager.AppSettings.Get(ConfigurationKeys.ServerHostAddress);
        public static string TfsAddress => ConfigurationManager.AppSettings.Get(ConfigurationKeys.TfsAddress);

        public static string TfsProject => ConfigurationManager.AppSettings.Get(ConfigurationKeys.TfsProject);
    }
}