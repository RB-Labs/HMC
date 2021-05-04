using App.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
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
        public string NewStatus { get; set; }
        public string Description { get; set; }
        public IEnumerable<SelectListItem> Statuses { get; set; }
    }
}
