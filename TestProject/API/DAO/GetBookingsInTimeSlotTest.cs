using RestfulApi.DAL;
using RestfulApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using TestProject.API.Utilities;

namespace TestProject.API.DAO
{
    public class GetBookingsInTimeSlotTest
    {
        //change fourth integer for hour change
        private static DateTime[] DateRange1 = new DateTime[] { new DateTime(2023, 11, 10, 9, 0, 0), new DateTime(2023, 11, 10, 10, 0, 0) };
        private static DateTime[] DateRange2 = new DateTime[] { new DateTime(2023, 11, 10, 10, 0, 0), new DateTime(2023, 11, 10, 11, 0, 0) };
        private static DateTime[] DateRange3 = new DateTime[] { new DateTime(2023, 11, 10, 11, 0, 0), new DateTime(2023, 11, 10, 12, 0, 0) };

        //private string dBConnectionString = "Connection";

        private static IDBBooking _dbBooking = new DBBooking();
        private static IDbConnection _dbConnection = DBConnection.Instance.GetOpenConnection();

        // Use a static property or method that NUnit can access for the ValueSource
        public static IEnumerable<DateTime[]> TestDates
        {
            get
            {
                yield return DateRange1;
                yield return DateRange2;
                yield return DateRange3;
            }
        }
        [Test]
        public async Task GetBookingWithinTimeslot_ShouldOnlyRetrieveOnesWithinTimeslot([ValueSource(nameof(TestDates))] DateTime[] inDates)
        {

            // Arrange
            var start = inDates[0];
            var end = inDates[1];
            const int expectedBookingCount = 3; // Set your expected count here
SearchBookingsFilters filters = new SearchBookingsFilters();

            TestContext.WriteLine($"Testing timeslot: Start - {start}, End - {end}");

            // Act
            var bookings = await _dbBooking.GetBookingsInTimeslot(_dbConnection, start, end, filters);

            // Additional Information
            TestContext.WriteLine($"Number of bookings returned: {bookings.Count}");
            TestContext.WriteLine($"Expected number of bookings: {expectedBookingCount}");

            // Assert
            foreach (var booking in bookings)
            {
                TestContext.WriteLine($"Booking ID: {booking.Id}, Start Time: {booking.TimeStart}, End Time: {booking.TimeEnd}");
                Assert.GreaterOrEqual(booking.TimeStart, start, $"Booking ID {booking.Id} start time should be within the timeslot");
                Assert.LessOrEqual(booking.TimeEnd, end, $"Booking ID {booking.Id} end time should be within the timeslot");
            }

            // Check if the number of bookings matches the expected count
            Assert.That(bookings.Count, Is.EqualTo(expectedBookingCount), "The number of bookings returned does not match the expected count");
        }
        [Test]
        public async Task GetBookingWithinTimeslot_ShouldReturnEmptyListWhenNoBookings()
        {
            // Arrange for a timeslot where you're sure there are no bookings
            var start = new DateTime(8002, 11, 10, 15, 0, 0);
            var end = new DateTime(8002, 11, 10, 16, 0, 0);

            // Act
            var bookings = await _dbBooking.GetBookingsInTimeslot(_dbConnection, start, end, new SearchBookingsFilters());

            // Assert
            Assert.IsEmpty(bookings);
        }
        [Test]
        public async Task GetBookingWithStubIdFilter_ShouldRetrieveCorrectBookings()
        {
            // Arrange
            var start = new DateTime(2023, 11, 10, 9, 0, 0);
            var end = new DateTime(2023, 11, 10, 12, 0, 0);
            int expectedStubId = 1; // Example stub ID
            SearchBookingsFilters filters = new SearchBookingsFilters { StubId = expectedStubId };

            // Act
            var bookings = await _dbBooking.GetBookingsInTimeslot(_dbConnection, start, end, filters);

            // Assert
            foreach (var booking in bookings)
            {
                Assert.That(booking.StubId, Is.EqualTo(expectedStubId), $"Booking ID {booking.Id} should have the stub ID {expectedStubId}");
            }
        }
        }
}
