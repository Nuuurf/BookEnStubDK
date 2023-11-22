namespace RestfulApi.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public string? Notes { get; set; }

        public int StubId { get; set; } //The assigned number for the stub assigned to this booking
    }
}
