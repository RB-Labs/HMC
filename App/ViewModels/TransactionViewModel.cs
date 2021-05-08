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

        [Display(Name = "Клиент")]
        [Required(ErrorMessage = "Необходимо выбрать клиента")]
        public string CustomerId { get; set; }

        [Display(Name = "Значение баланса")]
        [Required(ErrorMessage = "Необходимо задать значение")]
        public double Value { get; set; }

        [Display(Name = "Описание")]
        [Required(ErrorMessage = "Описание должно содержать не менее 3 символов")]
        [MinLength(3)]
        public string Description { get; set; }

        public IEnumerable<SelectListItem> Users { get; set; }
    }
}
