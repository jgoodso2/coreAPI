using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProjectInfo.API.Entities;

using Microsoft.Extensions.DependencyInjection;


namespace ProjectInfo.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    // Requires using RazorPagesMovie.Models;
                    var ProjectInfoContext = services.GetService<ProjectContext>();
                    ProjectInfoContext.EnsureSeedDataForContext();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            host.Run();

        }

        public static IWebHost BuildWebHost(string[] args) =>
           new WebHostBuilder()
                .UseKestrel(options =>
                {
                    // listen for HTTP
                    options.Listen(IPAddress.Parse("127.0.0.1"), 40000);

                    // retrieve certificate from store
                    using (var store = new X509Store(StoreName.My))
                    {
                        store.Open(OpenFlags.ReadOnly);
                        var certs = store.Certificates.Find(X509FindType.FindBySubjectName, "martin.wingtip.com", false);
                        if (certs.Count > 0)
                        {
                            var certificate = certs[0];

                            // listen for HTTPS
                            options.Listen(IPAddress.Parse("0.0.0.0"), 40001, listenOptions =>
                            {
                                listenOptions.UseHttps(certificate);
                            });
                        }
                    }
                })

                //.UseConfiguration(config)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                //.UseUrls("http://0.0.0.0:5000")  ///this may break debugging...
                .Build();
        //{
        //    CreateWebHostBuilder(args).Build().Run();
        //}

        //public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        //    WebHost.CreateDefaultBuilder(args)
        //        .UseStartup<Startup>();

    }
}
