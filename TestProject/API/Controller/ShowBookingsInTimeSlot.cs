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
using WebApplicationMVC.Models;
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
        public async Task ShowBookingsInTimeSlow_ReturnsAvailableBookings_WithValidDate()
        {
            //Arrange
            DateTime date = DateTime.Now.Date;
            var mockBusinessBooking = new Mock<IBookingData>();
            mockBusinessBooking.Setup(repo => repo.GetAvailableBookingsForGivenDate(date)).ReturnsAsync(new List<AvailableBookingsForTimeframe>());

            BookingController controller = new BookingController(mockBusinessBooking.Object);


            //Act
            var result = await controller.ShowBookingsInTimeSlot(date, null, true);

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task ShowBookingsInTimeSlow_ReturnsNotFound_WithPriorDate()
        {
            //Arrange
            DateTime date = DateTime.Now.Date.AddDays(-1);
            List<AvailableBookingsForTimeframe> nullList = null!;

            var mockBusinessBooking = new Mock<IBookingData>();
            mockBusinessBooking.Setup(repo => repo.GetAvailableBookingsForGivenDate(date)).ReturnsAsync(nullList);

            BookingController controller = new BookingController(mockBusinessBooking.Object);

            //Act
            var result = await controller.ShowBookingsInTimeSlot(date, null, true);

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

            var mockBusinessBooking = new Mock<IBookingData>();
            mockBusinessBooking.Setup(repo => repo.GetBookingsInTimeslot(start, end, It.IsAny<SearchBookingsFilters>())).ReturnsAsync(new List<Booking>());

            BookingController controller = new BookingController(mockBusinessBooking.Object);

            //Act
            var result = await controller.ShowBookingsInTimeSlot(start, end, false);

            //Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task ShowBookingsInTimeSlot_ReturnsOK_WithBooking()
        {
            //Arrange
            var mockBusinessBooking = new Mock<IBookingData>();
DateTime start = DateTime.Now;
            DateTime end = start.AddHours(1);

            List<Booking> bookings = new List<Booking> {
                new Booking {Id = 1, TimeStart = start, TimeEnd = start.AddHours(1), Notes = "Hello" },
                new Booking {Id = 2, TimeStart = start, TimeEnd = start.AddHours(1), Notes = "Olleh" } };

            mockBusinessBooking.Setup(repo => repo.GetBookingsInTimeslot(start, end, It.IsAny<SearchBookingsFilters>())).ReturnsAsync(bookings);

            BookingController controller = new BookingController(mockBusinessBooking.Object);

            //Act
            var result = await controller.ShowBookingsInTimeSlot(start, end, false) as OkObjectResult;

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

            //Act
            DateTime start = DateTime.Now;

            var result = await controller.ShowBookingsInTimeSlot(start, null, false);

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task ShowBookingsInTimeSlot_ReturnsBadRequest_WithInvalidDates()
        {
            //Arrange
            DateTime start = DateTime.Now;
            DateTime end = start.AddHours(-1);
            List<Booking> nullList = null!;

            var mockBusinessBooking = new Mock<IBookingData>();
            mockBusinessBooking.Setup(repo => repo.GetBookingsInTimeslot(start, end, null!)).ReturnsAsync(nullList);

            BookingController controller = new BookingController(mockBusinessBooking.Object);

            //Act
            var result = await controller.ShowBookingsInTimeSlot(start, end, false) as BadRequestObjectResult;

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }
        
        [Test]
        public async Task ShowBookingsInTimeSlot_ThrowsException_InternalError()
        {
            //Arrange
            DateTime start = DateTime.Now;
            DateTime end = start.AddHours(+1);
            var mockBusinessBooking = new Mock<IBookingData>();
            mockBusinessBooking.Setup(repo => repo.GetBookingsInTimeslot(start, end, It.IsAny<SearchBookingsFilters>()))
                .Throws(new Exception());

            BookingController controller = new BookingController(mockBusinessBooking.Object);

            //Act
            

            var resultTask = controller.ShowBookingsInTimeSlot(start, end, false);
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
