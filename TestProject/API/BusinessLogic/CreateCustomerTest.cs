﻿using Moq;
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

        [Test]
        public async Task CreateCustomer_ShouldReturnPassValue() {
            //Arrange

            int result = -1;

            Customer customer = new Customer {
                FirstName = "Lars",
                Phone = "88888888",
                Email = "somethingmail@noget.dk"
            };

            var mockDBCustomer = new Mock<IDBCustomer>();
            var (mockDBConnection, mockDbTransaction) = PredefinedMocks.ConnectionMocks(); ;
            //setup with parameters that would match real parameters
            mockDBCustomer.Setup(s => s.CreateCustomer(
                It.IsAny<IDbConnection>(),
                It.IsAny<Customer>(),
                It.IsAny<IDbTransaction>()
                )).ReturnsAsync(1);

                ICustomerData controller = new CustomerDataControl(mockDBCustomer.Object, mockDBConnection.Object);

            //Act
            result = await controller.CreateCustomer(mockDBConnection.Object, customer, mockDbTransaction.Object);
            //Assert

            Assert.Greater(result, 0);
        }

        [Test]
        public async Task CreateCustomer_ShouldCauseException() {
            //Arrange

            int result = -1;

            Customer customer = new Customer {
                FirstName = "Lars",
                Phone = "88888888",
                Email = "somethingmail@noget.dk"
            };
            
            var mockDBCustomer = new Mock<IDBCustomer>();
            var (mockDBConnection, mockDbTransaction) = PredefinedMocks.ConnectionMocks();
            //setup with parameters that would match real parameters
            mockDBCustomer.Setup(s => s.CreateCustomer(
                It.IsAny<IDbConnection>(),
                It.IsAny<Customer>(),
                It.IsAny<IDbTransaction>()
                )).ReturnsAsync(-1);

            ICustomerData controller = new CustomerDataControl(mockDBCustomer.Object, mockDBConnection.Object);

            // Act
            Func<Task<int>> testDelegate = async () =>
            {
                int result = await controller.CreateCustomer(mockDBConnection.Object, customer, mockDbTransaction.Object);
                return result;
            };

            // Assert
            Assert.ThrowsAsync<Exception>(async () => await testDelegate());
        }
    }
}
