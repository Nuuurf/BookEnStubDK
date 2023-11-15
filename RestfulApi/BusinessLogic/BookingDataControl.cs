using RestfulApi;
using RestfulApi.DAL;
using RestfulApi.Models;
using System.Data.Common;
using System.Data.SqlClient;

namespace RestfulApi.BusinessLogic {
    public class BookingDataControl : IBookingData {
        //nono
        private DBBooking _dBBooking;

        //Ready for dependency injection
        public BookingDataControl() {
            //Needs to change with injection
            _dBBooking = new DBBooking(DBConnection.Instance.GetOpenConnection());
        }

        public bool CreateBooking(Booking booking) {
            bool success = false;
            bool possibleDates = false;

            //List<Booking> bookings = _dBBooking.GetBookingsInTimeslot(booking.TimeStart, booking.TimeEnd);

            int maxStubs = new DBBooking(DBConnection.Instance.GetOpenConnection()).GetMaxStubs();
            int[] stubNumbers = new int[maxStubs];
            int lowestAvailableStubNumber = -1;

            //Check if dates are in the correct format and are possible
            possibleDates = ValidateBookingDates(booking);
            if (possibleDates) {
                using (SqlConnection con = DBConnection.Instance.GetOpenConnection()) {
                    _dBBooking = new DBBooking(con);
                    List<Booking> bookings = _dBBooking.GetBookingsInTimeslot(booking.TimeStart, booking.TimeEnd);
                    if (bookings.Count < maxStubs) {
                        for (int i = 0; i < bookings.Count; i++) {
                            // TODO: need to implement max stubs feature in DBStub
                            stubNumbers[i] = bookings[i].StubId;
                        }
                        // TODO: Maybe this functionality should be delegated to a seperate business logic controller for stubs.
                        lowestAvailableStubNumber = FindLowestAvailableNumber(stubNumbers);

                        if (lowestAvailableStubNumber != -1) {
                            //Set stubnumber in the Booking
                            booking.StubId = lowestAvailableStubNumber;
                            success = _dBBooking.CreateBooking(booking);
                        }
                        else {
                            //Find Lowest number algorithem failed
                            throw new Exception($"Find delegate appropriate stub algorithem failure. Suggested stub number: {lowestAvailableStubNumber}");
                        }
                    }
                    else {
                        //there is not more available stubs for this timeperiod
                        return success = false;
                    }
                }
            }
            else {
                //Dates are invalid
                return success = false;
            }

            return success;
        }

        public bool CreateMultipleBookings(List<Booking> bookings) {
            bool success = false;
            bool DatesAreValid = true;
            List<List<Booking>> dateGroupedBookings = new List<List<Booking>>();

            foreach (Booking booking in bookings) {
                try {
                    bool temp = ValidateBookingDates(booking);
                    if (temp = false) {
                        DatesAreValid = false;
                    }
                }
                catch {
                    DatesAreValid = false;
                }
            }

            if (DatesAreValid) {
                dateGroupedBookings = GroupBookingsByDate(bookings);
                //Try assigning valid stubIds to all bookings
                foreach(List<Booking> listedListBooking in dateGroupedBookings) {

                    //Assign available stubIds to stubs in group;
                    int[] stubAvailableNumbers = new int[0];
                    int lowestAvailableNumberTemp = 0;
                    List<Booking> BookingsAlreadyOnDate = _dBBooking.GetBookingsInTimeslot(listedListBooking[0].TimeStart, listedListBooking[0].TimeEnd);
                    int[] stubNumbersTemp = new int[BookingsAlreadyOnDate.Count];

                    if ((BookingsAlreadyOnDate.Count + listedListBooking.Count) <= _dBBooking.GetMaxStubs()) {
                        //Get stubIds for stub already on date
                        for (int i = 0; i < BookingsAlreadyOnDate.Count; i++) {
                            stubNumbersTemp[i] = BookingsAlreadyOnDate[i].StubId;
                        }
                        stubAvailableNumbers = FindLowestAvailableNumbers(stubNumbersTemp, _dBBooking.GetMaxStubs());

                        //Run through and assign available numbers
                        for (int i = 0; i < listedListBooking.Count; i++) {
                            listedListBooking[i].StubId = stubAvailableNumbers[i];
                        }
                    }
                    else {
                        //One of the grouped timeslots cannot fit within timeslot.
                        return false;
                    }

                }
                //if nothing has gone wrong yet, try and persist the bookings to the database
                using (SqlTransaction trans = DBConnection.Instance.GetOpenConnection().BeginTransaction(System.Data.IsolationLevel.Serializable)) {
                    success = _dBBooking.CreateMultipleBookings(dateGroupedBookings);
                    if(success) {
                        trans.Commit();
                    }
                    else {
                        trans.Rollback();
                    }
                }
            }
            else {

                return false;
            }

            return success;
        }

        private List<List<Booking>> GroupBookingsByDate(List<Booking> unPairedBookings) {
            List<List<Booking>> pairedBookings = new List<List<Booking>>();

            foreach (Booking booking in unPairedBookings) {
                bool addedToGroup = false;

                foreach (List<Booking> bookingGroup in pairedBookings) {
                    if (bookingGroup[0].TimeStart == booking.TimeStart && bookingGroup[0].TimeEnd == booking.TimeEnd) {
                        bookingGroup.Add(booking);
                        addedToGroup = true;
                        break; // Found matching group, no need to continue searching
                    }
                }

                // If not added to any existing group, create a new group
                if (!addedToGroup) {
                    List<Booking> newGroup = new List<Booking> { booking };
                    pairedBookings.Add(newGroup);
                }
            }

            return pairedBookings;
        }

        private List<List<Booking>> GroupBookingsByDate1(List<Booking> unPairedBookings) {
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
        }

        private bool ValidateBookingDates(Booking booking) {
            bool validDates = false;
            if (booking.TimeStart >= DateTime.Now.AddMinutes(-1) && booking.TimeEnd > DateTime.Now) {
                if (booking.TimeStart < booking.TimeEnd) {
                    validDates = true;
                }
                else new ArgumentException("Booking date format exception. Booking start date exceeds booking end date");
            }
            else {
                throw new ArgumentException("Booking date format exception. Booking dates preceding current date");
            }
            return validDates;
        }

        private int FindLowestAvailableNumber(int[] array) {
            Array.Sort(array);

            int lowestAvailableNumber = 1;

            foreach (var number in array) {
                if (number == lowestAvailableNumber) {
                    lowestAvailableNumber++;
                }
                else if (number > lowestAvailableNumber) {
                    break; // Found the lowest available number
                }
            }

            return lowestAvailableNumber;
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

            return result.ToArray();
        }
    }
}
