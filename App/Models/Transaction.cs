namespace App.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public User Customer { get; set; }
        public double Value { get; set; }
        public string Description { get; set; }
    }
}
