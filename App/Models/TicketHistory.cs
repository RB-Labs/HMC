using System;

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
        public Ticket Ticket { get; set; }
        public User User { get; set; }
        public TicketStatus Status { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}
