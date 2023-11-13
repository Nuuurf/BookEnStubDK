using Dapper;
using RestfulApi.BusinessLogic;
using RestfulApi.DAL;
using RestfulApi.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.API.Control {

    public class BookingControllerTest {

        [TearDown]
        public void TearDown() {
            using (SqlConnection con = DBConnection.Instance.GetOpenConnection()) {
                string script = "Delete from booking where notes = 'Delete me please'";

                con.Execute(script);
            }
        }

        [Test]
        public void CreateBooking_ShouldBeTrue() {
            //Arrange
            BookingDataControl bdc = new BookingDataControl();

            
            Booking booking = new Booking { //booking to test with
                Notes = "Delete me please",
                TimeStart = DateTime.Now,
                TimeEnd = DateTime.Now.AddHours(1),
            };

            bool createReturn = false;

            //Act
            createReturn = bdc.CreateBooking(booking);

            //Assert
            Assert.IsTrue(createReturn);
        }

        [Test]
        public void CreateBooking_ShouldThrowArgumentException() {
            //Arrange
            BookingDataControl bdc = new BookingDataControl();


            Booking booking = new Booking { //booking to test with
                Notes = "Delete me please",
                //instantiate date before the current time
                TimeStart = DateTime.Now.AddHours(-2),
                TimeEnd = DateTime.Now.AddHours(-1),
            };

            //Act
            TestDelegate act = () => bdc.CreateBooking(booking);

            //Assert
            Assert.Throws<ArgumentException>(act);
        }

    }
}
