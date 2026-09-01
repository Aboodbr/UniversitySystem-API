using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic; 
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using University.Infrastructure.Identity;
using University.Domain.Entities;

namespace University.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(
        UniversityDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        // 1. Apply pending migrations automatically
        if (context.Database.IsSqlServer())
        {
            await context.Database.MigrateAsync();
        }

        // 2. Seed Roles
        var roles = new[] { "Admin", "Professor", "Student" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // 3. Seed Default Admin User (If no users exist)
        if (!userManager.Users.Any())
        {
            var adminUser = new ApplicationUser
            {
                FirstName = "System",
                LastName = "Admin",
                UserName = "admin@university.com",
                Email = "admin@university.com",
                EmailConfirmed = true
            };

            // Create the admin user with a strong default password
            var result = await userManager.CreateAsync(adminUser, "Admin@12345");

            // Assign the "Admin" role to this user
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        // ==========================================
        // 4. Seed Departments
        // ==========================================
        if (!context.Departments.Any())
        {
            var departments = new List<Department>
    {
        new Department { Name = "Computer Science", Code = "CS" },
        new Department { Name = "Information Systems", Code = "IS" },
        new Department { Name = "Software Engineering", Code = "SE" },
        new Department { Name = "Artificial Intelligence", Code = "AI" }
    };

            await context.Departments.AddRangeAsync(departments);
            await context.SaveChangesAsync();
        }
    }
}