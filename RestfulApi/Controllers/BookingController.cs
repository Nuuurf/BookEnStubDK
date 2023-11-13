using Microsoft.AspNetCore.Mvc;
using RestfulApi.DAL;
using RestfulApi.Models;
using System.Data.SqlClient;

namespace RestfulApi.Controllers {

    [ApiController]
    [Route("[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IDBBooking _DBBooking;

        public BookingController(IDBBooking dbBooking)
        {
            _DBBooking = dbBooking;
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

        // URL: api/booking
        [HttpGet]
        public IActionResult GetBookingsInTimeslot(DateTime start, DateTime end) {
            try {
                List<Booking> bookingList = _DBBooking.GetBookingsInTimeslot(start, end);
                if (bookingList == null || bookingList.Count == 0) {
                    return NotFound("No bookings found.");
                }

                return Ok(bookingList);
            } catch (Exception ex) {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // URL: api/booking
        [HttpPost]
        public IActionResult CreateBooking(Booking booking) {
            try {
                bool bookingSuccess = _DBBooking.CreateBooking(booking);
                if (bookingSuccess == false) {
                    return BadRequest("Booking not created.");
                }

                return Ok(bookingSuccess);
            } catch (Exception ex) {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
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