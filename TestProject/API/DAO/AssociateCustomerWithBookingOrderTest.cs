
using RestfulApi.BusinessLogic;
using RestfulApi.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.API.Utilities;

namespace TestProject.API.DAO
{
    public class AssociateCustomerWithBookingOrderTest {

        [Test]
        public async Task AssociateCustomerWithBookingOrder_ShouldReturnTrue() {
            //Arrange
            int orderId = 1; //We just assume that we haven't cleaned the test db, if it fuckes insert an order with default in setup
            int customerId = 21; //Just the value inside the test database
            bool result = false;
            IDBCustomer dBCustomer = new DBCustomer();
            //Act
            using (IDbTransaction trans = DBConnection.Instance.GetOpenConnection().BeginTransaction()) {
                result = await dBCustomer.AssociateCustomerWithBookingOrder(trans.Connection!, orderId, customerId, trans);

                trans.Rollback();
            }
            //Assert
            Assert.True(result);
        }

        [Test]
        public Task AssociateCustomerWithBookingOrder_ShouldThrowException() {
            //Arrange
            int orderId = 1; //We just assume that we haven't cleaned the test db, if it fuckes insert an order with default in setup
            int customerId = -1; //value which does not exist 
            IDBCustomer dBCustomer = new DBCustomer();
            //Act & Assert
            using (IDbTransaction trans = DBConnection.Instance.GetOpenConnection().BeginTransaction()) {
                AsyncTestDelegate testDelegate = async () =>
                await dBCustomer.AssociateCustomerWithBookingOrder(trans.Connection!, orderId, customerId, trans!);

                Assert.ThrowsAsync<Exception>(testDelegate);

                trans.Rollback();
            }

            return Task.CompletedTask;
        }
    }
}