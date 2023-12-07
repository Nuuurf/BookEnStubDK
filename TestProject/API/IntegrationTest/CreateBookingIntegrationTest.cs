using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using RestfulApi.BusinessLogic;
using RestfulApi.Controllers;
using RestfulApi.DAL;
using RestfulApi.DTOs;
using RestfulApi.Models;
using TestProject.API.Utilities;

namespace TestProject.API.IntegrationTest
{
    public class CreateBookingIntegrationTest
    {
        private IDbConnection _connection;

        private readonly BookingRequest _bookingRequest = new BookingRequest
        {
            Appointments = new List<DTONewBooking>
            {
                new DTONewBooking
                {
                    Notes = "Delete me please",
                    TimeStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 15, 0, 0).AddDays(1),
                    TimeEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 16, 0, 0).AddDays(1) // One hour after TimeStart
                }
            },
            Customer = new DTOCustomer
            {
                FullName = "Banana Joe",
                Email = "Joe@Banana.com",
                Phone = "1234"
            }
        };

        [SetUp]
        public void SetUp()
        {
            _connection = DBConnection.Instance.GetOpenConnection();
        }
        [TearDown]
        public void TearDown()
        {
            using (IDbConnection con = _connection)
            {
                string script = "Delete from booking where notes = 'Delete me please'";

                con.Execute(script);
            }
            _connection.Close();
        }

                [Test]
        public async Task CreateBooking_AssociateWithCustomer_IntegrationTest()
        {
            // Arrange
            IDBCustomer _customer = new DBCustomer();
            IDBBooking _dBBooking = new DBBooking();
            ICustomerData _customerControl = new CustomerDataControl(_customer, _connection);
            IBookingData mockBookingData = new BookingDataControl(_dBBooking, _customerControl, _connection);
            BookingController controller = new BookingController(mockBookingData);

            // Act
            var resultTask = controller.CreateBooking(_bookingRequest);
            IActionResult result = await resultTask;

            //Assert
            if (result is ObjectResult objectResult)
            {
                Assert.That(objectResult.StatusCode, Is.EqualTo(200));
            }
            Assert.IsInstanceOf<ObjectResult>(result);
        }
    }
}
