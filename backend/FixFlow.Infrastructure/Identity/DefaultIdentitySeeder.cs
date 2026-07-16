using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace FixFlow.Infrastructure.Identity;

public static class DefaultIdentitySeeder
{
    public static async Task SeedAsync(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration)
    {
        var roles = new[] { "Admin", "Customer", "Technician" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        var adminFullName = configuration["AdminSeed:FullName"] ?? "System Admin";
        var adminEmail = configuration["AdminSeed:Email"] ?? "admin@fixflow.com";
        var adminPassword = configuration["AdminSeed:Password"] ?? "Admin123!";

        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser is null)
        {
            adminUser = new ApplicationUser
            {
                FullName = adminFullName,
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(adminUser, adminPassword);

            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to create default admin user: {errors}");
            }
        }

        if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}