using RestfulApi;
using RestfulApi.DAL;
using RestfulApi.Models;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Transactions;

namespace RestfulApi.BusinessLogic {
    public class BookingDataControl : IBookingData {

        private readonly IDBBooking _dBBooking;
        private readonly IDbConnection _connection;
        private readonly ICustomerData _customerData;
        //Ready for dependency injection
        public BookingDataControl(IDBBooking dbBooking, ICustomerData customerControl ,IDbConnection connection) {
            //Needs to change with injection
            _dBBooking = dbBooking;
            _connection = connection;
            _customerData = customerControl;
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
                    foreach (Booking b in bookings) {
                        await _dBBooking.CreateBooking(_connection, b, newBookingOrderId, transaction);
                    }

                    //If no exceptions have been thrown, every thing went right. Commit
                    transaction.Commit();
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

        public async Task<List<AvailableBookingsForTimeframe>> GetAvailableBookingsForGivenDate(DateTime date) {
            DateTime currentDate = DateTime.Now.Date;

            if (date < currentDate) {
                return null!;
            }
            List<AvailableBookingsForTimeframe> availableBookings = await _dBBooking.GetAvailableBookingsForGivenDate(_connection, date);

            return availableBookings;
        }

        public async Task<List<Booking>> GetBookingsInTimeslot(DateTime start, DateTime end) {
            if (start >= end) {
                return null!;
            }
            List<Booking> bookings = await _dBBooking.GetBookingsInTimeslot(_connection, start, end);

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

    }
}
