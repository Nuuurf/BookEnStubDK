using Microsoft.AspNetCore.Mvc;
using RestfulApi.BusinessLogic;
using RestfulApi.DAL;
using RestfulApi.Models;
using System.Data.SqlClient;
using System.Globalization;

namespace RestfulApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingData _bookingdata;

        public BookingController(IBookingData bookingData)
        {
            _bookingdata = bookingData;
        }

        /*
                // URL: api/booking
                [HttpPut]
                public IActionResult UpdateBooking([FromBody] Booking updatedBooking) {
                    try {
                        bool success = _DBBooking.UpdateBooking(updatedBooking);
                        if (success == false) {
                            return NotFound("Booking not updated");
                        }
                        return Ok(success);
                    } catch (Exception ex) {
                        return StatusCode(500, $"Internal Server Error: {ex.Message}");
                    }
                }

                // URL: api/booking
                [HttpGet("id")]
                public IActionResult GetSingleBooking(int id) {
                    try {
                        Booking booking = _DBBooking.GetSingleBooking(id);
                        if (booking == null) {
                            return NotFound("No booking found.");
                        }

                        return Ok(booking);
                    } catch (Exception ex) {
                        return StatusCode(500, $"Internal Server Error: {ex.Message}");
                    }
                }*/

        //URL: api/booking
        [HttpGet]
        public async Task<IActionResult> GetBookingsInTimeslot(DateTime start, DateTime end)
        {
            try
            {
                List<Booking> bookingList = await _bookingdata.GetBookingsInTimeslot(start, end);
                if (bookingList == null || bookingList.Count == 0)
                {
                    return NotFound("No bookings found.");
                }

                return Ok(bookingList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("/{date}")]
        public async Task<IActionResult> GetBookingsForGivenDate(string date)
        {
            DateTime dateFormatted;
            bool isValidDate = DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateFormatted);
            if (!isValidDate)
            {
                return BadRequest("Invalid date format. Please use YYYY-MM-DD format.");
            }

            try
            {
                List<AvailableBookingsForTimeframe> avaiableList = await _bookingdata.GetAvailableBookingsForGivenDate(dateFormatted);

                if (avaiableList == null)
                {
                    return BadRequest("Placed date is prior to today");
                }

                return Ok(avaiableList);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // URL: api/booking
        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] Booking? booking = null)
        {
            if (booking != null)
            {
                try
                {

                    int newBookingId = await _bookingdata.CreateBooking(booking);

                    if (newBookingId != 0)
                    {
                        return Ok(newBookingId);
                    }
                    else
                    {
                        //No stubs are available, status might have changed from last opdate of UI
                        //Added translations for the lulz
                        return UnprocessableEntity("DA: Alle stubbe for denne tidsperiode er optaget \n EN: All stubs for this period are unavailable");
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal Server Error: {ex.Message}");
                }
            }
            else {
                return BadRequest("No JSON object were transmitted with request");
            }
        }

        [HttpPost]
        [Route("/Booking/Multiple")]
        public async Task<IActionResult> CreateMultipleBooking([FromBody] List<Booking>? bookings = null) {
            if (bookings != null) {
                try {

                    bool success = await _bookingdata.CreateMultipleBookings(bookings);

                    if (success) {
                        return Ok(success);
                    }
                    else {
                        return UnprocessableEntity("DA: kunne ikke oprette bookinger, kapasitet oversteget med en eller flere bookinger");
                    }
                }
                catch (Exception ex) {
                    return StatusCode(500, $"Internal Server Error: {ex.Message}");
                }
            }
            else {
                return BadRequest("No JSON objects were transmitted with request");
            }
        }

        /*
                // URL: api/booking
                [HttpDelete("{id}")]
                public IActionResult DeleteBooking(int id) {
                    try {
                        bool bookingSuccess = _DBBooking.DeleteBooking(id);
                        if (bookingSuccess == false) {
                            return NotFound("Booking not deleted");
                        }
                        return Ok(bookingSuccess);
                    } catch (Exception ex) {
                        return StatusCode(500, $"Internal Server Error: {ex.Message}");
                    }
                }*/
    }
}