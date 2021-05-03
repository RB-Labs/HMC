using App.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace App
{
    public class RoleInitializer
    {
        private const string adminEmail = "root@gmail.com";
        private const string adminInitialPassword = "123456";
        private const string adminRole = "admin";
        private const string managerRole = "manager";
        private const string customerRole = "customer";
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<UserRole> roleManager)
        {
            if (await roleManager.FindByNameAsync(adminRole) == null)
            {
                await roleManager.CreateAsync(new UserRole(adminRole));
            }
            if (await roleManager.FindByNameAsync(managerRole) == null)
            {
                await roleManager.CreateAsync(new UserRole(managerRole));
            }
            if (await roleManager.FindByNameAsync(customerRole) == null)
            {
                await roleManager.CreateAsync(new UserRole(customerRole));
            }
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                var admin = new User { Email = adminEmail, UserName = adminEmail };
                var result = await userManager.CreateAsync(admin, adminInitialPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, adminRole);
                }
            }
        }
    }
}
