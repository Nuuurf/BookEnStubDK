namespace RestfulApi.Models
{
    public class Booking
    {
        public int? Id { get; set; }
        public int? OrderID { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public string? Notes { get; set; }
        public int? StubId { get; set; }
        public Customer? Customer { get; set; }
    }
}
