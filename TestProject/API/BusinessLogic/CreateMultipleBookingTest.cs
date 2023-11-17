using Dapper;
using Moq;
using NUnit.Framework;
using RestfulApi.BusinessLogic;
using RestfulApi.DAL;
using RestfulApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NuGet.Frameworks;
using TestProject.API.DAO;

namespace TestProject.API.BusinessLogic
{
    public class CreateMultipleBookingTest
    {

        private IDbConnection _connection;
        [SetUp]
        public void Setup()
        {
            _connection = DBConnection.Instance.GetOpenConnection();
        }
        [TearDown]
        public void TearDown()
        {
            using (IDbConnection con = _connection)
            {
                con.Execute("Delete from Booking where Notes = 'Delete me please'");
            }
            _connection.Close();
        }



        [Test]
        public async Task CreateMultipleBooking_ShouldBeTrue()
        {
            //Arrange
            BookingDataControl bdc = new BookingDataControl(new DBBooking(), _connection);

            List<Booking> bookings = new List<Booking> {
                new Booking{TimeStart = DateTime.Now, TimeEnd = DateTime.Now.AddHours(1), Notes = "Delete me please"},
                new Booking{TimeStart = DateTime.Now, TimeEnd = DateTime.Now.AddHours(1), Notes = "Delete me please"},
                new Booking{TimeStart = DateTime.Now, TimeEnd = DateTime.Now.AddHours(1), Notes = "Delete me please"},
            };

            //Act
            bool result = await bdc.CreateMultipleBookings(bookings);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task CreateMultipleBooking_ShouldBeFalse()
        {
            //Arrange
            BookingDataControl bdc = new BookingDataControl(new DBBooking(), _connection);

            List<Booking> bookings = new List<Booking> {
                new Booking{TimeStart = DateTime.Now, TimeEnd = DateTime.Now.AddHours(1), Notes = "Delete me please"},
                new Booking{TimeStart = DateTime.Now, TimeEnd = DateTime.Now.AddHours(-1), Notes = "Delete me please"},
                new Booking{TimeStart = DateTime.Now, TimeEnd = DateTime.Now.AddHours(1), Notes = "Delete me please"},
            };

            //Act
            bool result = await bdc.CreateMultipleBookings(bookings);

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task CreateMultipleBooking_ShouldFailDueToTimeslotOverload()
        {
            //Arrange
            BookingDataControl bdc = new BookingDataControl(new DBBooking(), _connection);

            List<Booking> bookings = new List<Booking>();
            //Make a list that is bigger than the max limit of stubs available at one time
            DBBooking db = new DBBooking();
            int maxStubLimit = await db.GetMaxStubs(_connection);
            for (int i = 0; i < maxStubLimit + 1; i++)
            {
                bookings.Add(new Booking { TimeStart = DateTime.Now, TimeEnd = DateTime.Now.AddHours(1), Notes = "Delete me please" });
            }

            //Act
            bool result = await bdc.CreateMultipleBookings(bookings);

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task CreateMultipleBookings_ShouldReturnFalse_WithInvalidDates()
        {
            //Arrange
            var mockDBBokking = new Mock<IDBBooking>();
            List<Booking> bookingList = new List<Booking> {new Booking{TimeStart = DateTime.Now.AddHours(2), TimeEnd = DateTime.Now.AddHours(1)} };
            mockDBBokking.Setup(repo => repo.CreateMultipleBookings(It.IsAny<IDbConnection>(),
                    It.IsAny<List<List<Booking>>>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(false);
            BookingDataControl controller = new BookingDataControl(mockDBBokking.Object, null);

            //Act
var result = await controller.CreateMultipleBookings(bookingList);
        //Assert
Assert.IsFalse(result);
        }

        [Test]
        public async Task CreateMultipleBookings_ShouldReturnTrue_WithBookingsAlreadyOnDate()
        {
            // Arrange
            var dBBookingMock = new Mock<IDBBooking>();
            var connectionMock = new Mock<IDbConnection>();
            var transactionMock = new Mock<IDbTransaction>();

            var testBookings = new List<Booking>
            {
                new Booking { TimeStart = DateTime.Now.AddHours(1),
                    TimeEnd = DateTime.Now.AddHours(2) },
                // Add more Booking objects to simulate various scenarios
            };

            var existingBookings = new List<Booking>
            {
                new Booking
                {
                    TimeStart = DateTime.Now.AddHours(1),
                    TimeEnd = DateTime.Now.AddHours(2),
                    StubId = 1
                }
            };
            int maxStubs = 10; // Assume enough stubs are available

            dBBookingMock.Setup(repo => repo.GetBookingsInTimeslot(It.IsAny<IDbConnection>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<IDbTransaction>()))
                .ReturnsAsync(existingBookings);

            dBBookingMock.Setup(repo => repo.GetMaxStubs(It.IsAny<IDbConnection>(), It.IsAny<IDbTransaction>()))
                .ReturnsAsync(maxStubs);

            dBBookingMock.Setup(repo =>
                    repo.CreateMultipleBookings(It.IsAny<IDbConnection>(), It.IsAny<List<List<Booking>>>(), It.IsAny<IDbTransaction>()))
                .ReturnsAsync(true);
            connectionMock.Setup(repo => repo.BeginTransaction(It.IsAny<System.Data.IsolationLevel>())).Returns(transactionMock.Object);
            var service = new BookingDataControl(dBBookingMock.Object, connectionMock.Object);

            // Act
            bool success = await service.CreateMultipleBookings(testBookings);

            // Assert
            foreach (var booking in testBookings)
            {
                Assert.IsTrue(booking.StubId > 0); // Check if StubId is assigned
            }
            Assert.IsTrue(success);
        }
        [Test]
        public async Task CreateMultipleBookings_ShouldReturnFalse_WithFullBookingsAlreadyOnDate()
        {
            // Arrange
            var dBBookingMock = new Mock<IDBBooking>();
            var connectionMock = new Mock<IDbConnection>();
            var transactionMock = new Mock<IDbTransaction>();
            DateTime timeStart = DateTime.Now.AddHours(1);
            DateTime timeEnd = DateTime.Now.AddHours(2);

            var testBookings = new List<Booking>{new Booking { TimeStart = timeStart, TimeEnd = timeEnd },};
            
            var existingBookings = new List<Booking>
            {
                new Booking{ TimeStart = timeStart, TimeEnd = timeEnd, StubId = 1 },
                new Booking{ TimeStart = timeStart, TimeEnd = timeEnd, StubId = 2 },
                new Booking{ TimeStart = timeStart, TimeEnd = timeEnd, StubId = 3 }
            };
            int maxStubs = 3; 

            dBBookingMock.Setup(repo => repo.GetBookingsInTimeslot(It.IsAny<IDbConnection>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<IDbTransaction>()))
                .ReturnsAsync(existingBookings);

            dBBookingMock.Setup(repo => repo.GetMaxStubs(It.IsAny<IDbConnection>(), It.IsAny<IDbTransaction>()))
                .ReturnsAsync(maxStubs);
            dBBookingMock.Setup(repo =>
                    repo.CreateMultipleBookings(It.IsAny<IDbConnection>(), It.IsAny<List<List<Booking>>>(), It.IsAny<IDbTransaction>()))
                .ReturnsAsync(false);
            connectionMock.Setup(repo => repo.BeginTransaction(It.IsAny<System.Data.IsolationLevel>())).Returns(transactionMock.Object);
            var service = new BookingDataControl(dBBookingMock.Object, connectionMock.Object);

            // Act
            bool success = await service.CreateMultipleBookings(testBookings);

            // Assert
            foreach (var booking in testBookings)
            {
                Assert.IsTrue(booking.StubId == 0); // Check if StubId is assigned
            }
            Assert.IsFalse(success);
        }
    }
}
