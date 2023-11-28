using Microsoft.AspNetCore.Mvc;
using RestfulApi.BusinessLogic;
using RestfulApi.DAL;
using RestfulApi.Models;
using System.Data.SqlClient;
using System.Globalization;
using RestfulApi.DTOs;
using System.ComponentModel.DataAnnotations;

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
        public async Task<IActionResult> ShowBookingsInTimeSlot(DateTime? start, DateTime? end, bool showAvailable)
        {
            if (start == null)
            {
                return BadRequest("Start date is required");
            }
            try
            {
                if (showAvailable == true)
                {
                    DateTime dateFormatted = start.Value.Date;
                    List<AvailableBookingsForTimeframe> availableList = await _bookingdata.GetAvailableBookingsForGivenDate(dateFormatted);

                    if (availableList == null)
                    {
                        return BadRequest("Placed date is prior to today");
                    }

                    return Ok(availableList);
                }

                if (end == null)
                {
                    return BadRequest("Must provide an end date");
                }
                if (start >= end)
                {
                    return BadRequest("End date must be after start date.");
                }

                List<Booking> bookingList = await _bookingdata.GetBookingsInTimeslot(start.Value, end.Value);
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
        
        // URL: api/booking
        [HttpPost]
        public async Task<IActionResult> CreateBooking(BookingRequest? booking = null)
        {
            //Console.WriteLine(booking.Customer.FullName.ToString());
            // Handle Customer association with BookingOrder here
            if (booking != null)
            {
                List<DTONewBooking> dtos = booking.Appointments;
                List<Booking> bookings = dtos.Select(itemA => new Booking {
                    // Map properties from ObjectTypeA to ObjectTypeB
                    TimeStart = itemA.TimeStart,
                    TimeEnd = itemA.TimeEnd,
                    Notes = itemA.Notes,
                }).ToList();

                Customer customer = new Customer {
                    FirstName = booking.Customer.FullName,
                    Phone = booking.Customer.Phone,
                    Email = booking.Customer.Email
                };

                try
                {

                    int newBookingId = await _bookingdata.CreateBooking(bookings, customer);

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