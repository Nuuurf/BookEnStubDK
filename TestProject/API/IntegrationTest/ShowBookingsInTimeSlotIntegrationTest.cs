using RestfulApi.BusinessLogic;
using RestfulApi.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestfulApi.Controllers;
using RestfulApi.Models;
using TestProject.API.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace TestProject.API.IntegrationTest
{
    public class ShowBookingsInTimeSlotIntegrationTest
    {
        private static IDbConnection _connection = DBConnection.Instance.GetOpenConnection();
        private static IDBCustomer _customer = new DBCustomer();
        private static IDBBooking _dBBooking = new DBBooking();
        private static ICustomerData _customerControl = new CustomerDataControl(_customer, _connection);
        private static IBookingData _mockBookingData = new BookingDataControl(_dBBooking, _customerControl, _connection);
        private static BookingController _controller = new BookingController(_mockBookingData);

        [Test]
        public async Task ShowBookingsInTimeSlow_ShouldReturnBookings_WithValidData()
        {
            //Arrange
            BookingRequestFilter filter = new BookingRequestFilter
            {
Start = new DateTime(2023, 11, 10, 09,00,00),
End = new DateTime(2023, 11, 10, 12,00,00),
ShowAvailable = false,
        OrderId = 1,
        StubId = 1,
        CustomerEmail = "testperson@test.test",
        CustomerPhone = "73277327"
            };

        //Act
        var result = await _controller.ShowBookingsInTimeSlot(filter);

            //Assert
            Assert.IsNotNull(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));

            var bookings = okResult.Value as List<Booking>;
            Assert.IsNotNull(bookings);
            Assert.That(bookings.Count, Is.EqualTo(3));
            foreach (var booking in bookings)
            {
                // Example assertions based on filter criteria
                Assert.IsTrue(booking.TimeStart >= filter.Start && booking.TimeEnd <= filter.End);
                Assert.That(booking.Customer.Email, Is.EqualTo(filter.CustomerEmail));
                Assert.That(booking.Customer.Phone, Is.EqualTo(filter.CustomerPhone));
                // Add more checks as needed based on your Booking and BookingRequestFilter structure
            }
        }

        [Test]
        public async Task ShowBookingsInTimeSlow_ShouldReturnAvailableBookings_WithValidData()
        {
            //Arrange
            BookingRequestFilter filter = new BookingRequestFilter
            {
                Start = DateTime.Now.Date.AddDays(1),
                ShowAvailable = true,
                };

            //Act
            var result = await _controller.ShowBookingsInTimeSlot(filter);

            //Assert
            Assert.IsNotNull(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));

            var bookings = okResult.Value as List<AvailableStubsForHour>;
            Assert.IsNotNull(bookings);
            Assert.That(bookings.Count, Is.EqualTo(13));

            Assert.That(bookings[0].AvailableStubIds.Count, Is.EqualTo(10));
        }
    }
}
