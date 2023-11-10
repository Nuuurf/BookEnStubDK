using Microsoft.AspNetCore.Mvc;
using RestfulApi.DAL;
using RestfulApi.Models;
using System.Data.SqlClient;

namespace RestfulApi.Controllers {

    [ApiController]
    [Route("[controller]")]
    public class BookingController : ControllerBase {

        private readonly DBBooking _DBBooking;


        public BookingController() {
            _DBBooking = new DBBooking();
        }

        /* URL: api/bookings
        [HttpGet]
        public IActionResult GetBooking() {
            try {
                List<Booking> bookingList = _DBBooking.GetBookings();
                if (bookingList == null || bookingList.Count == 0) {
                    return NotFound("No bookings found.");
                }

                return Ok(bookingList);
            } catch (Exception ex) {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }*/

        // URL: api/booking
        [HttpPost]
        public IActionResult CreateBooking(Booking booking) {
            try {
                bool bookingSuccess = _DBBooking.CreateBooking(booking);
                if (bookingSuccess == false) {
                    return NotFound("Booking not created");
                }

                return Ok(bookingSuccess);
            } catch (Exception ex) {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}