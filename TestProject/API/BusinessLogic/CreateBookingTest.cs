using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
            var mockDBConnection = new Mock<IDbConnection>();

            mockDBBooking.Setup(repo => repo.CreateBooking(
                    It.IsAny<IDbConnection>(),
                    It.IsAny<Booking>(),
                    It.IsAny<int>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(1); // Assuming the method returns an int (e.g., the new booking ID)

            mockDBBooking.Setup(repo =>
                    repo.CreateNewBookingOrder(It.IsAny<IDbConnection>(),
                        It.IsAny<IDbTransaction>()))
                .ReturnsAsync(1);

            mockDBConnection.Setup(repo => repo.BeginTransaction());
            BookingDataControl controller = new BookingDataControl(mockDBBooking.Object, mockDBConnection.Object);

            DateTime bookingStart = DateTime.Now.AddDays(1);
            DateTime bookingEnd = bookingStart.AddHours(1);
            Booking booking = new Booking { TimeStart = bookingStart, TimeEnd = bookingEnd };

            // Act
            var result = await controller.CreateBooking(booking);

            // Assert
            Assert.AreEqual(1, result);
        }


        [Test]
        public async Task CreateBooking_ThrowsArgumentException_WithPriorDate()
        {
            //Arrange
            var mockDBBokking = new Mock<IDBBooking>();
            DateTime bookingDate = DateTime.Now.AddDays(-1);
            Booking booking = new Booking();
            booking.TimeStart = bookingDate;

            mockDBBokking.Setup(repo => repo.CreateBooking(
                    It.IsAny<IDbConnection>(),
                    It.IsAny<Booking>(),
                    It.IsAny<int>(), It.IsAny<IDbTransaction>()))
                .ReturnsAsync(1);


            BookingDataControl controller = new BookingDataControl(mockDBBokking.Object, null);

            //Act   
            AsyncTestDelegate result = () => controller.CreateBooking(booking);

            //Assert
            Assert.ThrowsAsync<ArgumentException>(result);
        }

        [Test]
        public async Task CreateBooking_ThrowsArgumentNullException_WithNullBooking()
        {
            //Arrange
            var mockDBBokking = new Mock<IDBBooking>();
            Booking booking = null;
            mockDBBokking.Setup(repo => repo.CreateBooking(
                    It.IsAny<IDbConnection>(),
                    It.IsAny<Booking>(),
                    It.IsAny<int>(), It.IsAny<IDbTransaction>()))
                .ReturnsAsync(1);
            BookingDataControl controller = new BookingDataControl(mockDBBokking.Object, null);

            //Act   
            AsyncTestDelegate result = () => controller.CreateBooking(booking);

            //Assert
            Assert.ThrowsAsync<ArgumentNullException>(result);
        }

        [Test]
        public async Task CreateBooking_ThrowsArgumentException_WithEndDateEarlierThanStart()
        {
            //Arrange
            var mockDBBokking = new Mock<IDBBooking>();
            Booking booking = new Booking() { TimeStart = DateTime.Now.AddDays(2), TimeEnd = DateTime.Now.AddDays(1) };

            mockDBBokking.Setup(repo => repo.CreateBooking(
                    It.IsAny<IDbConnection>(),
                    It.IsAny<Booking>(),
                    It.IsAny<int>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(1);
            BookingDataControl controller = new BookingDataControl(mockDBBokking.Object, null);

            //Act   
            AsyncTestDelegate result = () => controller.CreateBooking(booking);

            //Assert
            Assert.ThrowsAsync<ArgumentException>(result);
        }

[Test]        
        public async Task CreateBooking_ThrowsException_WithNoAvailbableStubsForBooking()
        {
            //Arrange
            var mockDBBokking = new Mock<IDBBooking>();
            Booking booking = new Booking { TimeStart = DateTime.Now.AddDays(1), TimeEnd = DateTime.Now.AddDays(1).AddHours(1) };

            mockDBBokking.Setup(repo => repo.CreateBooking(
                    It.IsAny<IDbConnection>(),
                    It.IsAny<Booking>(),
                    It.IsAny<int>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(0);
            BookingDataControl controller = new BookingDataControl(mockDBBokking.Object, null);

            //Act   
            AsyncTestDelegate result = () => controller.CreateBooking(booking);

            //Assert
            Assert.ThrowsAsync<Exception>(result);
        }

        [Test]
        public async Task CreateBooking_DatabaseThrowsException()
        {
            //Arrange
            var mockDBBokking = new Mock<IDBBooking>();
            Booking booking = new Booking { TimeStart = DateTime.Now.AddDays(1), TimeEnd = DateTime.Now.AddDays(1).AddHours(1) };

            mockDBBokking.Setup(repo => repo.CreateBooking(
                    It.IsAny<IDbConnection>(),
                    It.IsAny<Booking>(),
                    It.IsAny<int>(),
                    It.IsAny<IDbTransaction>()))
                .ThrowsAsync(new Exception("Sql Exception"));
            BookingDataControl controller = new BookingDataControl(mockDBBokking.Object, null);

            //Act   
            AsyncTestDelegate result = () => controller.CreateBooking(booking);

            //Assert
            Assert.ThrowsAsync<Exception>(result);
        }

    }
}
