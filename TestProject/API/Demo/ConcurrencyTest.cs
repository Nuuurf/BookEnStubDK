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
            int maxIterations = 5000;  // Maximum number of times to run the test
            bool testPassed = false;

            for (int iteration = 0; iteration < maxIterations && !testPassed; iteration++)
            {
                // Arrange
                IDbConnection connection1 = _connection.GetOpenConnection();
                IDbConnection connection2 = _connection.GetOpenConnection();
                BookingDataControl controller1 = new BookingDataControl(_dBBooking1, new CustomerDataControl(_dBCustomer1, connection1), connection1);
                BookingDataControl controller2 = new BookingDataControl(_dBBooking1, new CustomerDataControl(_dBCustomer1, connection2), connection2);

                BookingController client1 = new BookingController(controller1);
                BookingController client2 = new BookingController(controller2);

                List<Task<IActionResult>> tasks = new List<Task<IActionResult>>();

                // Act
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                Task<IActionResult> taskClient1 = client1.CreateBooking(_bookingRequest1);
                tasks.Add(taskClient1);

                await Task.Delay(1);  // Simulate an artificial delay

                Task<IActionResult> taskClient2 = client2.CreateBooking(_bookingRequest2);
                tasks.Add(taskClient2);

                await Task.WhenAll(tasks);

                stopwatch.Stop();

                // Assert
                var firstResult = tasks[0].Result as ObjectResult;
                var secondResult = tasks[1].Result as ObjectResult;

                bool isFirstResultOk = firstResult is OkObjectResult;
                bool isSecondResultOk = secondResult is OkObjectResult;
                bool isFirstResultUnprocessable = firstResult is UnprocessableEntityObjectResult;
                bool isSecondResultUnprocessable = secondResult is UnprocessableEntityObjectResult;

                bool isOneOkAndOneUnprocessable = (isFirstResultOk && isSecondResultUnprocessable) || (isFirstResultUnprocessable && isSecondResultOk);

                if (!isOneOkAndOneUnprocessable)
                {
                    // Write test results in test summary for the failed iteration
                    TestContext.WriteLine($"Iteration {iteration + 1} failed.");
                    TestContext.WriteLine($"Client1 response was {firstResult}");
                    TestContext.WriteLine($"Client2 response was {secondResult}");
                    TestContext.WriteLine($"Delay between client requests was 1 millisecond");
                    TestContext.WriteLine($"Simulation took {stopwatch.Elapsed} time to complete");

                    // Break out of the loop if the test fails
                    break;
                }

                testPassed = true; // Mark test as passed if we reach this point
            }

            Assert.IsTrue(testPassed, "Test failed in one or more iterations.");
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
