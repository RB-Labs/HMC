using System;
using System.ComponentModel.DataAnnotations;

namespace App.Models
{

    public enum TicketStatus
    {
        New = 1,
        Pending = 2,
        InProgress = 3,
        Resolved = 4,
        Closed = 5,
        Rejected = 6
    }

    public class TicketHistory
    {
        public int Id { get; set; }

        [Required]
        public Ticket Ticket { get; set; }

        [Required]
        public User User { get; set; }

        [Display(Name = "Статус")]
        [Required]
        public TicketStatus Status { get; set; }

        [Display(Name = "Дата изменения")]
        [Required]
        public DateTime Date { get; set; }

        [Display(Name = "Описание")]
        [Required]
        public string Description { get; set; }
    }
}
