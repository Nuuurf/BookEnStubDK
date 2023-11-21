using Microsoft.AspNetCore.Mvc;
using RestfulApi.DAL;
using RestfulApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
            AsyncTestDelegate result;
            // Act & Assert
            try
            {
                using (var transaction = conn.BeginTransaction())
                {
                    int bookingOrderID = await _dbBooking.CreateNewBookingOrder(conn, transaction);
                    int stubCount = new Utilities().GetMaxDBStubs();
                    for (int i = 0; i < stubCount; i++)
                    {
                        await _dbBooking.CreateBooking(conn, overlappingBooking, bookingOrderID, transaction);
                    }

                    await _dbBooking.CreateBooking(conn, overlappingBooking, bookingOrderID, transaction);
                    transaction.Rollback();

                    Assert.Fail("Expected an SqlException to be thrown.");
                }
            }
            catch (SqlException ex)
            {
                Assert.AreEqual("No available stubs for timeslot: Nov 10 2020  9:00AM/Nov 10 2020 10:00AM", ex.Message);
            }
        }


    }
}
