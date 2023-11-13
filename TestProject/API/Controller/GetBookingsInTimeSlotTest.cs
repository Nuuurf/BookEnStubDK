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

namespace TestProject.API.Controller
{
    public class GetBookingsInTimeSlotTest
    {


        [Test]
        public void GetBookingsInTimeSlot_ReturnsNotFound_WithNoBookings()
        {
            //Arrange
            var mockDBBokking = new Mock<IDBBooking>();
            mockDBBokking.Setup(repo => repo.GetBookingsInTimeslot(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(new List<Booking>());

            BookingController controller = new BookingController(mockDBBokking.Object);

            //Act
            DateTime start = DateTime.Now;
            DateTime end = start.AddHours(1);

            var result = controller.GetBookingsInTimeslot(start, end);

            //Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public void GetBookingsInTimeSlot_ReturnsOK_WithBooking()
        {
            //Arrange
            var mockDBBokking = new Mock<IDBBooking>();
            DateTime currentTime = DateTime.Now;
            List<Booking> bookings = new List<Booking> {
                new Booking {Id = 1, TimeStart = currentTime, TimeEnd = currentTime.AddHours(1), Notes = "Hello" },
                new Booking {Id = 2, TimeStart = currentTime, TimeEnd = currentTime.AddHours(1), Notes = "Olleh" } };

            mockDBBokking.Setup(repo => repo.GetBookingsInTimeslot(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(bookings);

            BookingController controller = new BookingController(mockDBBokking.Object);

            //Act
            DateTime start = DateTime.Now;
            DateTime end = start.AddHours(1);

            var result = controller.GetBookingsInTimeslot(start, end) as OkObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(bookings, result.Value);
            Assert.AreEqual(200, result.StatusCode);
        }
    }
}
