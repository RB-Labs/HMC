using App.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace App.ViewModels
{
    public class TransactionViewModel
    {
        public TransactionViewModel()
        {
        }
        public TransactionViewModel(IEnumerable<User> users)
        {
            Users = users.Select(x => new SelectListItem
            {
                Value = x.Id,
                Text = x.UserName
            });
        }
        [Display(Name = "Customer")]
        public string CustomerId { get; set; }
        public double Value { get; set; }
        public string Description { get; set; }
        public IEnumerable<SelectListItem> Users { get; set; }
    }
}
