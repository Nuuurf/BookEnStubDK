using Castle.Components.DictionaryAdapter.Xml;
using RestfulApi;
using RestfulApi.DAL;
using RestfulApi.Models;

namespace RestfulApi.BusinessLogic {
    public class BookingDataControl : IBookingData{

        private IDBBooking _dBBooking;

        public BookingDataControl(IDBBooking dbBooking) {
            //Needs to change with injection
            _dBBooking = dbBooking;
        }

        public async Task<int> CreateBooking(Booking booking)
        {
            DateTime currentDate = DateTime.Now;
            if(booking != null && booking.TimeStart < currentDate)
            {
                throw new ArgumentException("Booking date format exception. Booking start date exceeds booking end date");
            }

            int newBookingId = 0;

            newBookingId = await _dBBooking.CreateBooking(booking);
            return newBookingId;
        }


        public async Task<List<AvailableBookingsForTimeframe>> GetAvailableBookingsForGivenDate(DateTime date)
        {
            DateTime currentDate = DateTime.Now.Date;

            if(date < currentDate)
            {
                return null;
            }
            
            List<AvailableBookingsForTimeframe> availableBookings = await _dBBooking.GetAvaiableBookingsForGivenDate(date);
            
            return availableBookings;
        }

        public async Task<List<Booking>> GetBookingsInTimeslot(DateTime start, DateTime end)
        {
            if(start > end)
            {
                return null;
            }

            List<Booking> bookings = await _dBBooking.GetBookingsInTimeslot(start, end);

            return bookings;
        }
    }
}
