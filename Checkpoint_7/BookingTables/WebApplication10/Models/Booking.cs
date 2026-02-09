namespace WebApplication10.Models
{
    public class Booking
    {
        public int Id { get; set; } 
        public int TableId { get; set; }
        public DateTime Time { get; set; }
        public string Customer { get; set; } 
    }
}
