using System;
using System.Collections.Generic;
using AppExample.Core.Entities;
using AppExample.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppExample.Application.Common.Access
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderStructure> OrderStructures { get; set; }
        public DbSet<ServiceType> ServiceTypes { get; set; }
        public DbSet<Unit> Units { get; set; }

        public DbSet<Feedback> Feedbacks { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Order>().HasIndex(order => order.Number).IsUnique();

            var hasher = new PasswordHasher<ApplicationUser>();

            builder.Entity<IdentityRole<int>>().HasData(new List<IdentityRole<int>>
            {
                new IdentityRole<int>
                {
                    Id = 1,
                    Name = "Manager",
                    NormalizedName = "Manager".ToUpper()
                },
                new IdentityRole<int>
                {
                    Id = 2,
                    Name = "Client",
                    NormalizedName = "Client".ToUpper()
                },
                new IdentityRole<int>
                {
                    Id = 3,
                    Name = "Courier",
                    NormalizedName = "Courier".ToUpper()
                }
            });

            builder.Entity<Unit>().HasData(new List<Unit>
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
            });
            
            builder.Entity<ApplicationUser>().HasData(new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = 1,
                    UserName = "Manager",
                    NormalizedUserName = "Manager".ToUpper(),
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "manager"),
                    LockoutEnabled = true,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                },
                new ApplicationUser
                {
                    Id = 2,
                    UserName = "Client",
                    NormalizedUserName = "Client".ToUpper(),
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "client"),
                    LockoutEnabled = true,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                },
                new ApplicationUser
                {
                    Id = 3,
                    UserName = "Courier",
                    NormalizedUserName = "Courier".ToUpper(),
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "courier"),
                    LockoutEnabled = true,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                }
            });
            
            builder.Entity<IdentityUserRole<int>>().HasData(new List<IdentityUserRole<int>>
            {
                new IdentityUserRole<int>
                {
                    RoleId = 1,
                    UserId = 1
                },
                new IdentityUserRole<int>
                {
                    RoleId = 2,
                    UserId = 2
                },
                new IdentityUserRole<int>
                {
                    RoleId = 3,
                    UserId = 3
                }
            });

            base.OnModelCreating(builder);
        }
    }
}