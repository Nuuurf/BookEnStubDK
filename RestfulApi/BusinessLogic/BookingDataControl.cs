using Dapper;
using Microsoft.Extensions.Caching.Memory;
using RestfulApi;
using RestfulApi.DAL;
using RestfulApi.Exceptions;
using RestfulApi.Models;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;

namespace RestfulApi.BusinessLogic {
    public class BookingDataControl : IBookingData {

        private readonly IDBBooking _dBBooking;
        private readonly IDbConnection _connection;
        private readonly ICustomerData _customerData;

        //Change here if the isolation level need to be changed in the methods that use the field.
        private System.Data.IsolationLevel _IsolationLevel = System.Data.IsolationLevel.Serializable;
        //Ready for dependency injection
        public BookingDataControl(IDBBooking dbBooking, ICustomerData customerControl ,IDbConnection connection) {
            //Needs to change with injection
            _dBBooking = dbBooking;
            _connection = connection;
            _customerData = customerControl;
        }

        //Used for isolation level injections, when testing concurrency control
        //public System.Data.IsolationLevel TestInsertIsolationLevel(System.Data.IsolationLevel level) {
        //    _IsolationLevel = level;
        //    return _IsolationLevel;
        //}

        public async Task<int> CreateBooking(List<Booking> bookings, Customer customer)
        {
            if (bookings == null) throw new ArgumentNullException("Booking is null");
            if (customer == null) throw new ArgumentNullException("Customer is null");
            if (!bookings.All(ValidateBookingDates))
            {
                throw new ArgumentException("Invalid booking dates");
            }

            int newBookingOrderId = -1;
            int customerId = -1;

            bool wasAssociated = false;

            using (var transaction = _connection.BeginTransaction(_IsolationLevel))
            {
                try
                {
                    newBookingOrderId = await _dBBooking.CreateNewBookingOrder(_connection, transaction);
                    customerId = await _customerData.CreateCustomer(_connection, customer, transaction);
                    wasAssociated = await _customerData.AssociateCustomerWithBookingOrder(_connection, customerId, newBookingOrderId, transaction);

                    if (!wasAssociated)
                    {
                        throw new Exception("Error associating customer with booking order");
                    }

                    foreach (var b in bookings)
                    {
                        b.StubId = await GetAvailableOrRandomStub(_connection, b.StubId, b.TimeStart, transaction);
                        if (b.StubId == null)
                        {
                            throw new OverBookingException($"No available stubs for {b.TimeStart}");
                        }
                        await _dBBooking.CreateBooking(_connection, b, newBookingOrderId, transaction);
                    }

                    transaction.Commit();
                    return newBookingOrderId;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
        public async Task<bool> DeleteBooking(int bookingId) {
            bool result = false;
            try {
                result = await _dBBooking.DeleteBooking(_connection, bookingId);

                //if (result == false) {
                //    throw new Exception($"Deletion of booking with id: {bookingId} resulted in unexpected result");
                //}
            }
            catch {
                throw;
            }
            return result;
        }
        public async Task<List<AvailableStubsForHour>> GetAvailableStubsForGivenTimeFrame(DateTime start, DateTime end)
        {
            if (start > end)
            {
                return null;
            }

            List<AvailableStubsForHour> availabilityList = new List<AvailableStubsForHour>();

            // Fetch all stubs
            List<int> allStubs = await _dBBooking.GetAllStubs(_connection);

            // Fetch all bookings in the specified time frame in a single query
            BookingRequestFilter req = new BookingRequestFilter { Start = start, End = end };
            List<Booking> allBookingsInRange = await _dBBooking.GetBookingsInTimeslot(_connection, req);

            for (DateTime date = start; date <= end; date = date.AddHours(1))
            {
                if (date.Hour < 9 || date.Hour >= 22) continue;

                List<int> bookedStubsForHour = allBookingsInRange
                    .Where(booking => booking.TimeStart >= date && booking.TimeStart < date.AddHours(1))
                    .Select(booking => booking.StubId)
                    .Where(stubId => stubId.HasValue) // Filter out null values
                    .Select(stubId => stubId.Value)
                    .Distinct()
                    .ToList();

                // Use Except with both collections being List<int>
                List<int> availableStubs = allStubs.Except(bookedStubsForHour).ToList();

                availabilityList.Add(new AvailableStubsForHour
                {
                    TimeStart = date,
                    TimeEnd = date.AddHours(1),
                    AvailableStubIds = availableStubs
                });
            }

            return availabilityList;
        }

        public async Task<List<Booking>> GetBookingsInTimeslot(BookingRequestFilter req) {
            if (req.Start >= req.End) {
                return null!;
            }
            List<Booking> bookings = await _dBBooking.GetBookingsInTimeslot(_connection, req);

            return bookings;
        }

        private bool ValidateBookingDates(Booking booking) {
            bool validDates = false;

            if(booking.TimeStart.Hour < 9 || booking.TimeStart.Hour > 21 || booking.TimeEnd.Hour < 10 || booking.TimeEnd.Hour > 22)
            {
                new ArgumentException("Bookings are outside of current opening hours");
            }
            else if (booking.TimeStart >= DateTime.Now.ToUniversalTime().AddMinutes(-1) && booking.TimeEnd > DateTime.Now.ToUniversalTime()) {
                if (booking.TimeStart < booking.TimeEnd) {
                    validDates = true;
                }
                else
                    new ArgumentException("Booking date format exception. Booking start date exceeds bookings end date");
            }
            else {
                throw new ArgumentException("Booking date format exception. Booking dates preceding current date");
            }

            return validDates;
        }
        private async Task<int?> GetAvailableOrRandomStub(IDbConnection conn, int? desiredStubId, DateTime startTime, IDbTransaction transaction = null)
        {
            var allStubs = await _dBBooking.GetAllStubs(conn, transaction);

            // Lock the rows during transaction
            var bookedStubs = await _dBBooking.GetBookedStubsForHour(conn, startTime, transaction, true);

            int? selectedStubId = null;
            if (desiredStubId.HasValue && !bookedStubs.Contains(desiredStubId.Value))
            {
                selectedStubId = desiredStubId;
            }

            if (!selectedStubId.HasValue)
            {
                var availableStubs = allStubs.Except(bookedStubs).ToList();
                if (availableStubs.Any())
                {
                    selectedStubId = availableStubs[new Random().Next(availableStubs.Count)];
                }
            }
            return selectedStubId;
        }
    }
}
