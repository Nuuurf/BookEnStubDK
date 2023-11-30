using Microsoft.AspNetCore.Mvc;
using RestfulApi.BusinessLogic;
using RestfulApi.DAL;
using RestfulApi.Models;
using System.Data.SqlClient;
using System.Globalization;
using RestfulApi.DTOs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace RestfulApi.Controllers {

    [ApiController]
    [Route("[controller]")]
    public class BookingController : ControllerBase {
        private readonly IBookingData _bookingdata;

        public BookingController(IBookingData bookingData) {
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
        public async Task<IActionResult> ShowBookingsInTimeSlot([FromQuery] BookingRequestFilter req)
        {
            try
            {
                if (req.ShowAvailable == true)
                {
                    if (req.End == null)
                    {
                        req.End = req.Start.AddDays(1);
                    }
                    List<AvailableStubsForHour> availableList
                            = await _bookingdata.GetAvailableStubsForGivenTimeFrame(req.Start, req.End.Value);
                        if (availableList == null!)
                        {
                            return BadRequest("End date is prior to start date");
                        }
                        return Ok(availableList);

                }

                if (req.End == null)
                {
                    return BadRequest("Must provide an end date");
                }
                List<Booking> bookingList = await _bookingdata.GetBookingsInTimeslot(req);

                if (bookingList == null!)
                {
                    return BadRequest("End date must be after start date.");
                }
                if (bookingList.Count == 0) {
                    return NotFound("No bookings found.");
                }

                return Ok(bookingList);
            }
            catch (Exception ex) {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // URL: api/booking
        [HttpPost]
        public async Task<IActionResult> CreateBooking(BookingRequest? booking = null) {
            //Console.WriteLine(booking.Customer.FullName.ToString());
            // Handle Customer association with BookingOrder here
            if (booking != null) {
                
                //Convert from dto to internal model class
                List<Booking> bookings = DTO.ConvertToBookingList(booking.Appointments);

                Customer customer = DTO.ConvertToCustomer(booking.Customer);
                

                try {

                    int newBookingId = await _bookingdata.CreateBooking(bookings, customer);

                    if (newBookingId != 0) {
                        return Ok(newBookingId);
                    }
                    else {
                        //No stubs are available, status might have changed from last opdate of UI
                        //Added translations for the lulz
                        return UnprocessableEntity("DA: Alle stubbe for denne tidsperiode er optaget \n EN: All stubs for this period are unavailable");
                    }
                }
                catch (Exception ex) {
                    return StatusCode(500, $"Internal Server Error: {ex.Message}");
                }
            }
            else {
                return BadRequest("No JSON object were transmitted with request");
            }
        }


        // URL: api/booking
        [HttpDelete("{id}")]
        public IActionResult DeleteBooking(int id) {
            try {
                bool bookingSuccess = _bookingdata.DeleteBooking(id).Result;
                if(bookingSuccess) {
                    return Ok(bookingSuccess);
                }
                else {
                    //Could not find any booking with the id. Answer unprocessable entity
                    return UnprocessableEntity("Could not find booking with that id");
                }
            }
            catch (Exception ex) {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}