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
        var mockBusinessBooking = new Mock<IBookingData>();

        mockBusinessBooking.Setup(repo => repo.CreateBooking(It.IsAny<List<Booking>>(), It.IsAny<Customer>()))
            .ReturnsAsync(1);

        BookingController controller = new BookingController(mockBusinessBooking.Object);

        
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
        BookingController controller = new BookingController(null!);

        //Act
        var result = await controller.CreateBooking();

        //Assert
        Assert.IsInstanceOf<BadRequestObjectResult>(result);
    }

    [Test]
    public async Task CreateBookingFailed_ReturnUnproccessableEntity_WithBooking()
    {
        //Arrange
        var mockBusinessBooking = new Mock<IBookingData>();
        mockBusinessBooking.Setup(repo => repo.CreateBooking(It.IsAny<List<Booking>>(), It.IsAny<Customer>())).ReturnsAsync(0);

        BookingController controller = new BookingController(mockBusinessBooking.Object);

        //Act   
        var result = await controller.CreateBooking(_bookingRequest);

        //Assert
        Assert.IsInstanceOf<UnprocessableEntityObjectResult>(result);
    }

    [Test]
    public async Task CreateBookingFailed_ThrowsException_InternalError()
    {
        //Arrange
        var mockBusinessBooking = new Mock<IBookingData>();
        
        mockBusinessBooking.Setup(repo => repo.CreateBooking(It.IsAny<List<Booking>>(), It.IsAny<Customer>()))
            .Throws(new Exception());

        BookingController controller = new BookingController(mockBusinessBooking.Object);

        //Act

        var resultTask = controller.CreateBooking(_bookingRequest);
        IActionResult result = await resultTask;

        //Assert
        if (result is ObjectResult objectResult)
        {
            Assert.That(objectResult.StatusCode, Is.EqualTo(500));
        }

        Assert.IsInstanceOf<ObjectResult>(result);
    }
}