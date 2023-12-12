using Microsoft.AspNetCore.Mvc;
using Moq;
using RestfulApi.BusinessLogic;
using RestfulApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.API.Utilities;

namespace TestProject.API.Controller {
    public class DeleteBookingTest {

        [Test]
        public async Task DeleteBooking_ShouldReturnOk() {
            //Arrange
            var mockBookingData = new Mock<IBookingData>();
            int bookingId = 1;
            IActionResult result = null;

            mockBookingData.Setup(m => m.DeleteBooking(bookingId)).ReturnsAsync(true);

            BookingController bookingController = new BookingController(mockBookingData.Object);

            //Act
            result = await bookingController.DeleteBooking(bookingId);

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [Test]
        public async Task DeleteBooking_ShouldReturn500() {
            //Arrange
            var mockBookingData = new Mock<IBookingData>();
            int bookingId = 1;
            IActionResult result = null;

            mockBookingData.Setup(m => m.DeleteBooking(bookingId)).Throws<Exception>();

            BookingController bookingController = new BookingController(mockBookingData.Object);

            //Act
            result = await bookingController.DeleteBooking(bookingId);

            //Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var serverErrorResult = result as ObjectResult;
            Assert.AreEqual(500, serverErrorResult.StatusCode);

        }

        [Test]
        public async Task DeleteBooking_ShouldReturn422() {
            //Arrange
            var mockBookingData = new Mock<IBookingData>();
            int bookingId = 1;
            IActionResult result = null;

            mockBookingData.Setup(m => m.DeleteBooking(bookingId)).ReturnsAsync(false);

            BookingController bookingController = new BookingController(mockBookingData.Object);

            //Act
            result = await bookingController.DeleteBooking(bookingId);

            //Assert
            Assert.IsInstanceOf<UnprocessableEntityObjectResult>(result);
            var unprocessableResult = result as UnprocessableEntityObjectResult;
            Assert.AreEqual(422, unprocessableResult.StatusCode);
        }

    }
}
