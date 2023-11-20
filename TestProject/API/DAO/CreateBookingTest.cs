using RestfulApi.DAL;
using RestfulApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.API.DAO
{
public class CreateBookingTest
    {
        private static IDBBooking _dbBooking = new DBBooking();
        private static Booking booking1 = new Booking
        {
            TimeStart = new DateTime(2023, 11, 10, 9, 0, 0),
            TimeEnd = new DateTime(2023, 11, 10, 10, 0, 0),
            Notes = "Some generic notes",
            StubId = 1
            //CustomerID = 1,
            //StubID = 1,
        };

        private static Booking booking2 = new Booking
        {
            TimeStart = new DateTime(2023, 11, 10, 10, 0, 0),
            TimeEnd = new DateTime(2023, 11, 10, 11, 0, 0),
            Notes = "Some generic notes",
            StubId = 2
            //CustomerID = 2,
            //StubID = 1,
        };

        private static Booking booking3 = new Booking
        {
            TimeStart = new DateTime(2023, 11, 10, 11, 0, 0),
            TimeEnd = new DateTime(2023, 11, 10, 12, 0, 0),
            Notes = "Some generic notes",
            StubId = 3
            //CustomerID = 3,
            //StubID = 2,
        };
        public static IEnumerable<Booking> TestBookings
        {
            get
            {
                yield return booking1;
                yield return booking2;
                yield return booking3;
            }
        }
        [Test]
        public async Task CreateBooking_ShouldReturnTrueIfValidInterval([ValueSource(nameof(TestBookings))] Booking inBooking)
        {
            IDbConnection conn = DBConnection.Instance.GetOpenConnection();
            int bookingID = 0;
            using (var transactions = conn.BeginTransaction())
            {
                bookingID = await _dbBooking.CreateBooking(conn, inBooking,1, transactions);
                transactions.Rollback();
            }
            Assert.True(bookingID > 0);
        }
        [Test]
        public async Task CreateBooking_ShouldFailForTooManyBookings()
        {
            // Arrange
            var overlappingBooking = new Booking
            {
                TimeStart = new DateTime(2020, 11, 10, 9, 0, 0),
                TimeEnd = new DateTime(2020, 11, 10, 10, 0, 0),
                Notes = "Overlapping booking",
            };

            IDbConnection conn = DBConnection.Instance.GetOpenConnection();
            int bookingID = 1;

            // Act
            using (var transaction = conn.BeginTransaction())
            {
                int stubCount = await _dbBooking.GetMaxStubs(conn, transaction);
                for (int i = 0; i < stubCount; i++)
                {
                    await _dbBooking.CreateBooking(conn, overlappingBooking, 1, transaction);
                }
bookingID = await _dbBooking.CreateBooking(conn, overlappingBooking,1, transaction);
                transaction.Rollback();
            }

            // Assert
            Assert.True(bookingID == 0, "Overlapping bookings should not be allowed");
        }


    }
}
