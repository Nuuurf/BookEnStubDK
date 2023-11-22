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

namespace TestProject.API.Controller
{
    public class GetBookingsInTimeSlotTest
    {
        [Test]
        public async Task GetBookingsInTimeSlot_ReturnsNotFound_WithNoBookings()
        {
            //Arrange
            var mockDBBokking = new Mock<IBookingData>();
            mockDBBokking.Setup(repo => repo.GetBookingsInTimeslot(It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(new List<Booking>());

            BookingController controller = new BookingController(mockDBBokking.Object);

            //Act
            DateTime start = DateTime.Now;
            DateTime end = start.AddHours(1);

            var result = await controller.GetBookingsInTimeslot(start, end);

            //Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task GetBookingsInTimeSlot_ReturnsOK_WithBooking()
        {
            //Arrange
            var mockDBBokking = new Mock<IBookingData>();
            DateTime currentTime = DateTime.Now;
            List<Booking> bookings = new List<Booking> {
                new Booking {Id = 1, TimeStart = currentTime, TimeEnd = currentTime.AddHours(1), Notes = "Hello" },
                new Booking {Id = 2, TimeStart = currentTime, TimeEnd = currentTime.AddHours(1), Notes = "Olleh" } };

            mockDBBokking.Setup(repo => repo.GetBookingsInTimeslot(It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(bookings);

            BookingController controller = new BookingController(mockDBBokking.Object);

            //Act
            DateTime start = DateTime.Now;
            DateTime end = start.AddHours(1);

            var result = await controller.GetBookingsInTimeslot(start, end) as OkObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.That(result.Value, Is.EqualTo(bookings));
            Assert.That(result.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task GetBookingsInTimeSlot_ReturnsNotFound_WithInvalidDates()
        {
            //Arrange
            var mockDBBokking = new Mock<IBookingData>();

            List<Booking> nullList = null!;
            mockDBBokking.Setup(repo => repo.GetBookingsInTimeslot(It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(nullList);

            BookingController controller = new BookingController(mockDBBokking.Object);

            //Act
            DateTime start = DateTime.Now;
            DateTime end = start.AddHours(-1);

            var result = await controller.GetBookingsInTimeslot(start, end);

            //Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task GetBookingsInTimeSlot_ThrowsException_InternalError()
        {
            //Arrange
            var mockDBBokking = new Mock<IBookingData>();

            mockDBBokking.Setup(repo => repo.GetBookingsInTimeslot(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Throws(new Exception());

            BookingController controller = new BookingController(mockDBBokking.Object);

            //Act
            DateTime start = DateTime.Now;
            DateTime end = start.AddHours(-1);

            var resultTask = controller.GetBookingsInTimeslot(start, end);
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
