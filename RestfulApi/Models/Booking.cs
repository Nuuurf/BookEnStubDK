namespace RestfulApi.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Notes { get; set; }
        public int CustomerID { get; set; }
        public int StubID { get; set; }
    }
}
