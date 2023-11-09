namespace WebApplicationMVC.Models
{
    public class NewBooking
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Notes { get; set; }
        public int CustomerID { get; set; }
    }
}
