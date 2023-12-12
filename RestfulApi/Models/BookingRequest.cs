using RestfulApi.DTOs;

namespace RestfulApi.Models {
    public class BookingRequest {
        public List<DTONewBooking> Appointments { get; set; }
        public DTOCustomer Customer { get; set; }
    }
}
