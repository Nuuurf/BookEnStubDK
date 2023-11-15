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
    public class CreateBookingTest
    {
        [Test]
        public async Task CreateBooking_ReturnsNewBookingId_WithNewBooking()
        {
            //Arrange
            var mockDBBokking = new Mock<IDBBooking>();

            DateTime bookingStart = DateTime.Now.AddDays(1);
            DateTime bookingEnd = DateTime.Now.AddDays(1).AddHours(1);
            Booking booking = new Booking();
            booking.TimeStart = bookingStart;
            booking.TimeEnd = bookingEnd;

            mockDBBokking.Setup(repo => repo.CreateBooking(It.IsAny<Booking>())).ReturnsAsync(1);

            BookingDataControl controller = new BookingDataControl(mockDBBokking.Object);

            //Act   
            var result = await controller.CreateBooking(booking);

            //Assert
            Assert.AreEqual(1, result);
        }

        [Test]
        public async Task CreateBooking_ReturnsZero_WithPriorDate()
        {
            //Arrange
            var mockDBBokking = new Mock<IDBBooking>();

            DateTime bookingDate = DateTime.Now.AddDays(-1);
            Booking booking = new Booking();
            booking.TimeStart = bookingDate;

            mockDBBokking.Setup(repo => repo.CreateBooking(It.IsAny<Booking>())).ReturnsAsync(0);

            BookingDataControl controller = new BookingDataControl(mockDBBokking.Object);

            //Act   
            AsyncTestDelegate result = () => controller.CreateBooking(booking);

            //Assert
            Assert.ThrowsAsync<ArgumentException>(result);
        }

        [Test]
        public async Task CreateBooking_ReturnsZero_WithNullBooking()
        {
            //Arrange
            var mockDBBokking = new Mock<IDBBooking>();
            Booking booking = null;

            mockDBBokking.Setup(repo => repo.CreateBooking(It.IsAny<Booking>())).ReturnsAsync(0);
            BookingDataControl controller = new BookingDataControl(mockDBBokking.Object);

            //Act   
            AsyncTestDelegate result = () => controller.CreateBooking(booking);

            //Assert
            Assert.ThrowsAsync<ArgumentNullException>(result);
        }
    }
}
