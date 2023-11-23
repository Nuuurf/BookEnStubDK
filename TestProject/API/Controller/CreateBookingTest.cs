using Microsoft.AspNetCore.Mvc;
using Moq;
using RestfulApi.BusinessLogic;
using RestfulApi.Controllers;
using RestfulApi.DTOs;
using RestfulApi.Models;

namespace TestProject.API.Controller;

public class CreateBookingTest
{
    private BookingRequest _bookingRequest = new BookingRequest
    {
        Appointments
            = new List<DTONewBooking>
            {
                new DTONewBooking
                {
                    Notes = "", TimeStart = DateTime.Now.AddHours(1), TimeEnd = DateTime.Now.AddHours(2)
                }
            },
        Customer = new DTOCustomer { FullName = "", Email = "", Phone = "" }
    };

    [Test]
    public async Task CreateBooking_ReturnsOK_WithNewBooking()
    {
        //Arrange
        var mockDBBokking = new Mock<IBookingData>();

        mockDBBokking.Setup(repo => repo.CreateBooking(It.IsAny<List<Booking>>(), It.IsAny<Customer>()))
            .ReturnsAsync(1);

        BookingController controller = new BookingController(mockDBBokking.Object);

        
        //Act   
        OkObjectResult? result = await controller.CreateBooking(_bookingRequest) as OkObjectResult;

        //Assert
        Assert.NotNull(result);
        Assert.AreEqual(200, result.StatusCode);
    }

    [Test]
    public async Task CreateBooking_ReturnBadRequest_WithNoBooking()
    {
        //Arrange
        var mockDBBokking = new Mock<IBookingData>();
        mockDBBokking.Setup(repo => repo.CreateBooking(It.IsAny<List<Booking>>(), It.IsAny<Customer>())).ReturnsAsync(0);

        BookingController controller = new BookingController(mockDBBokking.Object);

        //Act
        BadRequestObjectResult? result = await controller.CreateBooking() as BadRequestObjectResult;

        //Assert
        Assert.NotNull(result);
        Assert.AreEqual(400, result.StatusCode);
    }

    [Test]
    public async Task CreateBookingFailed_ReturnUnproccessableEntity_WithBooking()
    {
        //Arrange
        var mockDBBokking = new Mock<IBookingData>();
        mockDBBokking.Setup(repo => repo.CreateBooking(It.IsAny<List<Booking>>(), It.IsAny<Customer>())).ReturnsAsync(0);

        BookingController controller = new BookingController(mockDBBokking.Object);

        //Act   
        UnprocessableEntityObjectResult? result
            = await controller.CreateBooking(_bookingRequest) as UnprocessableEntityObjectResult;

        //Assert
        Assert.NotNull(result);
        Assert.AreEqual(422, result.StatusCode);
    }

    [Test]
    public async Task CreateBookingFailed_ThrowsException_InternalError()
    {
        //Arrange
        var mockDBBokking = new Mock<IBookingData>();
        
        mockDBBokking.Setup(repo => repo.CreateBooking(It.IsAny<List<Booking>>(), It.IsAny<Customer>()))
            .Throws(new Exception());

        BookingController controller = new BookingController(mockDBBokking.Object);

        //Act

        var resultTask = controller.CreateBooking(_bookingRequest);
        IActionResult result = await resultTask;

        //Assert
        if (result is ObjectResult objectResult)
        {
            Assert.AreEqual(500, objectResult.StatusCode);
        }

        Assert.IsInstanceOf<ObjectResult>(result);
    }
}