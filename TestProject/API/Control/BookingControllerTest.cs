using Dapper;
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

namespace TestProject.API.Control {

    public class BookingControllerTest {

        private IDbConnection _connection;


        [SetUp]
        public void SetUp()
        {
            _connection = DBConnection.Instance.GetOpenConnection();
        }
        [TearDown]
        public void TearDown() {
            using (IDbConnection con = _connection) {
                string script = "Delete from booking where notes = 'Delete me please'";

                con.Execute(script);
            }
            _connection.Close();
        }

        [Test]
        public async Task CreateBooking_ShouldBePassValue()
        {
            //Arrange
            BookingDataControl bdc = new BookingDataControl(new DBBooking(), _connection);

            Booking booking = new Booking
            { //booking to test with
                Notes = "Delete me please",
                TimeStart = DateTime.Now.AddHours(1),
                TimeEnd = DateTime.Now.AddHours(2),
            };

            int createReturn = 0;

            //Act
            createReturn = await bdc.CreateBooking(booking);


            //Assert
            Assert.GreaterOrEqual(createReturn, 0);
        }

        [Test]
        public async Task CreateBooking_ShouldThrowArgumentException()
        {
            //Arrange
            BookingDataControl bdc = new BookingDataControl(new DBBooking(), _connection);

            Booking booking = new Booking
            { //booking to test with
                Notes = "Delete me please",
                //instantiate date before the current time
                TimeStart = DateTime.Now.AddHours(-2),
                TimeEnd = DateTime.Now.AddHours(-1),
            };

            //Act
            AsyncTestDelegate act = () => bdc.CreateBooking(booking);

            //Assert
            Assert.ThrowsAsync<ArgumentException>(act);
        }
    }
}
