using System.Data;
using RestfulApi.Models;

namespace RestfulApi.DAL {
    public interface IDBBooking {

        public Task<int> CreateBooking(IDbConnection conn, Booking booking, IDbTransaction transaction = null);

        public Task<int> CreateMultipleBookings(IDbConnection conn, List<List<Booking>> dateGroupedBookings, IDbTransaction transaction = null);

        public Task<int> AddBookingsToBookingOrder(IDbConnection conn, int[] bookingIds, IDbTransaction trans);

        //bool DeleteBooking(int bookingID);

        //bool UpdateBooking(int bookingID, Booking booking);

        public Task<List<Booking>> GetBookingsInTimeslot(IDbConnection conn, DateTime start, DateTime end, IDbTransaction transaction = null);

        public Task<List<AvailableBookingsForTimeframe>> GetAvailableBookingsForGivenDate(IDbConnection conn, DateTime date, IDbTransaction transaction = null);
        public Task<int> GetMaxStubs(IDbConnection conn, IDbTransaction transaction = null);

        //List<Booking> GetBookingsForDay(DateTime date);
    }
}
