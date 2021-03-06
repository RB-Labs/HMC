using Microsoft.AspNetCore.Identity;

namespace App.Models
{
    public class UserRole : IdentityRole
    {
        public const string Admin = "admin";
        public const string Manager = "manager";
        public const string Customer = "customer";
        public const string Internal = "admin,manager";
        public UserRole(): base()
        {
        }

        public UserRole(string roleName): base(roleName)
        {
        }
    }
}
