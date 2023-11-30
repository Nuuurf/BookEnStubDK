﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RestfulApi.BusinessLogic;
using RestfulApi.Controllers;
using RestfulApi.DAL;
using RestfulApi.Models;

namespace TestProject.API.BusinessLogic
{
    public class GetAvailableBookingsForGivenDateTest
    {
        /*[Test]
        public async Task GetAvailableBookingsForGivenDate_ReturnAvailableBookings_WithValidDate()
        {
            //Arrange
            var mockDBBokking = new Mock<IDBBooking>();

            DateTime date = DateTime.Now.Date;
            List<AvailableBookingsForTimeframe> availableBookings = new List<AvailableBookingsForTimeframe>();
            mockDBBokking.Setup(repo => repo.GetAvailableBookingsForGivenDate( null!, date, null!)).ReturnsAsync(availableBookings);

            BookingDataControl controller = new BookingDataControl(mockDBBokking.Object, null!, null!);

            //Act   
            var result = await controller.GetAvailableBookingsForGivenDate(date);

            //Assert
            Assert.NotNull(result);
            Assert.That(result, Is.EqualTo(availableBookings));
        }

        [Test]
        public async Task GetAvailableBookingsForGivenDate_ReturnNull_WithPriorDate()
        {
            //Arrange
            DateTime priorDay = DateTime.Now.Date.AddDays(-1);

            BookingDataControl controller = new BookingDataControl(null!, null!, null!);

            //Act   
            var result = await controller.GetAvailableBookingsForGivenDate(priorDay);

            //Assert
            Assert.Null(result);
        }*/
        }
}
