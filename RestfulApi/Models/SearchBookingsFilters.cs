namespace RestfulApi.Models
{
    public class SearchBookingsFilters
    {
        public int? StubId { get; set; }
        public int? OrderId { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CustomerPhone { get; set; }
        public BookingSortOption SortOption { get; set; }
    }
}
