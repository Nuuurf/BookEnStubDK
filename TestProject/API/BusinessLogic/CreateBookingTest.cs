using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RestfulApi.BusinessLogic;
using RestfulApi.Controllers;
using RestfulApi.DAL;
using RestfulApi.Models;
using TestProject.API.DAO;

namespace TestProject.API.BusinessLogic
{
    public class CreateBookingTest
    {
        [Test]
        public async Task CreateBooking_ReturnsNewBookingId_WithNewBooking()
        {
            // Arrange
            
            var mockDBBooking = new Mock<IDBBooking>();
            mockDBBooking.Setup(repo => repo.CreateBooking(
                    It.IsAny<IDbConnection>(),
                    It.IsAny<Booking>(), It.IsAny<IDbTransaction>()))
                .ReturnsAsync(1); // Assuming the method returns an int (e.g., the new booking ID)

            BookingDataControl controller = new BookingDataControl(mockDBBooking.Object, null);

            DateTime bookingStart = DateTime.Now.AddDays(1);
            DateTime bookingEnd = bookingStart.AddHours(1);
            Booking booking = new Booking { TimeStart = bookingStart, TimeEnd = bookingEnd };

            // Act
            var result = await controller.CreateBooking(booking);

            // Assert
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

            mockDBBokking.Setup(repo => repo.CreateBooking(
                    It.IsAny<IDbConnection>(),
                    It.IsAny<Booking>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(1);


            BookingDataControl controller = new BookingDataControl(mockDBBokking.Object, null);

            //Act   
            AsyncTestDelegate result = () => controller.CreateBooking(booking);

            //Assert
            Assert.ThrowsAsync<ArgumentException>(result);
        }

        [Test]
        public async Task CreateBooking_ReturnsZero_WithNullBooking()
        {
            //Arrange
            var mockDbConnection = new Mock<IDbConnection>();
            mockDbConnection.Setup(conn => conn.Open()).Verifiable(); // Setup to verify that Open is called
            var mockDBBokking = new Mock<IDBBooking>();
            Booking booking = null;

            mockDBBokking.Setup(repo => repo.CreateBooking(
                    It.IsAny<IDbConnection>(),
                    It.IsAny<Booking>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(1);
            BookingDataControl controller = new BookingDataControl(mockDBBokking.Object, mockDbConnection.Object);

            //Act   
            AsyncTestDelegate result = () => controller.CreateBooking(booking);

            //Assert
            Assert.ThrowsAsync<ArgumentNullException>(result);
        }
    }
}
