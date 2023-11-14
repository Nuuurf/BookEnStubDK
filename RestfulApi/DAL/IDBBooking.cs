using RestfulApi.Models;

namespace RestfulApi.DAL {
    public interface IDBBooking {

        bool CreateBooking(Booking booking);

        //bool DeleteBooking(int bookingID);

        //bool UpdateBooking(int bookingID, Booking booking);

        Task<List<Booking>> GetBookingsInTimeslot(DateTime start, DateTime end);

        Task<List<AvailableBookingsForTimeframe>> GetAvaiableBookingsForGivenDate(DateTime date);

        //List<Booking> GetBookingsForDay(DateTime date);
    }
}
