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
            var mockDbTransaction = new Mock<IDbTransaction>();

            mockDBBooking.Setup(repo => repo.CreateBooking(
                    It.IsAny<IDbConnection>(),
                    It.IsAny<Booking>(),
                    It.IsAny<int>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(1); 

            mockDBBooking.Setup(repo =>
                    repo.CreateNewBookingOrder(It.IsAny<IDbConnection>(),
                        It.IsAny<IDbTransaction>()))
                .ReturnsAsync(1);

            // Setup mock transaction behavior
            mockDBConnection.Setup(conn => conn.BeginTransaction(It.IsAny<IsolationLevel>()))
                .Returns(mockDbTransaction.Object);
            mockDbTransaction.Setup(trans => trans.Commit());
            mockDbTransaction.Setup(trans => trans.Rollback());

            BookingDataControl controller = new BookingDataControl(mockDBBooking.Object, mockDBConnection.Object);

            DateTime bookingStart = DateTime.Now.AddDays(1);
            DateTime bookingEnd = bookingStart.AddHours(1);
            List<Booking> booking = new List<Booking> {new Booking{ TimeStart = bookingStart, TimeEnd = bookingEnd} };

            // Act
            var result = await controller.CreateBooking(booking);

            // Assert
            Assert.AreEqual(1, result);

            mockDbTransaction.Verify(trans => trans.Commit(), Times.Once);
            mockDbTransaction.Verify(trans => trans.Rollback(), Times.Never);
        }


        [Test]
        public async Task CreateBooking_ThrowsArgumentException_WithPriorDate()
        {
            //Arrange
            var mockDBBokking = new Mock<IDBBooking>();
            DateTime bookingDate = DateTime.Now.AddDays(-1);
            List<Booking> booking = new List<Booking> { new Booking { TimeStart = bookingDate} };

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
            List<Booking> booking = null;
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
            List<Booking> booking = new List<Booking> { new Booking { TimeStart = DateTime.Now.AddDays(2), TimeEnd = DateTime.Now.AddDays(1) } };
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
            var mockDBBooking = new Mock<IDBBooking>();
            var mockDBConnection = new Mock<IDbConnection>();
            var mockDbTransaction = new Mock<IDbTransaction>();
            List<Booking> booking = new List<Booking> { new Booking { TimeStart = DateTime.Now.AddDays(1), TimeEnd = DateTime.Now.AddDays(1).AddHours(1)} };

            mockDBBooking.Setup(repo => repo.CreateBooking(
                    It.IsAny<IDbConnection>(),
                    It.IsAny<Booking>(),
                    It.IsAny<int>(),
                    It.IsAny<IDbTransaction>()))
                .Throws<Exception>();
            // Setup mock transaction behavior
            mockDBConnection.Setup(conn => conn.BeginTransaction(It.IsAny<IsolationLevel>()))
                .Returns(mockDbTransaction.Object);
            mockDbTransaction.Setup(trans => trans.Commit());
            mockDbTransaction.Setup(trans => trans.Rollback());


            BookingDataControl controller = new BookingDataControl(mockDBBooking.Object, mockDBConnection.Object);

            //Act   
            AsyncTestDelegate result = () => controller.CreateBooking(booking);

            //Assert
            Assert.ThrowsAsync<Exception>(result);
            mockDbTransaction.Verify(trans => trans.Commit(), Times.Never);
            mockDbTransaction.Verify(trans => trans.Rollback(), Times.Once);
        }

        [Test]
        public async Task CreateBooking_DatabaseThrowsException()
        {
            //Arrange
            var mockDBBooking = new Mock<IDBBooking>();
            var mockDBConnection = new Mock<IDbConnection>();
            var mockDbTransaction = new Mock<IDbTransaction>();

            List<Booking> booking = new List<Booking>
            {
                new Booking { TimeStart = DateTime.Now.AddDays(1), TimeEnd = DateTime.Now.AddDays(1).AddHours(1) }
            };

            mockDBBooking.Setup(repo => repo.CreateBooking(
                    It.IsAny<IDbConnection>(),
                    It.IsAny<Booking>(),
                    It.IsAny<int>(),
                    It.IsAny<IDbTransaction>()))
                .ThrowsAsync(new Exception("Sql Exception"));
            // Setup mock transaction behavior
            mockDBConnection.Setup(conn => conn.BeginTransaction(It.IsAny<IsolationLevel>()))
                .Returns(mockDbTransaction.Object);
            mockDbTransaction.Setup(trans => trans.Commit());
            mockDbTransaction.Setup(trans => trans.Rollback());
            BookingDataControl controller = new BookingDataControl(mockDBBooking.Object, mockDBConnection.Object);

            //Act   
            AsyncTestDelegate result = () => controller.CreateBooking(booking);

            //Assert
            Assert.ThrowsAsync<Exception>(result);
        }

    }
}
