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
            string script = "Insert into bookings TimeStart, TimeEnd, Notes values (@StartTime, @EndTime, @Notes)";

            bool success = false;

            using(SqlConnection connection = _connectionString) {
                connection.Open();

                connection.Execute(script, booking);

                success = true;
            }

            return success;
        }

        // Not implemented
        public List<Booking> GetBookingsInTimeslot(DateTime start, DateTime end) {
            throw new NotImplementedException();
        }
    }
}
