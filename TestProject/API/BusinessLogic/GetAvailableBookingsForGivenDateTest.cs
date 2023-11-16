using System;
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
        [Test]
        public async Task GetAvailableBookingsForGivenDate_ReturnAvailableBookings_WithValidDate()
        {
            //Arrange
            var mockDBBokking = new Mock<IDBBooking>();

            DateTime date = DateTime.Now.Date;
            List<AvailableBookingsForTimeframe> availableBookings = new List<AvailableBookingsForTimeframe>();
            mockDBBokking.Setup(repo => repo.GetAvaiableBookingsForGivenDate( null, date, null)).ReturnsAsync(availableBookings);

            BookingDataControl controller = new BookingDataControl(mockDBBokking.Object, null);

            //Act   
            var result = await controller.GetAvailableBookingsForGivenDate(date);

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(availableBookings, result);
        }

        [Test]
        public async Task GetAvailableBookingsForGivenDate_ReturnNull_WithPriorDate()
        {
            //Arrange
            DateTime priorDay = DateTime.Now.Date.AddDays(-1);

            var mockDBBokking = new Mock<IDBBooking>();

            List<AvailableBookingsForTimeframe> availableBookings = new List<AvailableBookingsForTimeframe>();
            mockDBBokking.Setup(repo => repo.GetAvaiableBookingsForGivenDate(null, priorDay, null)).ReturnsAsync(availableBookings);

            BookingDataControl controller = new BookingDataControl(mockDBBokking.Object, null);

            //Act   
            var result = await controller.GetAvailableBookingsForGivenDate(priorDay);

            //Assert
            Assert.Null(result);
        }
    }
}
