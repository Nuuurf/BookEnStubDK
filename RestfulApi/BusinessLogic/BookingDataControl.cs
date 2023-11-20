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
        //Ready for dependency injection
        public BookingDataControl(IDBBooking dbBooking, IDbConnection connection) {
            //Needs to change with injection
            _dBBooking = dbBooking;
            _connection = connection;
        }

        public async Task<int> CreateBooking(Booking booking)
        {
            if (booking == null)
            {
                throw new ArgumentNullException("Booking is null");
            }
            if (!ValidateBookingDates(booking))
            {
                throw new ArgumentException("Booking date format exception. Booking start date exceeds booking end date");
            }

            int newBookingId = -1;
            int newBookingOrderId = -1;
            
                try
                {
                    using(var transaction = _connection.BeginTransaction())
                    {
                        newBookingOrderId = await _dBBooking.CreateNewBookingOrder(_connection, transaction);
                    newBookingId = await _dBBooking.CreateBooking(_connection, booking, newBookingOrderId, transaction);
                    //newBookingOrderId = await _dBBooking.AddBookingsToBookingOrder(_connection, new int[] { newBookingId }, transaction);
                    }
                }
                catch
                {
                    newBookingId = 0;
                }
            
            if (newBookingId <= 0)
            {
                throw new Exception("There are not available stubs for bookings in the desired timeslot");
            }
            return newBookingOrderId;
        }

        public async Task<int> CreateMultipleBookings(List<Booking> bookings)
        {
            int newBookingOrderId = -1;
            bool success = false;
            bool DatesAreValid = true;
            List<List<Booking>> dateGroupedBookings = new List<List<Booking>>();

            try
            {
                if (!bookings.All(ValidateBookingDates))
                {
                    return -1;
                }
            }
            catch
            {
                return -1;
            }
            dateGroupedBookings = GroupBookingsByDate(bookings);

            using (var transaction = _connection.BeginTransaction(System.Data.IsolationLevel.Serializable))
            {
                newBookingOrderId = await _dBBooking.CreateNewBookingOrder(_connection, transaction);
                foreach (List<Booking> listedListBooking in dateGroupedBookings)
                {

                    //Assign available stubIds to stubs in group;
                    int[] stubAvailableNumbers = new int[0];
                    int lowestAvailableNumberTemp = 0;

                    List<Booking> BookingsAlreadyOnDate
                        = await _dBBooking.GetBookingsInTimeslot(_connection, listedListBooking[0].TimeStart,
                            listedListBooking[0].TimeEnd, transaction);

                    int[] stubNumbersTemp = new int[BookingsAlreadyOnDate.Count];

                    if ((BookingsAlreadyOnDate.Count + listedListBooking.Count) <= await _dBBooking.GetMaxStubs(_connection, transaction))
                    {
                        //Get stubIds for stub already on date
                        for (int i = 0; i < BookingsAlreadyOnDate.Count; i++)
                        {
                            stubNumbersTemp[i] = BookingsAlreadyOnDate[i].StubId;
                        }

                        stubAvailableNumbers = FindLowestAvailableNumbers(stubNumbersTemp, await _dBBooking.GetMaxStubs(_connection, transaction));

                        //Run through and assign available numbers
                        for (int i = 0; i < listedListBooking.Count; i++)
                        {
                            listedListBooking[i].StubId = stubAvailableNumbers[i];
                        }
                    }
                    else
                    {
                        //One of the grouped timeslots cannot fit within timeslot.
                        return -1;
                    }

                    success = await _dBBooking.CreateMultipleBookings(_connection, dateGroupedBookings, newBookingOrderId, transaction);
                    if (success)
                    {
                        transaction.Commit();

                        return newBookingOrderId;
                    }
                    else
                    {
                        transaction.Rollback();

                        return -1;
                    }
                }
                //Try assigning valid stubIds to all bookings
                return newBookingOrderId;
            }
            
        }
        


        public async Task<List<AvailableBookingsForTimeframe>> GetAvailableBookingsForGivenDate(DateTime date) {
            DateTime currentDate = DateTime.Now.Date;

            if (date < currentDate) {
                return null;
            }
            List<AvailableBookingsForTimeframe> availableBookings = await _dBBooking.GetAvailableBookingsForGivenDate(_connection, date);
            
            return availableBookings;
        }

        public async Task<List<Booking>> GetBookingsInTimeslot(DateTime start, DateTime end) {
            if (start > end) {
                return null;
            }
            List<Booking> bookings = await _dBBooking.GetBookingsInTimeslot(_connection, start, end);
            
            return bookings;
        }

        private int[] FindLowestAvailableNumbers(int[] array, int resourceLimit) {
            Array.Sort(array); // Sort the array in ascending order

            List<int> result = new List<int>();
            int smallestAvailable = 1;

            foreach (int number in array) {
                if (number > smallestAvailable && smallestAvailable <= resourceLimit) {
                    result.Add(smallestAvailable); // Add the smallest available number
                    smallestAvailable++; // Move to the next number
                }

                if (number == smallestAvailable) {
                    smallestAvailable++; // Move to the next number
                }
            }

            // Add remaining numbers up to the resource limit
            while (smallestAvailable <= resourceLimit) {
                result.Add(smallestAvailable);
                smallestAvailable++;
            }
            Array.Sort(result.ToArray());

            return result.ToArray();
        }

        private List<List<Booking>> GroupBookingsByDate(List<Booking> unPairedBookings)
        {
            List<List<Booking>> pairedBookings = new List<List<Booking>>();

            foreach (Booking booking in unPairedBookings)
            {
                bool addedToGroup = false;

                foreach (List<Booking> bookingGroup in pairedBookings)
                {
                    if (bookingGroup[0].TimeStart == booking.TimeStart && bookingGroup[0].TimeEnd == booking.TimeEnd)
                    {
                        bookingGroup.Add(booking);
                        addedToGroup = true;
                        break; // Found matching group, no need to continue searching
                    }
                }

                // If not added to any existing group, create a new group
                if (!addedToGroup)
                {
                    List<Booking> newGroup = new List<Booking> { booking };
                    pairedBookings.Add(newGroup);
                }
            }

            return pairedBookings;
        }

        private bool ValidateBookingDates(Booking booking)
        {
            bool validDates = false;

            if (booking.TimeStart >= DateTime.Now.ToUniversalTime().AddMinutes(-1) && booking.TimeEnd > DateTime.Now.ToUniversalTime())
            {
                if (booking.TimeStart < booking.TimeEnd)
                {
                    validDates = true;
                }
                else
                    new ArgumentException("Booking date format exception. Booking start date exceeds booking end date");
            }
            else
            {
                throw new ArgumentException("Booking date format exception. Booking dates preceding current date");
            }

            return validDates;
        }
        
    }
}
