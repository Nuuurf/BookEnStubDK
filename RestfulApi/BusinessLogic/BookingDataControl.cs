using Castle.Components.DictionaryAdapter.Xml;
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

            int newBookingId = 0;
            
                try
                {
                    newBookingId = await _dBBooking.CreateBooking(_connection, booking);
                }
                catch
                {
                    newBookingId = 0;
                }
            
            if (newBookingId <= 0)
            {
                throw new Exception("There are not available stubs for bookings in the desired timeslot");
            }
            return newBookingId;
        }

        public async Task<bool> CreateMultipleBookings(List<Booking> bookings)
        {
            bool success = false;
            bool DatesAreValid = true;
            List<List<Booking>> dateGroupedBookings = new List<List<Booking>>();

            try
            {
                if (!bookings.All(ValidateBookingDates))
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            dateGroupedBookings = GroupBookingsByDate(bookings);

            using (var transaction = _connection.BeginTransaction(System.Data.IsolationLevel.Serializable))
            {
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
                        return false;
                    }

                    success = await _dBBooking.CreateMultipleBookings(_connection, dateGroupedBookings, transaction);
                    if (success)
                    {
                        transaction.Commit();

                        return success;
                    }
                    else
                    {
                        transaction.Rollback();

                        return success;
                    }
                }
                //Try assigning valid stubIds to all bookings
                return success;
            }
            
        }
        


        public async Task<List<AvailableBookingsForTimeframe>> GetAvailableBookingsForGivenDate(DateTime date) {
            DateTime currentDate = DateTime.Now.Date;

            if (date < currentDate) {
                return null;
            }
            List<AvailableBookingsForTimeframe> availableBookings = await _dBBooking.GetAvaiableBookingsForGivenDate(_connection, date);
            
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

            if (booking.TimeStart >= DateTime.Now.AddMinutes(-1) && booking.TimeEnd > DateTime.Now)
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
        /*private List<List<Booking>> GroupBookingsByDate1(List<Booking> unPairedBookings) {
            List<List<Booking>> pairedBookings = new List<List<Booking>>();

            //Iterate through all unpaired bookings.
            foreach (Booking booking in unPairedBookings) {

                //If paired bookings are empty add a new list with booking as baseline
                if (pairedBookings.Count == 0) {
                    pairedBookings.Add(new List<Booking> { booking });
                }
                //iterate through all groups to find a match
                else {
                    foreach (List<Booking> bookingGroup in pairedBookings) {
                        bool groupFound = false;

                        //Compare first element in group to booking
                        if (bookingGroup[0].TimeStart == booking.TimeStart) {
                            if (bookingGroup[0].TimeEnd == booking.TimeEnd) {
                                //add if it matches any
                                groupFound = true;
                                bookingGroup.Add(booking);
                            }
                        }
                        //if no match is found in any of the paired bookings
                        //Make new group
                        if (!groupFound) {
                            pairedBookings.Add(new List<Booking> { booking });
                        }
                    }
                }
            }

            return pairedBookings;
        }*/
    }
}
