using App.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace App.ViewModels
{
    public class TicketEditViewModel
    {
        public TicketEditViewModel()
        {
            Statuses = Enum.GetNames(typeof(TicketStatus))
                .Select(x => new SelectListItem
                {
                    Value = x,
                    Text = x
                });
        }

        public int Id { get; set; }

        [Display(Name = "Статус")]
        [Required(ErrorMessage = "Необходимо выбрать статус")]
        public string NewStatus { get; set; }

        [Display(Name = "Описание")]
        [Required(ErrorMessage = "Описание заявки должно содержать не менее 5 символов")]
        [MinLength(5)]
        public string Description { get; set; }

        public IEnumerable<SelectListItem> Statuses { get; set; }
    }
}
