using RestfulApi.BusinessLogic;
using RestfulApi.Controllers;
using RestfulApi.DAL;
using RestfulApi.Models;
using RestfulApi.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.API.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Dapper;

namespace TestProject.API.Demo {
    public class ConcurrencyTest {

        private DBCustomer _dBCustomer1 = new DBCustomer();
        private DBBooking _dBBooking1 = new DBBooking();

        private DBCustomer _dBCustomer2 = new DBCustomer();
        private DBBooking _dBBooking2 = new DBBooking();

        private static DBConnection _connection = DBConnection.Instance;

        private static List<Booking> _bookings = initializeBookings();

        private static Customer _customer1 = new Customer {
            FullName = "Client1",
            Phone = "11111111",
            Email = "client1@concurrency.test"
        };

        private static Customer _customer2 = new Customer {
            FullName = "Client2",
            Phone = "22222222",
            Email = "client2@concurrency.test"
        };

        //insert bookings

        private BookingRequest _bookingRequest1 = new BookingRequest {
            Appointments = DTO.ConvertToDTONewBooking(_bookings),
            Customer = DTO.ConvertToDTOCustomer(_customer1)
        };

        private BookingRequest _bookingRequest2 = new BookingRequest {
            Appointments = DTO.ConvertToDTONewBooking(_bookings),
            Customer = DTO.ConvertToDTOCustomer(_customer2)
        };

        
        [Test]
        public async Task IsolationLevelPerformanceTest()
        {
            // Arrange
            
            // Shared BookingDataControl instance
            IDbConnection sharedConnection = _connection.GetOpenConnection();
            BookingDataControl sharedBookingController = new BookingDataControl(_dBBooking1, new CustomerDataControl(_dBCustomer1, sharedConnection), sharedConnection);
            //TestContext.WriteLine(sharedBookingController.TestInsertIsolationLevel(isolationlevel));

            // Shared BookingController instances
            BookingController client1 = new BookingController(sharedBookingController);
            BookingController client2 = new BookingController(sharedBookingController);

            // List to hold tasks
            List<Task<IActionResult>> tasks = new List<Task<IActionResult>>();

            int waitPeriod = 1;
            Stopwatch stopwatch = new Stopwatch();

            // Act
            stopwatch.Start();

            // Start client1 request
            Task<IActionResult> taskClient1 = client1.CreateBooking(_bookingRequest1);
            tasks.Add(taskClient1);

            // Simulate an artificial delay
            await Task.Delay(waitPeriod);

            // Start client2 request
            Task<IActionResult> taskClient2 = client2.CreateBooking(_bookingRequest2);
            tasks.Add(taskClient2);

            // Wait for all tasks to be done
            await Task.WhenAll(tasks);

            // Stop timer
            stopwatch.Stop();

            // Assert
            var firstResult = tasks[0].Result;
            Assert.IsInstanceOf<OkObjectResult>(firstResult);

            var secondResult = tasks[1].Result;
            Assert.IsInstanceOf<UnprocessableEntityObjectResult>(secondResult);

            // Write test results in test summary
            TestContext.WriteLine($"Client1 responce was {firstResult}");
            TestContext.WriteLine($"Client2 responce was {secondResult}");
            TestContext.WriteLine($"Delay between client requests was ({waitPeriod}) milliseconds long");
            TestContext.WriteLine($"Simulation took {stopwatch.Elapsed} time to complete");
        }


        //Clean database this way, because we cannot manage actions as a transaction and rollback afterwards.
        [TearDown]
        public void TearDown() {
            string script = "delete from Booking where notes = 'Delete me please'";

            using(IDbConnection conn = DBConnection.Instance.GetOpenConnection()) {
                conn.Execute(script);
            }
        }

        //Create some generic semilar bookings at the same time.
        private static List<Booking> initializeBookings() {
            List<Booking> result = new();
            TimeSpan startSpan = new TimeSpan(15, 0, 0);
            TimeSpan endSpan = new TimeSpan(16, 0, 0);
            for(int i = 0; i < 6; i++) {
                Booking booking = new Booking {
                    TimeStart = DateTime.Now.Date.AddYears(2) + startSpan,
                    TimeEnd = DateTime.Now.Date.AddYears(2) + endSpan,
                    Notes = "Delete me please"
                };
                result.Add(booking);
            }

            return result;
        }

    }
}
