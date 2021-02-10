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
                    Name = "Admin",
                    NormalizedName = "admin".ToUpper()
                },
                new IdentityRole<int>
                {
                    Id = 2,
                    Name = "User",
                    NormalizedName = "User".ToUpper()
                }
            });
            
            builder.Entity<ApplicationUser>().HasData(new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = 1,
                    UserName = "email.for.testing.v1@gmail.com",
                    Email = "email.for.testing.v1@gmail.com",
                    NormalizedUserName = "email.for.testing.v1@gmail.com".ToUpper(),
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "administrator"),
                    LockoutEnabled = false,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                }
            });
            
            builder.Entity<IdentityUserRole<int>>().HasData(new List<IdentityUserRole<int>>
            {
                new IdentityUserRole<int>
                {
                    RoleId = 1,
                    UserId = 1
                }
            });

            base.OnModelCreating(builder);
        }
    }
}