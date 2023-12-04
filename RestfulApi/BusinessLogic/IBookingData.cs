using RestfulApi.Models;

namespace RestfulApi.BusinessLogic {
    public interface IBookingData {
        
        public Task<int> CreateBooking(List<Booking> booking, Customer customer);
        
        public Task<bool> DeleteBooking(int bookingId);
        
        public Task<List<Booking>> GetBookingsInTimeslot(BookingRequestFilter req);

        public Task<List<AvailableStubsForHour>> GetAvailableStubsForGivenTimeFrame(DateTime start, DateTime end);

        public System.Data.IsolationLevel TestInsertIsolationLevel(System.Data.IsolationLevel level);

    }
}
