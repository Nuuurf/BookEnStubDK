﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework.Constraints;
using RestfulApi.BusinessLogic;
using RestfulApi.Controllers;
using RestfulApi.Models;

namespace TestProject.API.Controller
{
    public class GetBookingsForGivenDateTest
    {
        [Test]
        public async Task GetBookingForGivenDate_ReturnsOK_WithValidDate()
        {
            //Arrange
            DateTime priorDay = DateTime.Now.Date.AddDays(-1);
            var mockDBBokking = new Mock<IBookingData>();
            mockDBBokking.Setup(repo => repo.GetAvailableBookingsForGivenDate(priorDay)).ReturnsAsync(new List<AvailableBookingsForTimeframe>());

            BookingController controller = new BookingController(mockDBBokking.Object);

            string date = priorDay.ToString("yyyy-MM-dd");

            //Act
            var result = await controller.GetBookingsForGivenDate(date) as OkObjectResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [TestCase("Test", TestName = "Parse word as date")]
        [TestCase("2023-40-11", TestName = "Parse invalid date (2023-40-11)")]
        [TestCase("2023-1-1", TestName = "Parse invalid date (2023-1-1)")]
        [TestCase("2023-1", TestName = "Parse invalid date (2023-1)")]
        [TestCase("2023", TestName = "Parse invalid date (2023)")]
        [TestCase("2-2-2", TestName = "Parse invalid date (2-2-2)")]
        [TestCase("2023-April-14", TestName = "Parse invalid date (2023-April-14)")]
        [TestCase("2023/11/14", TestName = "Parse invalid date (2023/11/14)")]
        public async Task GetBookingForGivenDate_ReturnsBadRequest_WithInvalidDate(string date)
        {
            //Arrange
            var mockDBBokking = new Mock<IBookingData>();
            mockDBBokking.Setup(repo => repo.GetAvailableBookingsForGivenDate(It.IsAny<DateTime>())).ReturnsAsync(new List<AvailableBookingsForTimeframe>());

            BookingController controller = new BookingController(mockDBBokking.Object);

            //Act
            var result = await controller.GetBookingsForGivenDate(date) as BadRequestObjectResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        [Test]
        public async Task GetBookingForGivenDate_ReturnsBadRequest_WithPriorDate()
        {
            //Arrange
            DateTime priorDay = DateTime.Now.Date.AddDays(-1);
            var mockDBBokking = new Mock<IBookingData>();

            List<AvailableBookingsForTimeframe> nullList = null;
            mockDBBokking.Setup(repo => repo.GetAvailableBookingsForGivenDate(priorDay)).ReturnsAsync(nullList);

            BookingController controller = new BookingController(mockDBBokking.Object);

            
            string date = priorDay.ToString("yyyy-MM-dd");

            //Act
            var result = await controller.GetBookingsForGivenDate(date) as BadRequestObjectResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }
    }
}