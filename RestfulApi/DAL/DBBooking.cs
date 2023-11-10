using RestfulApi.Models;
using System.Data.SqlClient;
using Dapper;
using Microsoft.VisualBasic;

namespace RestfulApi.DAL {
    public class DBBooking : IDBBooking {

        private SqlConnection _connectionString;

        public DBBooking() {
            _connectionString = DBConnection.Instance.GetOpenConnection();
        }

        /// <summary>
        /// Gets a single booking with the id matching the booking in the database.
        /// </summary>
        /// <param name="id"> the id of the booking object that needs to be retrieved</param>
        /// <returns>A Booking object which is either null or not, depening if the object was found or not</returns>
        public Booking GetSingleBooking(int id) {
            string script = "SELECT * FROM Booking WHERE Id = @id";

            Booking booking = null;

            using (SqlConnection connection = _connectionString) {
                booking = connection.QueryFirst<Booking>(script, new { id }); // Queries database for booking matching id and returns first result
            }

            return booking;
        }

        public bool UpdateBooking(Booking updatedBooking) {
            string script = "UPDATE Booking SET TimeStart = @StartTime, TimeEnd = @EndTime, Notes = @Notes WHERE Id = @Id";
            
            int rowsAffected = 0;
            
            using (SqlConnection connection = _connectionString) {
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

            using (SqlConnection connection = _connectionString) {
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
        public bool CreateBooking(Booking booking) {
            string script = "Insert into booking (TimeStart, TimeEnd, Notes) values (@TimeStart, @TimeEnd, @Notes)";

            bool success = false;

            using(SqlConnection con = DBConnection.Instance.GetOpenConnection()) {

                con.Execute(script, booking);

            string script = "Insert into booking TimeStart, TimeEnd, Notes values (@StartTime, @EndTime, @Notes)";

            bool success = false;

            using (SqlConnection connection = _connectionString) {
                connection.Execute(script, booking);
                success = true;
            }

            return success;
        }

        // Not implemented
        public List<Booking> GetBookingsInTimeslot(DateTime start, DateTime end) {
            string script = "SELECT id, TimeStart, TimeEnd, Notes FROM Booking WHERE TimeStart >= @TimeStart AND TimeEnd <= @TimeEnd";

            List<Booking> bookings;

            using(SqlConnection con = DBConnection.Instance.GetOpenConnection()) {
                bookings = con.Query<Booking>(script, new { TimeStart = start, TimeEnd = end }).ToList();

                //SqlCommand cmd = con.CreateCommand(script);


            }

            return bookings;
        }
    }
}
