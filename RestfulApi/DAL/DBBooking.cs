using RestfulApi.Models;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using Microsoft.VisualBasic;
using System.Data;
using System.Linq.Expressions;

namespace RestfulApi.DAL {
    public class DBBooking : IDBBooking {

        private SqlConnection conn;
        private SqlTransaction trans;

        public DBBooking(SqlConnection inCon) {
            if (inCon == null) {
                conn = DBConnection.Instance.GetOpenConnection();
            }
            else {
                conn = inCon;
            }
        }

        public DBBooking(SqlTransaction inTrans) {
            if (inTrans == null) {
                throw new Exception("Cannot use null as transaction object");
            }
            else {
                trans = inTrans;
                conn = inTrans.Connection;
            }
        }

        public DBBooking() {
            conn = DBConnection.Instance.GetOpenConnection();
        }

        /// <summary>
        /// Gets a single booking with the id matching the booking in the database.
        /// </summary>
        /// <param name="id"> the id of the booking object that needs to be retrieved</param>
        /// <returns>A Booking object which is either null or not, depening if the object was found or not</returns>
        public Booking GetSingleBooking(int id) {
            string script = "SELECT * FROM Booking WHERE Id = @id";

            Booking booking = null;
            try {
                booking = conn.QueryFirst<Booking>(script, new { id }); // Queries database for booking matching id and returns first result
            }
            catch {
                //if it fails send null for errorhandling in blc
                return null;
            }

            return booking;
        }

        //public bool UpdateBooking(Booking updatedBooking)
        //{
        //    string script = "UPDATE Booking SET TimeStart = @StartTime, TimeEnd = @EndTime, Notes = @Notes WHERE Id = @Id";

        //    int rowsAffected = 0;

        //    try {
        //        rowsAffected = conn.Execute(script, updatedBooking);
        //    }
        //    catch {
        //        return false;
        //    }

        //    return rowsAffected > 0;
        //}

        /// <summary>
        /// Deletes a booking with the id matching the booking in the database.
        /// </summary>
        /// <param name="id"> the id of the booking object that needs to be deleted</param>
        /// <returns>A boolean indicading where the action was successful or not</returns>
        public bool DeleteBooking(int id) {
            string script = "DELETE FROM Booking WHERE Id = @id";

            bool success = false;

            try {
                conn.Execute(script, new { id });
                success = true;
            }
            catch {
                //if it fails send false for errorhandling in blc
                success = false;
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

            try {
                var parameter = new { date = booking.TimeStart, note = booking.Notes };
                int result = await conn.QueryFirstOrDefaultAsync<int>(script, parameter, commandType: CommandType.StoredProcedure);

                newBookingId = result;
            }
            catch {
                return await Task.FromResult(0);
            }
            return newBookingId;

        }
        /// <summary>
        /// Inserts a new booking with its values into the database.
        /// Version of the createBooking command compatible within transactions
        /// </summary>
        /// <param name="booking"> the booking object that needs to be persisted</param>
        /// <param name="transaction"> A transaction which has an existing connection assigned to it</param>
        /// <returns>A boolean indicading where the action was successful or not</returns>
        public async Task<bool> CreateBooking(Booking booking, SqlTransaction transaction) {
            string script = "Insert into booking (TimeStart, TimeEnd, Notes, StubId) values (@TimeStart, @TimeEnd, @Notes, @StubId)";

            try {
                // Use the existing transaction object for the command
                await conn.ExecuteAsync(script, booking, transaction);
                return true;
            }
            catch (Exception) {
                //if it fails return false for errorhandling in blc
                return false;
            }
        }
        // Not implemented
        public async Task<List<Booking>> GetBookingsInTimeslot(DateTime start, DateTime end)
        {
            string script = "SELECT id, TimeStart, TimeEnd, Notes, StubId FROM Booking WHERE TimeStart < @TimeEnd AND TimeEnd > @TimeStart";

            List<Booking> bookings = new List<Booking>();

            try
            {
                bookings = (await conn.QueryAsync<Booking>(script, new { TimeStart = start, TimeEnd = end })).ToList();
            }
            catch {
                //if it fails send null for error handling in blc
                bookings = null;
            }

            return bookings.ToList();
        }

        public async Task<List<AvailableBookingsForTimeframe>> GetAvaiableBookingsForGivenDate(DateTime date) {
            string script = "dbo.GetAvailableBookingsForDate";

            try {
                var parameter = new { date = date.Date };

                var result = await conn.QueryAsync<AvailableBookingsForTimeframe>(script, parameter, commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
            catch {
                return new List<AvailableBookingsForTimeframe>();
            }

        }

        public async Task<bool> CreateMultipleBookings(List<List<Booking>> dateGroupedBookings) {
            bool success = false;
            int expectedAllChanges = 0;
            int actualAllChanges = 0;
            int groupSizeTemp = 0;
            int bookingsOnDateTemp = 0;
            int actualChangesTemp = 0;
            int expectedChangesTemp = 0;
            //find the maximal amount of bookings per timeslot
            int maxBookingsPerTimeSlotTemp = await GetMaxStubs();

            foreach (List<Booking> dateBookingGroup in dateGroupedBookings) {
                //find the size of the group wanted for this timeslot
                groupSizeTemp = dateBookingGroup.Count;
                //find the amount of bookings already on timeslot.

                bookingsOnDateTemp = GetBookingsInTimeslot(dateBookingGroup[0].TimeStart, dateBookingGroup[0].TimeEnd).Result.Count;


                //if the group size wanted to be added can fit within the number of the remaining available stubs in the timeslot.
                if (groupSizeTemp <= maxBookingsPerTimeSlotTemp - bookingsOnDateTemp) {
                    //set up expected changes
                    actualChangesTemp = 0;
                    expectedChangesTemp = dateBookingGroup.Count;
                    expectedAllChanges += dateBookingGroup.Count;

                    foreach (Booking booking in dateBookingGroup) {
                        //if creation was successful increment actual changes
                        if (await CreateBooking(booking) > 0) {
                            actualChangesTemp++;
                            actualAllChanges++;
                        }
                    }
                    if (actualChangesTemp != expectedChangesTemp) {
                        //something went wrong in the database somehow
                        return false;
                    }
                }
                else {
                    //it is not possible to complete the whole booking.
                    return false;
                }

            }
            //Everything has updates as expected
            if (expectedAllChanges == actualAllChanges) {
                success = true;
            }
            return success;
        }

        public async Task<int> GetMaxStubs() {
            using (SqlConnection connection = DBConnection.Instance.GetOpenConnection()) {
                return await connection.QueryFirstOrDefaultAsync<int>("Select Count(*) from Stub");
            }
            return -1;
        }
    }
}
