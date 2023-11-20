using RestfulApi.Models;

namespace RestfulApi.BusinessLogic {
    public interface IBookingData {

        public Task<int> CreateMultipleBookings(List<Booking> bookings);
        
        public Task<int> CreateBooking(List<Booking> booking);
        
        public Task<List<AvailableBookingsForTimeframe>> GetAvailableBookingsForGivenDate(DateTime date);
        
        public Task<List<Booking>> GetBookingsInTimeslot(DateTime start, DateTime end);
    
    }
}
