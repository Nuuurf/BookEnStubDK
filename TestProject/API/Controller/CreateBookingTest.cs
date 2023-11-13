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

namespace TestProject.API.Controller
{
    public class CreateBookingTest
    {

        [Test]
        public void CreateBooking_ReturnsOK_WithNewBooking()
        {
            //Arrange
            var mockDBBokking = new Mock<IDBBooking>();
            mockDBBokking.Setup(repo => repo.CreateBooking(It.IsAny<Booking>())).Returns(true);

            BookingController controller = new BookingController(mockDBBokking.Object);

            Booking booking = new Booking();

            //Act   
            var result = controller.CreateBooking(booking) as OkObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(200, result.StatusCode);

        }


        [Test]
        public void CreateBooking_ReturnBadRequest_WithNoBooking()
        {
            //Arrange
            var mockDBBokking = new Mock<IDBBooking>();
            mockDBBokking.Setup(repo => repo.CreateBooking(It.IsAny<Booking>())).Returns(false);

            BookingController controller = new BookingController(mockDBBokking.Object);
            Booking booking = new Booking();


            //Act
            var result = controller.CreateBooking(booking) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(400, result.StatusCode);

        }

        //[Test]
        //public void CreateBooking_ReturnInternalError_WithNullBooking()
        //{
        //    //Arrange
        //    var mockDBBokking = new Mock<IDBBooking>();
        //    mockDBBokking.Setup(repo => repo.CreateBooking(It.IsAny<Booking>())).Returns(null);

        //    BookingController controller = new BookingController(mockDBBokking.Object);

        //    //Act
        //    var result = controller.CreateBooking(null);

        //    if (result is ObjectResult objectResult)
        //    {
        //        Assert.AreEqual(500, objectResult.StatusCode);
        //    }
        //    else
        //    {
        //        Assert.Fail();
        //    }

        //    //Assert
        //    Assert.NotNull(result);
        
        //} 

    }
}
