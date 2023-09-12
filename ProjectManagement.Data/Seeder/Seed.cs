using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectManagement.Models.DatabaseContexts;
using ProjectManagement.Models.Domains.User.Enums;
using ProjectManagement.Models.Identity;

namespace ProjectManagement.Data.Seeder;
public class Seed
{
    private const string _adminPassword = "AdminUser$%0@admin6<o>";
    private static readonly string _userId = Guid.NewGuid().ToString();
    public static async Task EnsurePopulatedAsync(IApplicationBuilder app)
    {
        IServiceProvider serviceProvider = app.ApplicationServices.CreateScope().ServiceProvider;
        ApplicationDbContext context = serviceProvider.GetRequiredService<ApplicationDbContext>();
        UserManager<ApplicationUser> userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        if (!await context.Users.AnyAsync())
        {
            var createUser = await userManager.CreateAsync(Admin(), _adminPassword);
            string? role = Admin().UserRole.ToString();
            if (!createUser.Succeeded)
            {
                string errorMessage = createUser.Errors.FirstOrDefault()!.Description;
                Console.WriteLine($"Error occurred while trying to create the user. {errorMessage}");
            }
            bool roleExist = await roleManager.RoleExistsAsync(role);
            if (roleExist)
                await userManager.AddToRoleAsync(Admin(), role);
            else
                await roleManager.CreateAsync(new IdentityRole(role));
            Console.WriteLine("User created successfully.");
        }
    }

    private static ApplicationUser Admin()
    {
        return new ApplicationUser()
        {
            Id = _userId,
            UserName = "Admin",
            Name = "Admin",
            Email = "admin@chat.com",
            PhoneNumber = "04934082832",
            EmailConfirmed = true,
            UserRole = UserRole.Admin
        };
    }
}