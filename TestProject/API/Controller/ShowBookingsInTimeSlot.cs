using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RestfulApi.Controllers;
using RestfulApi.DAL;
using RestfulApi.Models;
using RestfulApi.BusinessLogic;
using System.Collections;

namespace TestProject.API.Controller
{
    public class ShowBookingsInTimeSlot
    {
        
        /*
         * The Following Tests tests searching for available bookings
         */
        [Test]
        public async Task ShowBookingsInTimeSlot_ReturnsAvailableBookings_WithValidDate()
        {
            //Arrange
            DateTime date = DateTime.Now.Date;

            BookingRequestFilter filter = new BookingRequestFilter
            {
                Start = date, End = date.AddDays(1), ShowAvailable = true
            };
            var mockBusinessBooking = new Mock<IBookingData>();
            mockBusinessBooking.Setup(repo => repo.GetAvailableStubsForGivenTimeFrame(date, date.AddDays(1))).ReturnsAsync(new List<AvailableStubsForHour>());

            BookingController controller = new BookingController(mockBusinessBooking.Object);


            //Act
            var result = await controller.ShowBookingsInTimeSlot(filter);

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task ShowBookingsInTimeSlot_ReturnsNotFound_WithPriorDate()
        {
            //Arrange
            DateTime date = DateTime.Now.Date.AddDays(-1);
            BookingRequestFilter filter = new BookingRequestFilter
            {
                Start = date, ShowAvailable = true
            };
            List<AvailableStubsForHour> nullList = null!;

            var mockBusinessBooking = new Mock<IBookingData>();
            mockBusinessBooking.Setup(repo => repo.GetAvailableStubsForGivenTimeFrame(date, date.AddDays(1))).ReturnsAsync(nullList);

            BookingController controller = new BookingController(mockBusinessBooking.Object);

            //Act
            var result = await controller.ShowBookingsInTimeSlot(filter);

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        /*
         * The Following Tests tests searching for existing bookings
         */
        [Test]
        public async Task ShowBookingsInTimeSlot_ReturnsNotFound_WithNoBookings()
        {
            //Arrange
            DateTime start = DateTime.Now;
            DateTime end = start.AddHours(1);
            BookingRequestFilter filter = new BookingRequestFilter
            {
                Start = start, End = end, ShowAvailable = false
            };
            var mockBusinessBooking = new Mock<IBookingData>();
            mockBusinessBooking.Setup(repo => repo.GetBookingsInTimeslot(filter)).ReturnsAsync(new List<Booking>());

            BookingController controller = new BookingController(mockBusinessBooking.Object);

            //Act
            var result = await controller.ShowBookingsInTimeSlot(filter);

            //Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task ShowBookingsInTimeSlot_ReturnsOK_WithBooking()
        {
            //Arrange
            var mockBusinessBooking = new Mock<IBookingData>();
            DateTime start = DateTime.Now;
            DateTime end = start.AddHours(1);
            BookingRequestFilter filter = new BookingRequestFilter
            {
                Start = start, End = end, ShowAvailable = false
            };
            List<Booking> bookings = new List<Booking> {
                new Booking {Id = 1, TimeStart = start, TimeEnd = start.AddHours(1), Notes = "Hello" },
                new Booking {Id = 2, TimeStart = start, TimeEnd = start.AddHours(1), Notes = "Olleh" } };

            mockBusinessBooking.Setup(repo => repo.GetBookingsInTimeslot(filter)).ReturnsAsync(bookings);

            BookingController controller = new BookingController(mockBusinessBooking.Object);

            //Act
            var result = await controller.ShowBookingsInTimeSlot(filter) as OkObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.That(result?.Value, Is.EqualTo(bookings));
            Assert.That(result?.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task ShowBookingsInTimeSlot_ReturnsBadRequest_WithNoEndDate()
        {
            //Arrange
            BookingController controller = new BookingController(null!);
            BookingRequestFilter filter = new BookingRequestFilter
            {
                Start = DateTime.Now, ShowAvailable = false
            };
            //Act
            var result = await controller.ShowBookingsInTimeSlot(filter);

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task ShowBookingsInTimeSlot_ReturnsBadRequest_WithInvalidDates()
        {
            //Arrange
            DateTime start = DateTime.Now;
            List<Booking> nullList = null!;
            BookingRequestFilter filter = new BookingRequestFilter
            {
                Start = start, End = start.AddHours(-1), ShowAvailable = false
            };
            var mockBusinessBooking = new Mock<IBookingData>();
            mockBusinessBooking.Setup(repo => repo.GetBookingsInTimeslot(filter)).ReturnsAsync(nullList);

            BookingController controller = new BookingController(mockBusinessBooking.Object);

            //Act
            var result = await controller.ShowBookingsInTimeSlot(filter) as BadRequestObjectResult;

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }
        
        [Test]
        public async Task ShowBookingsInTimeSlot_ThrowsException_InternalError()
        {
            //Arrange
            DateTime start = DateTime.Now;
            BookingRequestFilter filter = new BookingRequestFilter
            {
                Start = start, End = start.AddHours(-1), ShowAvailable = false
            };
            var mockBusinessBooking = new Mock<IBookingData>();
            mockBusinessBooking.Setup(repo => repo.GetBookingsInTimeslot(filter)).Throws(new Exception());

            BookingController controller = new BookingController(mockBusinessBooking.Object);

            //Act
            var resultTask = controller.ShowBookingsInTimeSlot(filter);
            var result = await resultTask;

            //Assert
            if (result is ObjectResult objectResult)
            {
                Assert.That(objectResult.StatusCode, Is.EqualTo(500));
            }
            Assert.IsInstanceOf<ObjectResult>(result);
        }
    }
}   
