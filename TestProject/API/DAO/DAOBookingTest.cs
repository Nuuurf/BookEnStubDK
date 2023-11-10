using RestfulApi.Models;
using NUnit.Framework;
using System;
using System.Diagnostics;
using RestfulApi.DAL;

namespace TestProject.API.DAO {
    public class DAOBookingTest {

        private static Booking booking1 = new Booking {
            StartTime = DateTime.Now,
            EndTime = DateTime.Now.AddHours(1),
            Notes = "Some generic notes",
            CustomerID = 1,
            StubID = 1,
        };

        private static Booking booking2 = new Booking {
            StartTime = DateTime.Now,
            EndTime = DateTime.Now.AddHours(1),
            Notes = "Some generic notes",
            CustomerID = 2,
            StubID = 1,
        };

        private static Booking booking3 = new Booking {
            StartTime = DateTime.Now.AddDays(1),
            EndTime = DateTime.Now.AddDays(1).AddHours(1),
            Notes = "Some generic notes",
            CustomerID = 3,
            StubID = 2,
        };

        //private string dBConnectionString = "Connection";

        private static IDBBooking _dbBooking = new DBBooking();

        // Use a static property or method that NUnit can access for the ValueSource
        public static IEnumerable<Booking> TestBookings {
            get {
                yield return booking1;
                yield return booking2;
                yield return booking3;
            }
        }


        [SetUp]
        public void Setup() {
        }

        [Test]
        public void Test1() {
            Assert.Pass();
        }

        [Test]
        public void CreateBooking_ShouldReturnTrueIfValidInterval([ValueSource(nameof(TestBookings))] Booking inBooking) {
            Assert.True(_dbBooking.CreateBooking(inBooking));
        }
    }
}
