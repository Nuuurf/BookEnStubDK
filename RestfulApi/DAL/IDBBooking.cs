using RestfulApi.Models;

namespace RestfulApi.DAL {
    public interface IDBBooking {


        bool CreateBooking(Booking booking);

        //bool DeleteBooking(int bookingID);

        //bool UpdateBooking(int bookingID, Booking booking);

        List<Booking> GetBookingsInTimeslot(DateTime start, DateTime end);

        //List<Booking> GetBookingsForDay(DateTime date);

    }
}
