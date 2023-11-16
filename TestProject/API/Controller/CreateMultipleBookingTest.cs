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
using TestProject.API.DAO;

namespace TestProject.API.Controller {
    public class CreateMultipleBookingTest
    {

        private IDbConnection _connection;
        [SetUp]
        public void Setup()
        {
            _connection = DBConnection.Instance.GetOpenConnection();
        }
        [TearDown] 
        public void TearDown() {
            using(IDbConnection con = _connection) {
                con.Execute("Delete from Booking where Notes = 'Delete me please'");
            }
_connection.Close();
        }



        [Test]
        public async Task CreateMultipleBooking_ShouldBeTrue() {
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
        public async Task CreateMultipleBooking_ShouldBeFalse() {
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
        public async Task CreateMultipleBooking_ShouldFailDueToTimeslotOverload() {
            //Arrange
            BookingDataControl bdc = new BookingDataControl(new DBBooking(), _connection);

            List<Booking> bookings = new List<Booking>();
                    //Make a list that is bigger than the max limit of stubs available at one time
                    DBBooking db= new DBBooking();
                    int maxStubLimit = await db.GetMaxStubs(_connection);
            for(int i = 0; i < maxStubLimit + 1; i++) {
                bookings.Add(new Booking { TimeStart = DateTime.Now, TimeEnd = DateTime.Now.AddHours(1), Notes = "Delete me please" });
            } 

            //Act
            bool result = await bdc.CreateMultipleBookings(bookings);

            //Assert
            Assert.IsFalse(result);
        }

    }
}
