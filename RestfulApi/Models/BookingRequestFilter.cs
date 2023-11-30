namespace RestfulApi.Models
{
    public class BookingRequestFilter
    {
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public bool ShowAvailable { get; set; }
        public int? StubId { get; set; }
        public int? OrderId { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CustomerPhone { get; set; }
        public BookingSortOption SortOption { get; set; } = 0;
    }
}
