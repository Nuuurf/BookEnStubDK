using RestfulApi.Models;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using Microsoft.VisualBasic;
using System.Data;
using System.Linq.Expressions;
using System.Transactions;
using System.Data.Common;

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
        public async Task<int> CreateBooking(IDbConnection conn, Booking booking, int bookingOrderID, IDbTransaction transaction = null)
        {
            string script = "InsertBooking";
            int newBookingId = 0;

            try {
                var parameter = new { date = booking.TimeStart, orderId = bookingOrderID, note = booking.Notes };
                int result = await conn.QueryFirstOrDefaultAsync<int>(script, parameter, commandType: CommandType.StoredProcedure, transaction: transaction);

                newBookingId = result;
            }
            catch(Exception ex)
            {
                throw;
            }
            return newBookingId;

        }
        // Not implemented
        public async Task<List<Booking>> GetBookingsInTimeslot(IDbConnection conn, DateTime start, DateTime end, IDbTransaction transaction = null)
        {
            string script = "SELECT id, TimeStart, TimeEnd, Notes, StubId FROM Booking WHERE TimeStart < @TimeEnd AND TimeEnd > @TimeStart";

            List<Booking> bookings = new List<Booking>();

            try
            {
                bookings = (await conn.QueryAsync<Booking>(script, new { TimeStart = start, TimeEnd = end }, transaction: transaction)).ToList();
            }
            catch {
                //if it fails send null for error handling in blc
                bookings = null;
            }

            return bookings.ToList();
        }
        public async Task<int> CreateNewBookingOrder(IDbConnection conn, IDbTransaction trans = null)
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
        

        public async Task<List<AvailableBookingsForTimeframe>> GetAvailableBookingsForGivenDate(IDbConnection conn, DateTime date, IDbTransaction transaction = null) {
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

        public async Task<bool> CreateMultipleBookings(IDbConnection conn, List<List<Booking>> dateGroupedBookings, int bookingOrderID, IDbTransaction transaction = null) {
            int generatedBookingOrderId = -1;
            int expectedAllChanges = 0;
            int actualAllChanges = 0;
            int groupSizeTemp = 0;
            int bookingsOnDateTemp = 0;
            int actualChangesTemp = 0;
            int expectedChangesTemp = 0;
            List<int> generatedBookingIds = new List<int>();
            //find the maximal amount of bookings per timeslot
            int maxBookingsPerTimeSlotTemp = await GetMaxStubs(conn, transaction);

            foreach (List<Booking> dateBookingGroup in dateGroupedBookings) {
                //find the size of the group wanted for this timeslot
                groupSizeTemp = dateBookingGroup.Count;
                //find the amount of bookings already on timeslot.

                bookingsOnDateTemp = GetBookingsInTimeslot(conn, dateBookingGroup[0].TimeStart, dateBookingGroup[0].TimeEnd, transaction).Result.Count;

                //if the group size wanted to be added can fit within the number of the remaining available stubs in the timeslot.
                if (groupSizeTemp <= maxBookingsPerTimeSlotTemp - bookingsOnDateTemp) {
                    //set up expected changes
                    actualChangesTemp = 0;
                    expectedChangesTemp = dateBookingGroup.Count;
                    expectedAllChanges += dateBookingGroup.Count;

                    foreach (Booking booking in dateBookingGroup) {
                        int generatedId = await CreateBooking(conn, booking, bookingOrderID, transaction);
                        if(generatedId > 0) {
                            actualChangesTemp++;
                            actualAllChanges++;
                            generatedBookingIds.Add(generatedId);
                        }

                        ////if creation was successful increment actual changes
                        //if (await CreateBooking(conn, booking, transaction) > 0) {
                        //    actualChangesTemp++;
                        //    actualAllChanges++;
                        //}
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
            if (expectedAllChanges == actualAllChanges)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<int> GetMaxStubs(IDbConnection conn, IDbTransaction transaction = null) {
            try
            {
                return await conn.QueryFirstOrDefaultAsync<int>("Select Count(*) from Stub", transaction: transaction);
            }
            catch
            {
                return -1;
            }
            
        }

        
    }
}
