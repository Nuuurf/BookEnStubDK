﻿using RestfulApi.Models;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Builder;
using RestfulApi.DAL;

namespace TestProject.API.DAO {
    public class DAOBookingTest {

        private static Booking booking1 = new Booking {
            TimeStart = new DateTime(2023, 11, 10, 9, 0, 0),
            TimeEnd = new DateTime(2023, 11, 10, 10, 0, 0),
            Notes = "Some generic notes",
            StubId = 1
            //CustomerID = 1,
            //StubID = 1,
        };

        private static Booking booking2 = new Booking {
        TimeStart = new DateTime(2023, 11, 10, 10, 0, 0),
        TimeEnd = new DateTime(2023, 11, 10, 11, 0, 0),
        Notes = "Some generic notes",
        StubId = 2
            //CustomerID = 2,
            //StubID = 1,
        };

        private static Booking booking3 = new Booking {
            TimeStart = new DateTime(2023, 11, 10, 11, 0, 0),
            TimeEnd = new DateTime(2023, 11, 10, 12, 0, 0),
            Notes = "Some generic notes",
            StubId = 3
            //CustomerID = 3,
            //StubID = 2,
        };

        //change fourth integer for hour change
        private static DateTime[] DateRange1 = new DateTime[] { new DateTime(2023, 11, 10, 9, 0, 0), new DateTime(2023, 11, 10, 10, 0, 0) };
        private static DateTime[] DateRange2 = new DateTime[] { new DateTime(2023, 11, 10, 10, 0, 0), new DateTime(2023, 11, 10, 11, 0, 0) };
        private static DateTime[] DateRange3 = new DateTime[] { new DateTime(2023, 11, 10, 11, 0, 0), new DateTime(2023, 11, 10, 12, 0, 0) };

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

        public static IEnumerable<DateTime[]> TestDates {
            get {
                yield return DateRange1;
                yield return DateRange2;
                yield return DateRange3;
            }
        }

        [SetUp]
        public void Setup() {

            //List<Booking> bookings = (List<Booking>)TestBookings;

            for (int i = 0; i < TestBookings.Count(); i++) {
                string script = "Insert into booking (TimeStart, TimeEnd, Notes) values (@TimeStart, @TimeEnd, @Notes)";

                using (SqlConnection con = DBConnection.Instance.GetOpenConnection()) {

                    con.Execute(script, TestBookings);

                }
            }

        }

        [TearDown]
        public async Task TearDown() {
            using (SqlConnection con = DBConnection.Instance.GetOpenConnection()) {
                string script = "Delete from booking where notes = 'Some generic notes'";

                await con.ExecuteAsync(script);
            }
        }

        [Test]
        public async Task CreateBooking_ShouldReturnTrueIfValidInterval([ValueSource(nameof(TestBookings))] Booking inBooking)
        {
            Assert.True(await _dbBooking.CreateBooking(DBConnection.Instance.GetOpenConnection(), inBooking) > 0);
        }

        [Test]
        public async Task GetBookingWithinTimeslot_ShouldOnlyRetrieveOnesWithinTimeslot([ValueSource(nameof(TestDates))] DateTime[] inDates)
        {

            // Arrange
            var start = inDates[0];
            var end = inDates[1];
            const int expectedBookingCount = 3; // Set your expected count here

            TestContext.WriteLine($"Testing timeslot: Start - {start}, End - {end}");

            // Act
            var bookings = await _dbBooking.GetBookingsInTimeslot(DBConnection.Instance.GetOpenConnection(),start, end);

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
            Assert.AreEqual(expectedBookingCount, bookings.Count, "The number of bookings returned does not match the expected count");
        }

        //Backup test :3
        //[Test]
        //public void BookingsInTimeslots_PassedShouldMatchFetched([ValueSource(nameof(TestDates))] DateTime[] inDates) {
        //    //Arrange
        //    var start = inDates[0];
        //    var end = inDates[1];

        //    var bookings = _dbBooking.GetBookingsInTimeslot(start, end);

        //    int fetched = bookings.Count;
        //    int passed = 0;
        //    int forIndex = 0;
        //    string forIndexInformation = "";

        //    //Act
        //    foreach (var booking in bookings) {
        //        forIndexInformation += $"{forIndex} - booking - start: {booking.TimeStart} - end: {booking.TimeEnd} \n";
        //        forIndex++;
        //        if (booking.TimeStart >= start && booking.TimeEnd <= end) {
        //            passed++;
        //        } 
        //    }

        //    //Assert
        //    TestContext.WriteLine(forIndexInformation);
        //    TestContext.WriteLine($"Testing timeslot: Start - {start}, End - {end}");
        //    TestContext.WriteLine($"Number of test which should pass: {fetched} - Actually passed {passed}");

        //    Assert.AreEqual(fetched, passed);
        //}

    }
}
