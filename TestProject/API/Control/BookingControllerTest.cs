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
using TestProject.API.Utilities;

namespace TestProject.API.Control
{

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
            BookingDataControl bdc = new BookingDataControl(new DBBooking(),new CustomerDataControl(new DBCustomer(),_connection), _connection);

            List<Booking> booking = new List<Booking>
            { //booking to test with
                new Booking {Notes = "Delete me please",
                    TimeStart = DateTime.Now.AddHours(1),
                    TimeEnd = DateTime.Now.AddHours(2),}
                };

            Customer customer = new Customer { FullName = "First Name", Email = "Fake Email", Phone = "1234567890", };
            int createReturn = 0;

            //Act
            createReturn = await bdc.CreateBooking(booking, customer);


            //Assert
            Assert.GreaterOrEqual(createReturn, 0);
        }

        [Test]
        public Task CreateBooking_ShouldThrowArgumentException()
        {
            //Arrange
            BookingDataControl bdc = new BookingDataControl(new DBBooking(), new CustomerDataControl(new DBCustomer(), _connection), _connection);

            List<Booking> booking = new List<Booking>
            { //booking to test with
                new Booking
                {
                    Notes = "Delete me please",
                    //instantiate date before the current time
                    TimeStart = DateTime.Now.AddHours(-2),
                    TimeEnd = DateTime.Now.AddHours(-1),
                }
                };
            Customer customer = new Customer { FullName = "First Name", Email = "Fake Email", Phone = "1234567890", };
            //Act
            AsyncTestDelegate act = () => bdc.CreateBooking(booking, customer);

            //Assert
            Assert.ThrowsAsync<ArgumentException>(act);

            return Task.CompletedTask;
        }
    }
}
