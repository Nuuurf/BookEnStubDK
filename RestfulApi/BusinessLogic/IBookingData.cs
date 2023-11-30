using RestfulApi.Models;

namespace RestfulApi.BusinessLogic {
    public interface IBookingData {
        
        public Task<int> CreateBooking(List<Booking> booking, Customer customer);
        
        public Task<List<Booking>> GetBookingsInTimeslot(DateTime start, DateTime end, SearchBookingsFilters filters);

        public Task<List<AvailableStubsForHour>> GetAvailableStubsForGivenTimeFrame(DateTime start, DateTime end);

    }
}
