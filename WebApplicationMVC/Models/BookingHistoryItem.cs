namespace WebApplicationMVC.Models {
    public class BookingHistoryItem {
        public int? Id { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public string? Notes { get; set; }
    }
}

