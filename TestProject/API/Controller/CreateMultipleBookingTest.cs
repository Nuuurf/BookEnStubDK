using Microsoft.AspNetCore.Mvc;
using Moq;
using RestfulApi.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NuGet.Frameworks;
using RestfulApi.Controllers;
using RestfulApi.Models;

namespace TestProject.API.Controller
{
    public class CreateMultipleBookingTest
    {
        [Test]
        public async Task CreateMultipleBookingTest_ShouldReturnOK_WithBookings()
        {
            //Arrange
List<Booking> bookingList = new List<Booking>();
            var mockDBBokking = new Mock<IBookingData>();
            mockDBBokking.Setup(repo => repo.CreateMultipleBookings(bookingList)).ReturnsAsync(1);
            BookingController controller = new BookingController(mockDBBokking.Object);

            //Act 
            var result = await controller.CreateMultipleBooking(bookingList) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

        }

        [Test]
        public async Task CreateMultipleBookingTest_ShouldReturnUnProccessableEntity_WithBookings()
        {
            //Arrange
            List<Booking> bookingList = new List<Booking>();
            var mockDBBokking = new Mock<IBookingData>();
            mockDBBokking.Setup(repo => repo.CreateMultipleBookings(bookingList)).ReturnsAsync(-1);
            BookingController controller = new BookingController(mockDBBokking.Object);

            //Act 
            var result = await controller.CreateMultipleBooking(bookingList) as UnprocessableEntityObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(422, result.StatusCode);

        }
        [Test]
        public async Task CreateMultipleBookingTest_ShouldReturnBackRequest_WithNoBookings()
        {
            //Arrange
            List<Booking> bookingList = null;
            var mockDBBokking = new Mock<IBookingData>();
            mockDBBokking.Setup(repo => repo.CreateMultipleBookings(bookingList)).ReturnsAsync(-1);
            BookingController controller = new BookingController(mockDBBokking.Object);

            //Act 
            var result = await controller.CreateMultipleBooking(bookingList) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);

        }

        [Test]
        public async Task CreateMultipleBookingTest_ThrowsException_InternalError()
        {
            //Arrange
            var mockDBBokking = new Mock<IBookingData>();
            List<Booking> bookingList = new List<Booking>();
            mockDBBokking.Setup(repo => repo.CreateMultipleBookings(It.IsAny<List<Booking>>()))
                .Throws(new Exception());

            BookingController controller = new BookingController(mockDBBokking.Object);

            //Act

            var resultTask = controller.CreateMultipleBooking(bookingList);
            var result = await resultTask;

            //Assert
            if (result is ObjectResult objectResult)
            {
                Assert.AreEqual(500, objectResult.StatusCode);
            }
            Assert.IsInstanceOf<ObjectResult>(result);
        }
    }
}
