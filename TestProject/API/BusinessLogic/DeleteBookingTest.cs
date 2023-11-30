using Moq;
using RestfulApi.BusinessLogic;
using RestfulApi.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.API.Utilities;

namespace TestProject.API.BusinessLogic {
    public class DeleteBookingTest {

        

        [Test]
        public async Task DeleteBooking_shouldReturnTrue() {
            //Arrange
            var mockDBBooking = new Mock<IDBBooking>();
            int bookingId = 1;
            bool result = false;
                //set up mock
            mockDBBooking.Setup(m => m.DeleteBooking(null!, bookingId, null!)).ReturnsAsync(true);
            BookingDataControl bdc = new BookingDataControl(mockDBBooking.Object, null!, null!);

            //Act
            result = await bdc.DeleteBooking(bookingId);

            //Assert
            Assert.True(result);
        }


        [Test]
        public async Task DeleteBooking_shouldReturnFalse() {
            //Arrange
            Mock<IDBBooking> mockDBBooking = new Mock<IDBBooking>();
            var (mockConnection, mockTransaction) = PredefinedMocks.ConnectionMocks();
            int bookingId = -1;
            bool result = true;

            //set up mock
            mockDBBooking.Setup(m => m.DeleteBooking(mockConnection.Object, bookingId, mockTransaction.Object)).ReturnsAsync(false);
            BookingDataControl bdc = new BookingDataControl(mockDBBooking.Object, null, mockConnection.Object);
            //Act
            result = await bdc.DeleteBooking(bookingId);

            //Assert
            Assert.False(result);
        }
    }
}
