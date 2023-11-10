namespace RestfulApi.DTOs
{
    public class AvailableBookingsForTimeframe
    {
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public int Available { get; set; }
    }
}
