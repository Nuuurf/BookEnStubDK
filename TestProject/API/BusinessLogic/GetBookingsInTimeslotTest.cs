using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RestfulApi.BusinessLogic;
using RestfulApi.Controllers;
using RestfulApi.DAL;
using RestfulApi.Models;

namespace TestProject.API.BusinessLogic
{
    public class GetBookingsInTimeslotTest
    {
        [Test]
        public async Task GetBookingsInTimeSlot_ReturnsBookings_WithDates()
        {
            //Arrange
            var mockDBBokking = new Mock<IDBBooking>();

            DateTime currentTime = DateTime.Now;

            List<Booking> bookings = new List<Booking> {
                new Booking {Id = 1, TimeStart = currentTime, TimeEnd = currentTime.AddHours(1), Notes = "Hello" },
                new Booking {Id = 2, TimeStart = currentTime, TimeEnd = currentTime.AddHours(1), Notes = "Olleh" } };

            mockDBBokking.Setup(repo => repo.GetBookingsInTimeslot(null, It.IsAny<DateTime>(), It.IsAny<DateTime>(), null)).ReturnsAsync(bookings);

            BookingDataControl controller = new BookingDataControl(mockDBBokking.Object, null); 

            //Act
            DateTime start = DateTime.Now;
            DateTime end = start.AddHours(1);

            var result = await controller.GetBookingsInTimeslot(start, end);

            //Assert
            Assert.AreEqual(bookings, result);
        }


        [Test]
        public async Task GetBookingsInTimeSlot_ReturnsNull_WithInvalidDates()
        {
            //Arrange
            var mockDBBokking = new Mock<IDBBooking>();
            List<Booking> bookings = null;

            mockDBBokking.Setup(repo => repo.GetBookingsInTimeslot(null, It.IsAny<DateTime>(), It.IsAny<DateTime>(), null)).ReturnsAsync(bookings);

            BookingDataControl controller = new BookingDataControl(mockDBBokking.Object, null);

            //Act
            DateTime start = DateTime.Now;
            DateTime end = start.AddHours(-1);

            var result = await controller.GetBookingsInTimeslot(start, end);

            //Assert
            Assert.Null(result);
        }
    }
}
