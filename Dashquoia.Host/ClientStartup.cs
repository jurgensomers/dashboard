using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;

namespace Dashquoia.Host
{
    public class ClientStartup
    {
        public void Configuration(IAppBuilder app)
        {
            const string rootFolder = "./dist";
            var fileSystem = new PhysicalFileSystem(rootFolder);
            var options = new FileServerOptions
            {
                EnableDefaultFiles = true,
                FileSystem = fileSystem
            };

            app.UseFileServer(options);
        }
    }
}