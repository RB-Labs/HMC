using App.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace App
{
    public class RoleInitializer
    {
        private const string adminEmail = "root@gmail.com";
        private const string adminInitialPassword = "123456";
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<UserRole> roleManager)
        {
            if (await roleManager.FindByNameAsync(UserRole.Admin) == null)
            {
                await roleManager.CreateAsync(new UserRole(UserRole.Admin));
            }
            if (await roleManager.FindByNameAsync(UserRole.Manager) == null)
            {
                await roleManager.CreateAsync(new UserRole(UserRole.Manager));
            }
            if (await roleManager.FindByNameAsync(UserRole.Customer) == null)
            {
                await roleManager.CreateAsync(new UserRole(UserRole.Customer));
            }
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                var admin = new User { Email = adminEmail, UserName = adminEmail };
                var result = await userManager.CreateAsync(admin, adminInitialPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, UserRole.Admin);
                }
            }
        }
    }
}
