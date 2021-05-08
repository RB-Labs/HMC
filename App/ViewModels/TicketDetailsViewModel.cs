using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels
{
    public class TicketDetailsViewModel
    {
        public class History
        {
            [Display(Name = "Автор")]
            public string User { get; set; }

            [Display(Name = "Описание")]
            public string Description { get; set; }

            [Display(Name = "Статус")]
            public string Status { get; set; }

            [Display(Name = "Дата изменения")]
            public DateTime Date { get; set; }
        }

        public TicketDetailsViewModel()
        {
            TicketHistory = new List<History>();
        }

        public int Id { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        public string Text { get; set; }

        [Display(Name = "Автор")]
        public string Author { get; set; }

        [Display(Name = "Дата создания")]
        public DateTime Date { get; set; }

        public IList<History> TicketHistory { get; set; }
    }
}
