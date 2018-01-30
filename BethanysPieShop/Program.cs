using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using BethanysPieShop.Models;
using Microsoft.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace BethanysPieShop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);
            var logger = NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
                                    
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    logger.Debug("Test Message");
                    // Requires using RazorPagesMovie.Models;
                    DbInitializer.Seed(services);
                }
                catch (Exception ex)
                {
                  //  var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.Error(ex, "An error occurred seeding the DB.");
                }
            }

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseNLog()
                .Build();

    }
}
