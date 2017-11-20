using System;
using System.Configuration;
using System.Threading.Tasks;
using Dashquoia.Api;
using Microsoft.Owin.Hosting;
using Owin;
using Serilog;

namespace Dashquoia.Host
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.ColoredConsole()
                .CreateLogger();

            Run().GetAwaiter().GetResult();

        }

        private static async Task Run()
        {
            var serverTask = Task.Run(() => RunServer()); 
            var clientTask = Task.Run(() => RunClient());

            await serverTask;
            await clientTask;
        }

        private static void RunServer()
        {
            string serverBaseAddress = ConfigurationManager.AppSettings.Get("serverhostaddress");

            // Start OWIN host
            using (WebApp.Start<Startup>(url: serverBaseAddress))
            {
                Console.WriteLine($"Server is running on {serverBaseAddress}");
                Console.WriteLine("Press <enter> to stop server");
                Console.ReadLine();
                Console.WriteLine($"Server stopped");
            }
        }

        private static void RunClient()
        {
#if DEBUG
#else
            string baseAddress = ConfigurationManager.AppSettings.Get("clienthostaddress");

            // Start OWIN host
            using (WebApp.Start<ClientStartup>(url: baseAddress))
            {
                Console.WriteLine($"Client is running on {baseAddress}");
                Console.WriteLine("Press <enter> to stop client");
                Console.ReadLine();
                Console.WriteLine($"Client stopped");
            }
#endif
        }
    }

   
}