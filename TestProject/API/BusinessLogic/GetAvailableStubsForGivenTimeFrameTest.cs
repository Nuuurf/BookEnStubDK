using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework.Constraints;
using RestfulApi.BusinessLogic;
using RestfulApi.DAL;
using RestfulApi.Models;

namespace TestProject.API.BusinessLogic
{
    public class GetAvailableStubsForGivenTimeFrameTest
    {
        private static Mock<IDbConnection> mockDbConnection = new Mock<IDbConnection>();
        private static DateTime dateTime = new DateTime(2022, 10, 10, 9, 0, 0);
        private static Booking b1 = new Booking{TimeStart = dateTime, TimeEnd = dateTime.AddHours(1), StubId = 1};
        private static Booking b2 = new Booking{TimeStart = dateTime, TimeEnd = dateTime.AddHours(1), StubId = 2 };
        private static Booking b3 = new Booking { TimeStart = dateTime.AddHours(1), TimeEnd = dateTime.AddHours(2), StubId = 1 };
        private static Booking b4 = new Booking { TimeStart = dateTime.AddHours(1), TimeEnd = dateTime.AddHours(2), StubId = 2 };
        private static Booking b5 = new Booking { TimeStart = dateTime.AddHours(1), TimeEnd = dateTime.AddHours(2), StubId = 3 };
        private static Booking b6 = new Booking { TimeStart = dateTime.AddHours(-1), TimeEnd = dateTime, StubId = 2 };
        private static Booking b7 = new Booking { TimeStart = dateTime.AddHours(13), TimeEnd = dateTime.AddHours(14), StubId = 2 };
        private static List<Booking> bookingList = new List<Booking>{ b1, b2, b3, b4,b5, b6, b7 };

        [Test]
        public async Task GetAvailableStubsForGivenTimeFrameTest_ShouldReturnListOfAvailableStubs_WithValidData()
        {
            //Arrange
            DateTime start = dateTime;
            DateTime end = start.AddHours(2);

            Mock<IDBBooking> mockDBBooking = new Mock<IDBBooking>();
            mockDBBooking.Setup(repo => repo.GetAllStubs(mockDbConnection.Object, null!)).ReturnsAsync(new List<int>{1,2,3});

            mockDBBooking.Setup(repo =>
                    repo.GetBookingsInTimeslot(mockDbConnection.Object, It.IsAny<BookingRequestFilter>(),null!))
                .ReturnsAsync(bookingList);

            BookingDataControl controller = new BookingDataControl(mockDBBooking.Object, null!, mockDbConnection.Object);

            //Act   
            List<AvailableStubsForHour> result = await controller.GetAvailableStubsForGivenTimeFrame(start, end);

            //Assert
            Assert.That(result.Count, Is.EqualTo(3));
            Assert.That(result.ElementAt(0).AvailableStubIds.Count, Is.EqualTo(1));
            Assert.That(result.ElementAt(1).AvailableStubIds.Count, Is.EqualTo(0));
            Assert.That(result.ElementAt(2).AvailableStubIds.Count, Is.EqualTo(3));
            Assert.That(result.ElementAt(0).TimeStart, Is.EqualTo(b1.TimeStart));
            Assert.That(result.ElementAt(1).TimeStart, Is.EqualTo(b3.TimeStart));
        }

        [Test]
        public async Task GetAvailableStubsForGivenTimeFrameTest_ShouldReturnNull_WithEndPriorToStart()
        {
            //Arrange
            DateTime start = dateTime;
            DateTime end = start.AddHours(-2);
            
            BookingDataControl controller = new BookingDataControl(null!, null!, null!);

            //Act   
            List<AvailableStubsForHour> result = await controller.GetAvailableStubsForGivenTimeFrame(start, end);

            //Assert
            Assert.IsNull(result);
        }

    }
}
