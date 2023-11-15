using RestfulApi.Models;

namespace RestfulApi.DAL {
    public interface IDBBooking {

        public Task<int> CreateBooking(Booking booking);

        public Task<bool> CreateMultipleBookings(List<List<Booking>> dateGroupedBookings);

        //bool DeleteBooking(int bookingID);

        //bool UpdateBooking(int bookingID, Booking booking);

        public Task<List<Booking>> GetBookingsInTimeslot(DateTime start, DateTime end);

        public Task<List<AvailableBookingsForTimeframe>> GetAvaiableBookingsForGivenDate(DateTime date);
        public Task<int> GetMaxStubs();

        //List<Booking> GetBookingsForDay(DateTime date);
    }
}
