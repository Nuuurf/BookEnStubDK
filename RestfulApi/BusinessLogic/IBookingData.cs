using RestfulApi.Models;

namespace RestfulApi.BusinessLogic {
    public interface IBookingData {
        public bool CreateBooking(Booking booking);

        public bool CreateMultipleBookings(List<Booking> bookings);


    }
}
