namespace RestfulApi.Models {
    public class BookingRequest {
        public List<Booking> Appointments { get; set; }
        public Customer Customer { get; set; }
    }
}
