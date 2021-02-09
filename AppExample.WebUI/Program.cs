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
                RolesEnum.Manager,
                RolesEnum.Courier,
                RolesEnum.Client
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
                    UserName = role.ToString(),
                    NormalizedUserName = role.ToString().ToUpper(),
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, role.ToString().ToLower()),
                    LockoutEnabled = true,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                };

                await roleManager.CreateAsync(identityRole);
                await userManager.CreateAsync(user);
                await userManager.AddToRoleAsync(user, role.ToString());

                if (role == RolesEnum.Courier)
                {
                    var secondCourierName = $"{role.ToString()}2";
                    var secondCourier = new ApplicationUser
                    {
                        Id = ((int) role) + 1,
                        UserName = secondCourierName,
                        NormalizedUserName = secondCourierName.ToUpper(),
                        EmailConfirmed = true,
                        PasswordHash = hasher.HashPassword(null, "AZDAadwagb611z5!1"),
                        LockoutEnabled = true,
                        SecurityStamp = Guid.NewGuid().ToString("D"),
                    };
                    await userManager.CreateAsync(secondCourier);
                    await userManager.AddToRoleAsync(secondCourier, role.ToString());
                }
            }

            var units = new List<Unit>
            {
                new Unit
                {
                    Id = 1,
                    FullName = "Грамм",
                    ShortName = "гр",
                    Description = "Грамм"
                },
                new Unit
                {
                    Id = 2,
                    FullName = "Килограмм",
                    ShortName = "кг",
                    Description = "Килограмм"
                },
                new Unit
                {
                    Id = 3,
                    FullName = "Метр",
                    ShortName = "м",
                    Description = "Метр"
                },
                new Unit
                {
                    Id = 4,
                    FullName = "Штука",
                    ShortName = "шт",
                    Description = "Штука"
                },
                new Unit
                {
                    Id = 5,
                    FullName = "Кубический метр",
                    ShortName = "куб. м",
                    Description = "Кубический метр"
                }
            };


            var serviceTypes = new List<ServiceType>
            {
                new ServiceType
                {
                    Id = 1,
                    Name = "Почтовые",
                    Description = "Почтовые"
                },
                new ServiceType
                {
                    Id = 2,
                    Name = "Курьерская доставка",
                    Description = "Курьерская доставка"
                },
                new ServiceType
                {
                    Id = 3,
                    Name = "Складские услуги",
                    Description = "Складские услуги"
                },
                new ServiceType
                {
                    Id = 4,
                    Name = "Мелкорозничная доставка",
                    Description = "Мелкорозничная доставка"
                },
            };

            await context.ServiceTypes.AddRangeAsync(serviceTypes);
            await context.Units.AddRangeAsync(units);
            await context.SaveChangesAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}