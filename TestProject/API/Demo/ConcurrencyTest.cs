﻿using RestfulApi.BusinessLogic;
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
            FirstName = "Client1",
            Phone = "11111111",
            Email = "client1@concurrency.test"
        };

        private static Customer _customer2 = new Customer {
            FirstName = "Client2",
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



        //[Test]
        //public void ConcurrencyTest_RepeatableRead() {
        //    IsolationLevel isolationlevel = IsolationLevel.RepeatableRead;
        //}

        [Test]
        public async Task ConcurrencyTest_Serializable() {
            //Arrange   //Not actually used since we cant inject isolation levels
            IsolationLevel isolationlevel = IsolationLevel.Serializable;
                //Set up clients to compete for resource, with two different connection.
                //Client 1
            IDbConnection connection1 = _connection.GetOpenConnection();
            BookingController client1 = new BookingController(new BookingDataControl(_dBBooking1, new CustomerDataControl(_dBCustomer1, connection1), connection1));
                //Client 2
            IDbConnection connection2 = _connection.GetOpenConnection();
            BookingController client2 = new BookingController(new BookingDataControl(_dBBooking2, new CustomerDataControl(_dBCustomer2, connection2), connection2));

                //Create something to contain the tasks while they compute
            List<Task<IActionResult>> tasks = new List<Task<IActionResult>>();

            int waitPeriod = 1;

            Stopwatch stopwatch = new Stopwatch();
            // Act
                //Start a timer, just for fun since we cant measure the time difference between isolationlevels.
            stopwatch.Start();
                //Start client1 request
            Task<IActionResult> taskClient1 = client1.CreateBooking(_bookingRequest1);
            tasks.Add(taskClient1);

                //Simulate an artificial delay
            await Task.Delay(waitPeriod);

                //Start client2 request
            Task<IActionResult> taskClient2 = client2.CreateBooking(_bookingRequest2);
            tasks.Add(taskClient2);

                //Wait for all tasks to be done before proceding
            await Task.WhenAll(tasks);
                
                //Stop timer
            stopwatch.Stop();

            // Assert
            OkObjectResult? firstResult = tasks[0].Result as OkObjectResult;
            Assert.NotNull(firstResult);
            Assert.AreEqual(200, firstResult.StatusCode);

            ObjectResult? secondResult = tasks[1].Result as ObjectResult;
            Assert.NotNull(secondResult);
            Assert.AreEqual(500, secondResult.StatusCode);

                //Write test results in test summary.
            TestContext.WriteLine($"Client1 responce was {firstResult.StatusCode}");
            TestContext.WriteLine($"Client2 responce was {secondResult.StatusCode}");
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

            for(int i = 0; i < 6; i++) {
                Booking booking = new Booking {
                    TimeStart = DateTime.Now.AddYears(2),
                    TimeEnd = DateTime.Now.AddYears(2).AddHours(1),
                    Notes = "Delete me please"
                };
                result.Add(booking);
            }

            return result;
        }

    }
}