namespace RestfulApi.Models
{
    public class AvailableStubsForHour
    {
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public List<int> AvailableStubIds { get; set; }
    }
}
