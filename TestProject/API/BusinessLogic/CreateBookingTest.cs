using System.Data;
using Moq;
using RestfulApi.BusinessLogic;
using RestfulApi.DAL;
using RestfulApi.Models;
using TestProject.API.Utilities;

namespace TestProject.API.BusinessLogic;

public class CreateBookingTest
{
    private Mock<ICustomerData> MockCustomerControl()
    {
        var mockCustomerControl = new Mock<ICustomerData>();
        //Setting up Buisness Logic Customer Mock
        mockCustomerControl.Setup(repo =>
                repo.CreateCustomer(It.IsAny<IDbConnection>(), It.IsAny<Customer>(), It.IsAny<IDbTransaction>()))
            .ReturnsAsync(1);

        mockCustomerControl
            .Setup(repo => repo.AssociateCustomerWithBookingOrder(It.IsAny<IDbConnection>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<IDbTransaction>()))
            .ReturnsAsync(true);

        return mockCustomerControl;
    }

    private TimeSpan ts = new TimeSpan(15, 0, 0);


    [Test]
    public async Task CreateBooking_ReturnsNewBookingId_WithNewBooking()
    {
        // Arrange
        var mockDBBooking = new Mock<IDBBooking>();
        var (mockDBConnection, mockDbTransaction) = PredefinedMocks.ConnectionMocks();
        var mockCustomerControl = MockCustomerControl();

        //Setting up Database Booking Mock
        mockDBBooking.Setup(repo => repo.CreateBooking(
                It.IsAny<IDbConnection>(),
                It.IsAny<Booking>(),
                It.IsAny<int>(),
                It.IsAny<IDbTransaction>()))
            .ReturnsAsync(1);

        mockDBBooking.Setup(repo =>
                repo.CreateNewBookingOrder(It.IsAny<IDbConnection>(),
                    It.IsAny<IDbTransaction>()))
            .ReturnsAsync(1);

        BookingDataControl controller
            = new BookingDataControl(mockDBBooking.Object, mockCustomerControl.Object, mockDBConnection.Object);

        DateTime bookingStart = DateTime.Now.Date + ts;
        DateTime bookingEnd = bookingStart.AddHours(1);
        var booking = new List<Booking> { new Booking { TimeStart = bookingStart, TimeEnd = bookingEnd } };
        Customer customer = new Customer();

        // Act
        int result = await controller.CreateBooking(booking, customer);

        // Assert
        Assert.That(result, Is.EqualTo(1));

        mockDbTransaction.Verify(trans => trans.Commit(), Times.Once);
        mockDbTransaction.Verify(trans => trans.Rollback(), Times.Never);
    }

    [Test]
    public Task CreateBooking_ThrowsArgumentException_WithPriorDate()
    {
        //Arrange
        var mockDBBokking = new Mock<IDBBooking>();
        DateTime bookingDate = DateTime.Now.AddDays(-1);
        List<Booking> booking = new List<Booking> { new Booking { TimeStart = bookingDate } };
Customer customer = new Customer();
        mockDBBokking.Setup(repo => repo.CreateBooking(
                It.IsAny<IDbConnection>(),
                It.IsAny<Booking>(),
                It.IsAny<int>(),
                It.IsAny<IDbTransaction>()))
            .ReturnsAsync(1);


        BookingDataControl controller = new BookingDataControl(mockDBBokking.Object, null, null!);

        //Act   
        AsyncTestDelegate result = () => controller.CreateBooking(booking, customer);

        //Assert
        Assert.ThrowsAsync<ArgumentException>(result);

        return Task.CompletedTask;
    }

    [Test]
    public Task CreateBooking_ThrowsArgumentNullException_WithNullBooking()
    {
        //Arrange
        var mockDbBokking = new Mock<IDBBooking>();
        List<Booking> booking = null!;

        mockDbBokking.Setup(repo => repo.CreateBooking(
                It.IsAny<IDbConnection>(),
                It.IsAny<Booking>(),
                It.IsAny<int>(),
                It.IsAny<IDbTransaction>()))
            .ReturnsAsync(1);

        BookingDataControl controller = new BookingDataControl(mockDbBokking.Object, null!, null!);

        //Act   
        AsyncTestDelegate result = () => controller.CreateBooking(booking!, null!);

        //Assert
        Assert.ThrowsAsync<ArgumentNullException>(result);

        return Task.CompletedTask;
    }

