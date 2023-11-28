using Moq;
using RestfulApi.BusinessLogic;
using RestfulApi.DAL;
using RestfulApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.API.Utilities;

namespace TestProject.API.BusinessLogic {
    public class CreateCustomerTest {
private static Customer customer = new Customer
{
    FirstName = "Lars",
    Phone = "88888888",
    Email = "somethingmail@noget.dk"
};

        [Test]
        public async Task CreateCustomer_ShouldReturnPassValue() {
            
            //Arrange
            int result = -1;


            var mockDBCustomer = new Mock<IDBCustomer>();
            var (mockDBConnection, mockDbTransaction) = PredefinedMocks.ConnectionMocks(); ;
            //setup with parameters that would match real parameters
            mockDBCustomer.Setup(s => s.CreateCustomer(mockDBConnection.Object,
                customer, mockDbTransaction.Object))
                .ReturnsAsync(1);

                ICustomerData controller = new CustomerDataControl(mockDBCustomer.Object, mockDBConnection.Object);

            //Act
            result = await controller.CreateCustomer(mockDBConnection.Object, customer, mockDbTransaction.Object);

            //Assert
            Assert.Greater(result, 0);
        }

        [Test]
        public Task CreateCustomer_ShouldCauseException() {
            
            //Arrange
            var mockDBCustomer = new Mock<IDBCustomer>();
            var (mockDBConnection, mockDbTransaction) = PredefinedMocks.ConnectionMocks();
            //setup with parameters that would match real parameters
            mockDBCustomer.Setup(s => s.CreateCustomer(mockDBConnection.Object,
                    customer, mockDbTransaction.Object))
                .ReturnsAsync(-1);

            ICustomerData controller = new CustomerDataControl(mockDBCustomer.Object, mockDBConnection.Object);

            // Act
            async Task<int> TestDelegate()
            {
                int result = await controller.CreateCustomer(mockDBConnection.Object, customer, mockDbTransaction.Object);

                return result;
            }

            // Assert
            Assert.ThrowsAsync<Exception>(async () => await TestDelegate());

            return Task.CompletedTask;
        }
    }
}
