using System.Data;
using System.Text;
using Dapper;
using RestfulApi.Models;

namespace RestfulApi.DAL {

    public class DBBooking : IDBBooking {
        /// <summary>
        ///     Inserts a new booking with its values into the database.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="booking"> the booking object that needs to be persisted</param>
        /// <param name="transaction"></param>
        /// <returns>A boolean indicading where the action was successful or not</returns>
        public async Task<int> CreateBooking(IDbConnection conn, Booking booking, int bookingOrderID, IDbTransaction transaction = null!) {
            string script = "InsertBooking";
            int newBookingId = 0;
            try {
                var parameter = new { date = booking.TimeStart, orderId = bookingOrderID, note = booking.Notes };
                int result = await conn.QueryFirstOrDefaultAsync<int>(script, parameter, commandType: CommandType.StoredProcedure, transaction: transaction);

                newBookingId = result;
            }
            catch {
                throw;
            }
            return newBookingId;
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

        public async Task<bool> DeleteBooking(IDbConnection conn, int bookingId, IDbTransaction trans = null) {
            bool result = false;
            string script = "Delete from Booking where Id = @id";

            try {
                var parameter = new { id = bookingId };

                int affected = await conn.ExecuteAsync(script, parameter, transaction: trans);

                //number of rows updated
                if (affected == 1) {
                    result = true;
                }
                else if (affected < 1) {
                    //multiple deletions happened
                    throw new Exception($"Unexpected number of deletions ({affected}) while trying to delete booking with id: {bookingId}");
                }
                else {
                    throw new Exception("Unable to delete booking with id:" + bookingId);
                }
            }
            catch (Exception ex) {
                throw new Exception("Unexpect error happened while trying to delete booking \n" + ex.Message);
            }

            return result;
        }

        // Not implemented
        public async Task<List<Booking>> GetBookingsInTimeslot(IDbConnection conn, DateTime start, DateTime end, SearchBookingsFilters filters, IDbTransaction transaction = null!) {
            StringBuilder script = new StringBuilder(
                "SELECT b.Id, b.TimeStart, b.TimeEnd, b.Notes, b.StubId, b.BookingOrderID " +
                "FROM Booking b " +
                "INNER JOIN BookingOrder bo ON b.BookingOrderID = bo.Id " +
                "INNER JOIN Customer c ON bo.customer_id_FK = c.Id " +
                "WHERE b.TimeStart < @TimeEnd AND b.TimeEnd > @TimeStart");

            // Dynamically build the query based on provided filters
            if (filters.StubId.HasValue)
                script.Append(" AND b.StubId = @StubId");
            if (filters.OrderId.HasValue)
                script.Append(" AND b.BookingOrderID = @OrderId");
            if (!string.IsNullOrWhiteSpace(filters.CustomerEmail))
                script.Append(" AND c.email = @CustomerEmail");
            if (!string.IsNullOrWhiteSpace(filters.CustomerPhone))
                script.Append(" AND c.phone = @CustomerPhone");

            // Adjust the sorting mechanism
            switch (filters.SortOption) {
                case BookingSortOption.Date:
                    script.Append(" ORDER BY b.TimeStart");
                    break;
                case BookingSortOption.OrderId:
                    script.Append(" ORDER BY b.BookingOrderID");
                    break;
                case BookingSortOption.CustomerId:
                    script.Append(" ORDER BY bo.customer_id_FK");
                    break;
            }

            var parameters = new {
                TimeStart = start,
                TimeEnd = end,
                StubId = filters.StubId,
                OrderId = filters.OrderId,
                CustomerEmail = filters.CustomerEmail,
                CustomerPhone = filters.CustomerPhone
            };

            List<Booking> bookings;

            try {
                Console.WriteLine(script.ToString());
                bookings = (await conn.QueryAsync<Booking>(script.ToString(), parameters, transaction)).ToList();
            }
            catch {
                //if it fails send null for error handling in blc
                bookings = null!;
            }

            return bookings;
        }


        public async Task<int> CreateNewBookingOrder(IDbConnection conn, IDbTransaction trans = null!) {
            const string insertBookingOrderQuery = "INSERT INTO BookingOrder DEFAULT VALUES; SELECT SCOPE_IDENTITY();";
            int bookingOrderId = -1;

            try {
                // Create a new BookingOrder
                CommandDefinition commandDefinition = new CommandDefinition(insertBookingOrderQuery, transaction: trans);
                bookingOrderId = await conn.ExecuteScalarAsync<int>(commandDefinition);
            }
            catch {
                // Return fail value if failed to create BookingOrder
                return -1;
            }

            return bookingOrderId;
        }
    }
}