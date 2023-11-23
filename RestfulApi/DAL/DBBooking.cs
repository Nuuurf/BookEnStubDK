using RestfulApi.Models;
using System.Data.SqlClient;
using System.Data;
using Dapper;


namespace RestfulApi.DAL {
    public class DBBooking : IDBBooking {


        public DBBooking() {
        }

        /// <summary>
        /// Inserts a new booking with its values into the database.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="booking"> the booking object that needs to be persisted</param>
        /// <param name="transaction"></param>
        /// <returns>A boolean indicading where the action was successful or not</returns>
        public async Task<int> CreateBooking(IDbConnection conn, Booking booking, int bookingOrderID, IDbTransaction transaction = null!)
        {
            string script = "InsertBooking";
            int newBookingId = 0;

            try {
                var parameter = new { date = booking.TimeStart, orderId = bookingOrderID, note = booking.Notes };
                int result = await conn.QueryFirstOrDefaultAsync<int>(script, parameter, commandType: CommandType.StoredProcedure, transaction: transaction);

                newBookingId = result;
            }
            catch
            {
                throw;
            }
            return newBookingId;
        }

        // Not implemented
        public async Task<List<Booking>> GetBookingsInTimeslot(IDbConnection conn, DateTime start, DateTime end, IDbTransaction transaction = null!)
        {
            string script = "SELECT id, TimeStart, TimeEnd, Notes, StubId FROM Booking WHERE TimeStart < @TimeEnd AND TimeEnd > @TimeStart";

            List<Booking> bookings = new List<Booking>();

            try
            {
                bookings = (await conn.QueryAsync<Booking>(script, new { TimeStart = start, TimeEnd = end }, transaction: transaction)).ToList();
            }
            catch {
                //if it fails send null for error handling in blc
                bookings = null!;
            }

            return bookings.ToList();
        }
        public async Task<int> CreateNewBookingOrder(IDbConnection conn, IDbTransaction trans = null!)
        {
            const string insertBookingOrderQuery = "INSERT INTO BookingOrder DEFAULT VALUES; SELECT SCOPE_IDENTITY();";
            int bookingOrderId = -1;

            try
            {
                // Create a new BookingOrder
                var commandDefinition = new CommandDefinition(insertBookingOrderQuery, transaction: trans);
                bookingOrderId = await conn.ExecuteScalarAsync<int>(commandDefinition);

            }
            catch
            {
                // Return fail value if failed to create BookingOrder
                return -1;
            }

            return bookingOrderId;
        }
        

        public async Task<List<AvailableBookingsForTimeframe>> GetAvailableBookingsForGivenDate(IDbConnection conn, DateTime date, IDbTransaction transaction = null!) {
            string script = "dbo.GetAvailableBookingsForDate";

            try {
                var parameter = new { date = date.Date };

                var result = await conn.QueryAsync<AvailableBookingsForTimeframe>(script, parameter, commandType: CommandType.StoredProcedure, transaction: transaction);

                return result.ToList();
            }
            catch {
                return new List<AvailableBookingsForTimeframe>();
            }

        }
        
    }
}
