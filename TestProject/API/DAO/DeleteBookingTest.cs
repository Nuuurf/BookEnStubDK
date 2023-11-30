using RestfulApi.DAL;
using RestfulApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.API.Utilities;

namespace TestProject.API.DAO {
    public class DeleteBookingTest {

        private static DBBooking _DbBooking = new DBBooking();
        private IsolationLevel _IsolationLevel = System.Data.IsolationLevel.Serializable;

        [Test]
        public async Task DeleteBooking_shouldReturnTrue() {
            //Arrange
            Booking booking = TestModels.BookingValid();
            int insertedId = -1;
            int bookingOrderId = -1;
            bool result = false;

            //Act
            using (IDbTransaction trans = DBConnection.Instance.GetOpenConnection().BeginTransaction(_IsolationLevel)) {
                bookingOrderId = await _DbBooking.CreateNewBookingOrder(trans.Connection, trans);
                insertedId = await _DbBooking.CreateBooking(trans.Connection, booking, bookingOrderId, trans);
                result = await _DbBooking.DeleteBooking(trans.Connection, insertedId, trans);

                Assert.True(result);

                trans.Rollback();
            }
        }

        [Test]
        public async Task DeleteBooking_ShouldThrowException() {
            //Arrange
            int bookingId = -1; //Imposible id

            //Act & Assert
            using (IDbTransaction trans = DBConnection.Instance.GetOpenConnection().BeginTransaction(_IsolationLevel)) {
                AsyncTestDelegate testDelegate = async () =>
                await _DbBooking.DeleteBooking(trans.Connection, bookingId, trans);

                Assert.ThrowsAsync<Exception>(testDelegate);

                trans.Rollback();
            }
        }
    }
}
