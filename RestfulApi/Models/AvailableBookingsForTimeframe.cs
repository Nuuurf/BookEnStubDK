namespace RestfulApi.Models
{
    public class AvailableBookingsForTimeframe
    {
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public int AvailableStubs { get; set; }
    }
}
