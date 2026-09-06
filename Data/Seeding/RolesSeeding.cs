using Microsoft.AspNetCore.Identity;

namespace TeamTaskManager.Data.Seeding
{
    public static class RolesSeeding
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = { "Manager", "Employee" };
            foreach(var role in roleNames)
            {
                bool roleExists = await roleManager.RoleExistsAsync(role);
                if (!roleExists)
                    await roleManager.CreateAsync(new IdentityRole(role));

            }
            
        }
    }
}
