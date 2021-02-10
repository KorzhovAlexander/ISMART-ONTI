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
                        logger.LogInformation("Start db migration");
                        await GenerateLocalInMemoryData(services, context);
                        logger.LogInformation("End db migration");
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

        private static async Task GenerateLocalInMemoryData(IServiceProvider services, ApplicationDbContext context)
        {
            var roleManager = services.GetService<RoleManager<IdentityRole<int>>>();
            var userManager = services.GetService<UserManager<ApplicationUser>>();

            var roles = new[]
            {
                RolesEnum.Admin,
                RolesEnum.User,
            };

            var hasher = new PasswordHasher<ApplicationUser>();

            foreach (var role in roles)
            {
                var identityRole = new IdentityRole<int>
                {
                    Id = (int) role,
                    Name = role.ToString(),
                    NormalizedName = role.ToString().ToUpper()
                };

                var user = new ApplicationUser
                {
                    Id = (int) role,
                    UserName = $"{role.ToString()}@yandex.ru",
                    Email = $"{role.ToString()}@yandex.ru",
                    NormalizedUserName = role.ToString().ToUpper(),
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, role.ToString().ToLower()),
                    LockoutEnabled = true,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                };

                await roleManager.CreateAsync(identityRole);
                await userManager.CreateAsync(user);
                await userManager.AddToRoleAsync(user, role.ToString());
            }

            await context.SaveChangesAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}