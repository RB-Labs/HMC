using System;

namespace App.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public User Author { get; set; }
        public DateTime Date { get; set; }
    }
}
