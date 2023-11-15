using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RestfulApi.Controllers;
using RestfulApi.DAL;
using RestfulApi.Models;
using RestfulApi.BusinessLogic;

namespace TestProject.API.Controller
{
    public class CreateBookingTest
    {
        [Test]
        public async Task CreateBooking_ReturnsOK_WithNewBooking()
        {
            //Arrange
            var mockDBBokking = new Mock<IBookingData>();
            mockDBBokking.Setup(repo => repo.CreateBooking(It.IsAny<Booking>())).ReturnsAsync(1);

            BookingController controller = new BookingController(mockDBBokking.Object);

            Booking booking = new Booking();

            //Act   
            var result = await controller.CreateBooking(booking) as OkObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }


        [Test]
        public async Task CreateBooking_ReturnBadRequest_WithNoBooking()
        {
            //Arrange
            var mockDBBokking = new Mock<IBookingData>();
            mockDBBokking.Setup(repo => repo.CreateBooking(It.IsAny<Booking>())).ReturnsAsync(0);

            BookingController controller = new BookingController(mockDBBokking.Object);

            //Act
            var result = await controller.CreateBooking(null) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        [Test]
        public async Task CreateBookingFailed_ReturnUnproccessableEntity_WithBooking()
        {
            //Arrange
            var mockDBBokking = new Mock<IBookingData>();
            mockDBBokking.Setup(repo => repo.CreateBooking(It.IsAny<Booking>())).ReturnsAsync(0);

            BookingController controller = new BookingController(mockDBBokking.Object);

            Booking booking = new Booking();

            //Act   
            var result = await controller.CreateBooking(booking) as UnprocessableEntityObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(422, result.StatusCode);
        }
    }
}
