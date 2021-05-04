using App.Models;
using System;
using System.Collections.Generic;

namespace App.ViewModels
{
    public class TicketDetailsViewModel
    {
        public class History
        {
            public string User { get; set; }
            public string Description { get; set; }
            public string Status { get; set; }
            public DateTime Date { get; set; }
        }
        public TicketDetailsViewModel()
        {
            TicketHistory = new List<History>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string Author { get; set; }
        public DateTime Date { get; set; }
        public IList<History> TicketHistory { get; set; }
    }
}
