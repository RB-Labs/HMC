using Microsoft.AspNetCore.Identity;

namespace App.Models
{
    public class User: IdentityUser
    {
        public User(): base()
        {
        }
        public User(string userName): base(userName)
        {
        }

    }
}
