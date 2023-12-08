using Microsoft.AspNetCore.Mvc;
using RestfulApi.BusinessLogic;
using RestfulApi.DAL;
using RestfulApi.Models;
using System.Data.SqlClient;
using System.Globalization;
using RestfulApi.DTOs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using RestfulApi.Exceptions;

namespace RestfulApi.Controllers {

    [ApiController]
    [Route("[controller]")]
    public class BookingController : ControllerBase {
        private readonly IBookingData _bookingdata;

        public BookingController(IBookingData bookingData) {
            _bookingdata = bookingData;
        }

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
                            = await _bookingdata.GetAvailableStubsForGivenTimeFrame(req);
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
                    return NoContent();
                }

                return Ok(bookingList);
            }
            catch (Exception ex) {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // URL: api/booking
        [HttpPost]
        public async Task<IActionResult> CreateBooking(BookingRequest? booking = null)
        {
            if (booking == null)
            {
                return BadRequest("No JSON object were transmitted with request");
            }

            List<Booking> bookings = DTO.ConvertToBookingList(booking.Appointments);
            Customer customer = DTO.ConvertToCustomer(booking.Customer);

                try
                {
                    int newBookingId = await _bookingdata.CreateBooking(bookings, customer);
                    return Ok(newBookingId);
                }
                catch (OverBookingException)
                {
                    return UnprocessableEntity("DA: Alle stubbe for denne tidsperiode er optaget \n EN: All stubs for this period are unavailable");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal Server Error: {ex.Message}");
                }
            }

        // URL: api/booking
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id) {
            try {
                bool bookingSuccess = await _bookingdata.DeleteBooking(id);
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