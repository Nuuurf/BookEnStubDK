namespace WebApplicationMVC.DTOs
{
    public class AvailableBookingsForTimeframe
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Available { get; set; }
    }
}
