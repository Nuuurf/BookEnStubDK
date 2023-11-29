using System.Data;
using RestfulApi.Models;

namespace RestfulApi.DAL {
    public interface IDBBooking {

        public Task<int> CreateBooking(IDbConnection conn, Booking booking, int bookingOrderID, IDbTransaction transaction = null!);

        public Task<int> CreateNewBookingOrder(IDbConnection conn, IDbTransaction trans);

        public Task<bool> DeleteBooking(IDbConnection conn, int bookingID, IDbTransaction trans = null!);

        //bool UpdateBooking(int bookingID, Booking booking);

        public Task<List<Booking>> GetBookingsInTimeslot(IDbConnection conn, DateTime start, DateTime end, IDbTransaction transaction = null!);

        public Task<List<AvailableBookingsForTimeframe>> GetAvailableBookingsForGivenDate(IDbConnection conn, DateTime date, IDbTransaction transaction = null!);

    }
}
