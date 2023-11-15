using Dapper;
using Moq;
using NUnit.Framework;
using RestfulApi.BusinessLogic;
using RestfulApi.DAL;
using RestfulApi.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.API.Controller {
    public class CreateMultipleBookingTest {

        [TearDown] 
        public void TearDown() {
            using(SqlConnection con = DBConnection.Instance.GetOpenConnection()) {
                con.Execute("Delete from Booking where Notes = 'Delete me please'");
            }
        }



        [Test]
        public async Task CreateMultipleBooking_ShouldBeTrue() {
            //Arrange
            BookingDataControl bdc = new BookingDataControl(new DBBooking());

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
            BookingDataControl bdc = new BookingDataControl(new DBBooking());

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
            BookingDataControl bdc = new BookingDataControl(new DBBooking());

            List<Booking> bookings = new List<Booking>();
                    //Make a list that is bigger than the max limit of stubs available at one time
            int maxStubLimit = new DBBooking(DBConnection.Instance.GetOpenConnection()).GetMaxStubs().Result;
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