    [Test]
    public Task CreateBooking_ThrowsArgumentException_WithEndDateEarlierThanStart()
    {
        //Arrange
        var mockDBBokking = new Mock<IDBBooking>();
        DateTime bookingStart = DateTime.Now.Date + ts;
        DateTime bookingEnd = bookingStart.AddDays(-1);
        var booking = new List<Booking>
        {
            new Booking { TimeStart = bookingStart, TimeEnd = bookingEnd }
        };

        mockDBBokking.Setup(repo => repo.CreateBooking(
                It.IsAny<IDbConnection>(),
                It.IsAny<Booking>(),
                It.IsAny<int>(),
                It.IsAny<IDbTransaction>()))
            .ReturnsAsync(1);

        BookingDataControl controller = new BookingDataControl(mockDBBokking.Object,null!, null!);

        //Act   
        AsyncTestDelegate result = () => controller.CreateBooking(booking, null!);

        //Assert
        Assert.ThrowsAsync<ArgumentException>(result);

        return Task.CompletedTask;
    }

    [Test]
    public Task CreateBooking_ThrowsException_WithNoAvailbableStubsForBooking()
    {
        //Arrange
        var mockDBBooking = new Mock<IDBBooking>();
        var (mockDBConnection, mockDbTransaction) = PredefinedMocks.ConnectionMocks();
        var mockCustomerControl = MockCustomerControl();

        var booking = new List<Booking>
        {
            new Booking { TimeStart = DateTime.Now.AddDays(1), TimeEnd = DateTime.Now.AddDays(1).AddHours(2) }
        };

        Customer customer = new Customer();
        mockDBBooking.Setup(repo => repo.CreateBooking(
                It.IsAny<IDbConnection>(),
                It.IsAny<Booking>(),
                It.IsAny<int>(),
                It.IsAny<IDbTransaction>()))
            .Throws<Exception>();


        BookingDataControl controller = new BookingDataControl(mockDBBooking.Object,mockCustomerControl.Object, mockDBConnection.Object);

        //Act   
        AsyncTestDelegate result = () => controller.CreateBooking(booking, customer);

        //Assert
        Assert.ThrowsAsync<Exception>(result);
        mockDbTransaction.Verify(trans => trans.Commit(), Times.Never);
        mockDbTransaction.Verify(trans => trans.Rollback(), Times.Once);

        return Task.CompletedTask;
    }

    [Test]
    public Task CreateBooking_DatabaseThrowsException()
    {
        //Arrange
        var mockDBBooking = new Mock<IDBBooking>();
        var (mockDBConnection, mockDbTransaction) = PredefinedMocks.ConnectionMocks();
        var mockCustomerControl = MockCustomerControl();
        DateTime bookingStart = DateTime.Now.Date + ts;
        DateTime bookingEnd = bookingStart.AddHours(1);
        var booking = new List<Booking>
        {
            new Booking { TimeStart = bookingStart, TimeEnd = bookingEnd }
        };

        Customer customer = new Customer();
        mockDBBooking.Setup(repo => repo.CreateBooking(
                It.IsAny<IDbConnection>(),
                It.IsAny<Booking>(),
                It.IsAny<int>(),
                It.IsAny<IDbTransaction>()))
            .ThrowsAsync(new Exception("Sql Exception"));

        BookingDataControl controller = new BookingDataControl(mockDBBooking.Object, mockCustomerControl.Object, mockDBConnection.Object);

        //Act   
        AsyncTestDelegate result = () => controller.CreateBooking(booking, customer);

        //Assert
        Assert.ThrowsAsync<Exception>(result);

        return Task.CompletedTask;
    }
}