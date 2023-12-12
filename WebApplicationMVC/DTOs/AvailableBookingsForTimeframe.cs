namespace WebApplicationMVC.DTOs
{
    public class AvailableBookingsForTimeframe
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<int> Available { get; set; }
    }
}
