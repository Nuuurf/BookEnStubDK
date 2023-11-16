using RestfulApi.Models;
using System.Data.SqlClient;
using Dapper;
using Microsoft.VisualBasic;
using System.Data;
using System.Linq.Expressions;

namespace RestfulApi.DAL {
    public class DBBooking : IDBBooking {

        private DBConnection conn;

        public DBBooking() {
            conn = DBConnection.Instance;
        }

        /// <summary>
        /// Gets a single booking with the id matching the booking in the database.
        /// </summary>
        /// <param name="id"> the id of the booking object that needs to be retrieved</param>
        /// <returns>A Booking object which is either null or not, depening if the object was found or not</returns>
        public Booking GetSingleBooking(int id) {
            string script = "SELECT * FROM Booking WHERE Id = @id";

            Booking booking = null;

            using (SqlConnection connection = conn.GetOpenConnection()) {
                booking = connection.QueryFirst<Booking>(script,
                    new { id }); // Queries database for booking matching id and returns first result
            }

            return booking;
        }

        public bool UpdateBooking(Booking updatedBooking) {
            string script
                = "UPDATE Booking SET TimeStart = @StartTime, TimeEnd = @EndTime, Notes = @Notes WHERE Id = @Id";

            int rowsAffected = 0;

            using (SqlConnection connection = conn.GetOpenConnection()) {
                rowsAffected = connection.Execute(script, updatedBooking);
            }

            return rowsAffected > 0;
        }

        /// <summary>
        /// Deletes a booking with the id matching the booking in the database.
        /// </summary>
        /// <param name="id"> the id of the booking object that needs to be deleted</param>
        /// <returns>A boolean indicading where the action was successful or not</returns>
        public bool DeleteBooking(int id) {
            string script = "DELETE FROM Booking WHERE Id = @id";

            bool success = false;

            using (SqlConnection connection = conn.GetOpenConnection()) {
                connection.Execute(script, new { id });
                success = true;
            }

            return success;
        }

        /// <summary>
        /// Inserts a new booking with its values into the database.
        /// </summary>
        /// <param name="booking"> the booking object that needs to be persisted</param>
        /// <returns>A boolean indicading where the action was successful or not</returns>

        public async Task<int> CreateBooking(Booking booking)
        {
            string script = "InsertBooking";


            int newBookingId = 0;


            using (SqlConnection con = conn.GetOpenConnection())
            {
                var parameter = new { date = booking.TimeStart };
                int result = await con.QueryFirstOrDefaultAsync<int>(script, parameter, commandType : CommandType.StoredProcedure);

                newBookingId = result;
            }
                return newBookingId;

        }
        // Not implemented
        public async Task<List<Booking>> GetBookingsInTimeslot(DateTime start, DateTime end) {
            string script = "SELECT id, TimeStart, TimeEnd, Notes, StubId FROM Booking WHERE TimeStart < @TimeEnd AND TimeEnd > @TimeStart";

            using (SqlConnection con = conn.GetOpenConnection()) {
                var bookings = await con.QueryAsync<Booking>(script, new { TimeStart = start, TimeEnd = end });
                return bookings.ToList();
            }
        }

        public async Task<List<AvailableBookingsForTimeframe>> GetAvaiableBookingsForGivenDate(DateTime date) {
            string script = "dbo.GetAvailableBookingsForDate";

            using (SqlConnection con = conn.GetOpenConnection()) {
                var parameter = new { date = date.Date };

                var result = await con.QueryAsync<AvailableBookingsForTimeframe>(script, parameter, commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
        }
    }
}
