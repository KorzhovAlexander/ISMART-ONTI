using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppExample.Application.Common.Access;
using AppExample.Core.Entities;
using AppExample.Core.Entities.Identity;
using AppExample.Core.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AppExample.WebUI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetService<ApplicationDbContext>();

                    if (context.Database.IsInMemory())
                    {
                        logger.LogInformation("InMemoryDb");
                    }
                    else
                    {
                        logger.LogInformation("Local db");
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while migrating or seeding the database");
                    throw;
                }
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}