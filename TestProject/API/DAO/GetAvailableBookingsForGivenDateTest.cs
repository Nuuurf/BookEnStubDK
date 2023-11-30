using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NuGet.Frameworks;
using RestfulApi.DAL;
using RestfulApi.Models;
using TestProject.API.Utilities;

namespace TestProject.API.DAO
{
    public class GetAvailableBookingsForGivenDateTest
    {
        /*[Test]
        public async Task GetAvailableBookingsForGivenDate_ShouldReturn119AvailableBookings_WithValidDate()
        {
            //Arrange
            IDbConnection conn = DBConnection.Instance.GetOpenConnection();
IDBBooking booking = new DBBooking();
            int expectedAvailbleBookingCount = 119;
            int expectedTimeSlots = 13;
            int stubCount = new GetMaxStubs().GetMaxDBStubs();
            DateTime date = new DateTime(2023, 11, 10);
            TestContext.WriteLine($"Testing Date {date}");

            // Act
            var bookings = await booking.GetAvailableBookingsForGivenDate(conn, date);
            int availableBookingsForDay = 0;
            foreach (var e in bookings)
            {
                availableBookingsForDay += e.AvailableStubs;
            }

            //Assert
            Assert.That(availableBookingsForDay, Is.EqualTo(expectedAvailbleBookingCount));
            Assert.That(bookings.Count, Is.EqualTo(expectedTimeSlots));
        }*/
    }
}
