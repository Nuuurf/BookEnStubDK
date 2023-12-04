﻿using Microsoft.AspNetCore.SignalR;
using RestfulApi;
using RestfulApi.DAL;
using RestfulApi.Exceptions;
using RestfulApi.Models;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using RestfulApi.Utilities;

namespace RestfulApi.BusinessLogic {
    public class BookingDataControl : IBookingData {

        private readonly IDBBooking _dBBooking;
        private readonly IDbConnection _connection;
        private readonly ICustomerData _customerData;
        private readonly IHubContext<DateHub> _hubContext;
        private readonly IEmailDataControl _emailData;
        //Ready for dependency injection
        public BookingDataControl(IDBBooking dbBooking, ICustomerData customerControl, IHubContext<DateHub> hubContext, IDbConnection connection, IEmailDataControl emailData) {
            //Needs to change with injection
            _dBBooking = dbBooking;
            _customerData = customerControl;
            _hubContext = hubContext;
            _connection = connection;
            _emailData = emailData;
        }

        public async Task<int> CreateBooking(List<Booking> bookings, Customer customer) {
            //If null return exception
            if (bookings == null) {
                throw new ArgumentNullException("Booking is null");
            }

            if (customer == null)
            {
                throw new ArgumentNullException("Customer is null");
            }

            //Validate booking dates are in the right format and time
            if (!bookings.All(ValidateBookingDates)) {
                throw new ArgumentException("Booking date format exception. Booking start date exceeds bookings end date");
            }

            //Assign id to default error values
            int newBookingOrderId = -1;
            int customerId = -1;

            //check if some error is not caught 
            bool wasAssociated = false;

            using (var transaction = _connection.BeginTransaction(System.Data.IsolationLevel.Serializable)) {
                try {

                    //Create a new booking order and retrieve its id
                    newBookingOrderId = await _dBBooking.CreateNewBookingOrder(_connection, transaction);

                    //create or retrieve customer id
                    customerId = await _customerData.CreateCustomer(_connection, customer, transaction);

                    //associate customer id with booking order
                    wasAssociated = await _customerData.AssociateCustomerWithBookingOrder(_connection, customerId, newBookingOrderId, transaction);

                    if(!wasAssociated) {
                        throw new Exception("An error occured while associating customer with booking order. Customer id: " + customerId + ". BookingOrderId: " + newBookingOrderId);
                    }

                    //Persist each booking from input
                    foreach (Booking b in bookings)
                    {
                        b.StubId = await GetAvailableOrRandomStub(_connection, b.StubId, b.TimeStart, transaction);

                        if (b.StubId == null)
                        {
                            throw new OverBookingException($"No Available stubs for {b.TimeStart}");
                        }
                        await _dBBooking.CreateBooking(_connection, b, newBookingOrderId, transaction);
                    }

                    //If no exceptions have been thrown, every thing went right. Commit
                    transaction.Commit();

                    await _emailData.SendEmail(customer.Email, bookings, customer, newBookingOrderId);
                    
                    var updatedDates = new HashSet<string>();
                    // Send update to all relevant date groups
                    foreach (var booking in bookings)
                    {
                        string dateGroup = booking.TimeStart.ToString("yyyy-MM-dd");

                        // Check if the date has already been updated
                        if (!updatedDates.Contains(dateGroup))
                        {
                            // If not updated, send update and add to HashSet
                            await _hubContext.Clients.Group(dateGroup).SendAsync("ReceiveUpdate", dateGroup);
                            updatedDates.Add(dateGroup);
                        }
                    }
                }
                catch{
                    //Something went wrong. Rollback
                    transaction.Rollback();

                    //Pass the exception to calling method
                    throw;
                }

                //return the booking order id, which has the bookings assigned to it.
                return newBookingOrderId;
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
            // Get all stubs
            List<int> allStubs = await _dBBooking.GetAllStubs(conn, transaction);

            var bookedStubs = await _dBBooking.GetBookedStubsForHour(conn, startTime, transaction);
            if (desiredStubId != null || desiredStubId >= 1 || desiredStubId <= allStubs.Count)
            {

                // Check if the desired stub is available
                if (!bookedStubs.Contains(desiredStubId.Value))
                {
                    return desiredStubId;
                }
            }
            
            // Exclude booked stubs
            var availableStubs = allStubs.Except(bookedStubs).ToList();

            // Select a random stub if any are available
            if (availableStubs.Any())
            {
                Random random = new Random();
                return availableStubs[random.Next(availableStubs.Count)];
            }

            return null; // No stubs available
        }

    }
}
