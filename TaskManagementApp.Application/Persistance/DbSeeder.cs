using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace TaskManagementApp.Application.Persistance
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            await context.Database.MigrateAsync();

            if (!context.Roles.Any())
            {
                context.Roles.AddRange(
                    new Role { RoleId = 1, Name = "Manager" },
                    new Role { RoleId = 2, Name = "Developer" }
                );
                await context.SaveChangesAsync();
            }

            if (!context.Users.Any())
            {
                context.Users.Add(new User
                {
                    FullName = "Admin Manager",
                    Email = "manager@taskapp.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    RoleId = 1
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
