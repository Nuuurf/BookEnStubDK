using RestfulApi.Models;
using System.Data.SqlClient;
using Dapper;

namespace RestfulApi.DAL {
    public class DBBooking : IDBBooking {

        private SqlConnection _connectionString;

        public DBBooking() {
            _connectionString = DBConnection.Instance.GetOpenConnection();
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
