using RestfulApi;
using RestfulApi.DAL;
using RestfulApi.Models;

namespace RestfulApi.BusinessLogic {
    public class BookingDataControl : IBookingData{

        private DBBooking _dBBooking;

        public BookingDataControl() {
            //Needs to change with injection
            _dBBooking = new DBBooking();
        }

        public bool CreateBooking(Booking booking) {
            bool success = false;
            bool possibleDates = false;

            List<Booking> bookings = _dBBooking.GetBookingsInTimeslot(booking.TimeStart, booking.TimeEnd);

            //Change to non hardcoded value when stored procedure has implemented a way of getting this value from database.
            int maxStubs = 8;
            int[] stubNumbers = new int[maxStubs];
            int lowestAvailableStubNumber = -1;

            //Check if dates are in the correct format and are possible
            if(booking.TimeStart >= DateTime.Now.AddMinutes(-1) &&  booking.TimeEnd > DateTime.Now) {
                if (booking.TimeStart < booking.TimeEnd) {
                    possibleDates = true;
                }
                else new ArgumentException("Booking date format exception. Booking start date exceeds booking end date");
            }
            else {
                throw new ArgumentException("Booking date format exception. Booking dates preceding current date");
            }

            if(bookings.Count < maxStubs && possibleDates) {
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
                //there is no more available stubs for this timeperiod.
                return success = false;
            }

            return success;
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
    }
}
