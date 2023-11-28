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
            var mockDBBooking = new Mock<IDBBooking>();

            DateTime start = DateTime.Now;
            DateTime end = start.AddHours(1);

            List<Booking> bookings = new List<Booking> {
                new Booking {Id = 1, TimeStart = start, TimeEnd = start.AddHours(1), Notes = "Hello" },
                new Booking {Id = 2, TimeStart = start, TimeEnd = start.AddHours(1), Notes = "Olleh" } };

            mockDBBooking.Setup(repo => repo.GetBookingsInTimeslot(null!, start, end, null!)).ReturnsAsync(bookings);

            BookingDataControl controller = new BookingDataControl(mockDBBooking.Object, null!, null!); 

            //Act
            var result = await controller.GetBookingsInTimeslot(start, end);

            //Assert
            Assert.That(result, Is.EqualTo(bookings));
        }


        [Test]
        public async Task GetBookingsInTimeSlot_ReturnsNull_WithInvalidDates()
        {
            //Arrange
            DateTime start = DateTime.Now;
            DateTime end = start.AddHours(-1);
            
            BookingDataControl controller = new BookingDataControl(null!, null!, null!);

            //Act
            var result = await controller.GetBookingsInTimeslot(start, end);

            //Assert
            Assert.Null(result);
        }

        [Test]
        public async Task GetBookingsInTimeSlot_ReturnsNull_WithDuplicateDates()
        {
            //Arrange
            DateTime start = DateTime.Now;

            BookingDataControl controller = new BookingDataControl(null!, null!, null!);

            //Act
            var result = await controller.GetBookingsInTimeslot(start, start);

            //Assert
            Assert.Null(result);
        }
    }
}
