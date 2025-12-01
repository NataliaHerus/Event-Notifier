using EventNotifier.IdentityServer.Data.Models;
using EventNotifier.IdentityServer.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventNotifier.IdentityServer.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task ApplyMigrations(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<IdenityServerDbContext>();
            var serviceProvider = scope.ServiceProvider;
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (dbContext!.Database.GetPendingMigrations().Any())
            {
                await dbContext.Database.MigrateAsync();
            }
            foreach (var role in Enum.GetValues(typeof(Roles)).Cast<Roles>().Select(r => r.ToString()))
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            const string USERNAME = "myadmin@myadmin.com";
            const string PASSWORD = "Admin1@";

            var existingUser = userManager.FindByNameAsync(USERNAME).Result;

            if (existingUser == null)
            {
                var user = new User
                {
                    UserName = USERNAME,
                    Email = USERNAME,
                };

                var result = userManager.CreateAsync(user, PASSWORD).Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
        }
    }
}
