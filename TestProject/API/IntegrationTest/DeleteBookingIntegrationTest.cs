using RestfulApi.BusinessLogic;
using RestfulApi.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestfulApi.Controllers;
using TestProject.API.Utilities;
using Dapper;
using RestfulApi.DTOs;
using RestfulApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace TestProject.API.IntegrationTest
{
public class DeleteBookingIntegrationTest
    {
        private static IDbConnection _connection = DBConnection.Instance.GetOpenConnection();
        private static IDBCustomer _customer = new DBCustomer();
        private static IDBBooking _dBBooking = new DBBooking();
        private static ICustomerData _customerControl = new CustomerDataControl(_customer, _connection);
        private static IBookingData _mockBookingData = new BookingDataControl(_dBBooking, _customerControl, _connection);
        private static BookingController _controller = new BookingController(_mockBookingData);
        private static int _bookingId;

        [SetUp]
        public void SetUp()
        {
            string script = "Delete from booking where notes = 'Delete Bookings For Delete Integration Test'";

            _connection.Execute(script);
                string scriptInsert = @"
                INSERT INTO Booking (TimeStart, TimeEnd, StubId, Notes, BookingOrderID) 
                VALUES ('2023-12-08T15:00:00', '2023-12-08T16:00:00', 1, 'Delete Bookings For Delete Integration Test', 1);
                SELECT SCOPE_IDENTITY();";

                _bookingId = _connection.ExecuteScalar<int>(scriptInsert);
        }

        [Test]
        public async Task DeleteBookingIntegrationTest_ShouldReturnTrue_WithValidID()
        {
            //Arrange
            int bookingID = _bookingId;

            //Act
var result = await _controller.DeleteBooking(bookingID);

            //Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var success = okResult.Value as bool?;
            Assert.IsTrue(success);
        }
    }
}
