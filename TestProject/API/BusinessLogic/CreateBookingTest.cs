using System.Data;
using Moq;
using RestfulApi.BusinessLogic;
using RestfulApi.DAL;
using RestfulApi.Models;
using TestProject.API.Utilities;

namespace TestProject.API.BusinessLogic;

public class CreateBookingTest
{

    private TimeSpan ts = new TimeSpan(15, 0, 0);

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

    [Test]
    public async Task CreateBooking_ReturnsNewBookingId_WithNewBooking()
    {
        // Arrange
        DateTime bookingStart = DateTime.Now.Date.AddDays(1) + ts;
        DateTime bookingEnd = bookingStart.AddHours(1);
        Booking booking = new Booking { TimeStart = bookingStart, TimeEnd = bookingEnd };
        List<Booking> bookingList = new List<Booking> { booking };
        Customer customer = new Customer();

        //Setting Mocks
        var mockDBBooking = new Mock<IDBBooking>();
        var (mockDBConnection, mockDbTransaction) = PredefinedMocks.ConnectionMocks();
        var mockCustomerControl = MockCustomerControl();

        //Setting up Database Booking Mock
        mockDBBooking.Setup(repo => repo.CreateBooking(
                mockDBConnection.Object, booking, 1, mockDbTransaction.Object))
            .ReturnsAsync(1);

        mockDBBooking.Setup(repo =>
                repo.CreateNewBookingOrder(mockDBConnection.Object, mockDbTransaction.Object))
            .ReturnsAsync(1);

        BookingDataControl controller
            = new BookingDataControl(mockDBBooking.Object, mockCustomerControl.Object, mockDBConnection.Object);

        // Act
        int result = await controller.CreateBooking(bookingList, customer);

        // Assert
        Assert.That(result, Is.EqualTo(1));

        mockDbTransaction.Verify(trans => trans.Commit(), Times.Once);
        mockDbTransaction.Verify(trans => trans.Rollback(), Times.Never);
    }

    [Test]
    public Task CreateBooking_ThrowsArgumentException_WithPriorDate()
    {
        //Arrange
        DateTime bookingDate = DateTime.Now.AddDays(-1);
        List<Booking> booking = new List<Booking> { new Booking { TimeStart = bookingDate } };
Customer customer = new Customer();
        
        BookingDataControl controller = new BookingDataControl(null!, null!, null!);

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
        List<Booking> booking = null!;

        BookingDataControl controller = new BookingDataControl(null!, null!, null!);

        //Act   
        AsyncTestDelegate result = () => controller.CreateBooking(booking!, null!);

        //Assert
        Assert.ThrowsAsync<ArgumentNullException>(result);

        return Task.CompletedTask;
    }
    [Test]
    public Task CreateBooking_ThrowsArgumentNullException_WithNullCustomer()
    {
        //Arrange
        List<Booking> booking = new List<Booking>();

        BookingDataControl controller = new BookingDataControl(null!, null!, null!);

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
        DateTime bookingStart = DateTime.Now.Date + ts;
        DateTime bookingEnd = bookingStart.AddDays(-1);
        List<Booking> booking = new List<Booking>
        {
            new Booking { TimeStart = bookingStart, TimeEnd = bookingEnd }
        };
Customer customer = new Customer();

        BookingDataControl controller = new BookingDataControl(null!,null!, null!);

        //Act   
        AsyncTestDelegate result = () => controller.CreateBooking(booking, customer);

        //Assert
        Assert.ThrowsAsync<ArgumentException>(result);

        return Task.CompletedTask;
    }

    [Test]
    public Task CreateBooking_ThrowsException_WithNoAvailbableStubsForBooking()
    {
        //Arrange
        DateTime bookingStart = DateTime.Now.Date.AddDays(1) + ts;
        DateTime bookingEnd = bookingStart.AddHours(1);
        Booking booking = new Booking
        {
            TimeStart = bookingStart,
            TimeEnd = bookingEnd
        };
        List<Booking> bookingList = new List<Booking> { booking };
        Customer customer = new Customer();

        var mockDBBooking = new Mock<IDBBooking>();
        var (mockDBConnection, mockDbTransaction) = PredefinedMocks.ConnectionMocks();
        var mockCustomerControl = MockCustomerControl();

        mockDBBooking.Setup(repo => repo.CreateBooking(
                mockDBConnection.Object,
                booking,
                It.IsAny<int>(),
                mockDbTransaction.Object))
            .Throws<Exception>();


        BookingDataControl controller = new BookingDataControl(mockDBBooking.Object,mockCustomerControl.Object, mockDBConnection.Object);

        //Act   
        AsyncTestDelegate result = () => controller.CreateBooking(bookingList, customer);

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
        DateTime bookingStart = DateTime.Now.Date.AddDays(1) + ts;
        DateTime bookingEnd = bookingStart.AddHours(1);
        var booking = new List<Booking>
        {
            new Booking { TimeStart = bookingStart, TimeEnd = bookingEnd }
        };

        Customer customer = new Customer();
        mockDBBooking.Setup(repo => repo.CreateBooking(
                mockDBConnection.Object,
                It.IsAny<Booking>(),
                It.IsAny<int>(),
                mockDbTransaction.Object))
            .ThrowsAsync(new Exception("Sql Exception"));

        BookingDataControl controller = new BookingDataControl(mockDBBooking.Object, mockCustomerControl.Object, mockDBConnection.Object);

        //Act   
        AsyncTestDelegate result = () => controller.CreateBooking(booking, customer);

        //Assert
        Assert.ThrowsAsync<Exception>(result);

        return Task.CompletedTask;
    }
}