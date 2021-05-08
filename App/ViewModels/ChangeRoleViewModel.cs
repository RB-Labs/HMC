using App.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels
{
    public class ChangeRoleViewModel
    {
        public ChangeRoleViewModel()
        {
            AllRoles = new List<UserRole>();
            UserRoles = new List<string>();
        }

        public string UserId { get; set; }

        [Display(Name = "Электронная почта")]
        [Required]
        public string UserEmail { get; set; }

        public List<UserRole> AllRoles { get; set; }

        public IList<string> UserRoles { get; set; }
    }
}
