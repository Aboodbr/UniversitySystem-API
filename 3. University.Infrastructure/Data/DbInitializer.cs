using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using University.Infrastructure.Identity;

namespace University.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(UniversityDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        if (context.Database.IsSqlServer())
        {
            await context.Database.MigrateAsync();
        }

        // Seed logic would go here
    }
}
