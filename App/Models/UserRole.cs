using Microsoft.AspNetCore.Identity;

namespace App.Models
{
    public class UserRole: IdentityRole
    {
        public UserRole(): base()
        {
        }

        public UserRole(string roleName): base(roleName)
        {
        }
    }
}
