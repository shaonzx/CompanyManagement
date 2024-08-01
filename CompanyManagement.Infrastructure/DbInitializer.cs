using CompanyManagement.Core;
using CompanyManagement.Core.Entities;
using CompanyManagement.Infrastucture;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyManagement.Infrastructure
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<ApplicationDbContext>();
            var userManager = services.GetRequiredService<UserManager<Client>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            context.Database.Migrate();

            foreach (ClientRolesEnum role in Enum.GetValues(typeof(ClientRolesEnum)))
            {
                if (!await roleManager.RoleExistsAsync(role.ToString()))
                {
                    await roleManager.CreateAsync(new IdentityRole(role.ToString()));
                }
            }

            // seed admin
            var adminUser = await userManager.FindByNameAsync(ClientRolesEnum.Admin.ToString());
            if (adminUser == null)
            {
                adminUser = new Client { UserName = "admin", Email = "admin@example.com" };
                await userManager.CreateAsync(adminUser, "Admin@123");

                // Assign admin role to the user
                await userManager.AddToRoleAsync(adminUser, ClientRolesEnum.Admin.ToString());
            }
        }
    }
}
