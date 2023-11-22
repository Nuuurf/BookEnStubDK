﻿using Moq;
using RestfulApi.BusinessLogic;
using RestfulApi.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.API.BusinessLogic {
    public class CreateCustomerTest {

        [Test]
        public async Task CreateCustomer_ShouldReturnPassValue() {
            //Arrange

            int result = -1;
            string name = "lars";
            string phone = "88888888";
            string email = "somethingmail@noget.dk";

            var mockDBCustomer = new Mock<IDBCustomer>();
            var mockDBConnection = new Mock<IDbConnection>();
            var mockDbTransaction = new Mock<IDbTransaction>();
            //setup with parameters that would match real parameters
            mockDBCustomer.Setup(s => s.CreateCustomer(
                It.IsAny<IDbConnection>(),
                It.Is<string>(s => !string.IsNullOrEmpty(s) && s.Length >= 1 && s.Length <= 100),
                It.Is<string>(s => !string.IsNullOrEmpty(s) && s.Length == 8 || s.Length == 10),
                It.Is<string>(s => !string.IsNullOrEmpty(s) && s.Length >= 1 && s.Length <= 100),
                It.IsAny<IDbTransaction>()
                )).ReturnsAsync(1);

                // Setup mock transaction behavior
            mockDBConnection.Setup(conn => conn.BeginTransaction(It.IsAny<IsolationLevel>()))
                .Returns(mockDbTransaction.Object);
            mockDbTransaction.Setup(trans => trans.Commit());
            mockDbTransaction.Setup(trans => trans.Rollback());

            ICustomerData controller = new CustomerDataControl(mockDBCustomer.Object, mockDBConnection.Object);

            //Act
            result = await controller.CreateCustomer(mockDBConnection.Object, name, phone, email, mockDbTransaction.Object);
            //Assert

            Assert.Greater(result, 0);
        }

        [Test]
        public async Task CreateCustomer_ShouldCauseException() {
            //Arrange

            int result = -1;
            string name = "lars";
            string phone = "88888888";
            string email = "somethingmail@noget.dk";

            var mockDBCustomer = new Mock<IDBCustomer>();
            var mockDBConnection = new Mock<IDbConnection>();
            var mockDbTransaction = new Mock<IDbTransaction>();
            //setup with parameters that would match real parameters
            mockDBCustomer.Setup(s => s.CreateCustomer(
                It.IsAny<IDbConnection>(),
                It.Is<string>(s => !string.IsNullOrEmpty(s) && s.Length >= 1 && s.Length <= 100),
                It.Is<string>(s => !string.IsNullOrEmpty(s) && s.Length == 8 || s.Length == 10),
                It.Is<string>(s => !string.IsNullOrEmpty(s) && s.Length >= 1 && s.Length <= 100),
                It.IsAny<IDbTransaction>()
                )).ReturnsAsync(-1);

            // Setup mock transaction behavior
            mockDBConnection.Setup(conn => conn.BeginTransaction(It.IsAny<IsolationLevel>()))
                .Returns(mockDbTransaction.Object);
            mockDbTransaction.Setup(trans => trans.Commit());
            mockDbTransaction.Setup(trans => trans.Rollback());

            ICustomerData controller = new CustomerDataControl(mockDBCustomer.Object, mockDBConnection.Object);

            // Act
            Func<Task<int>> testDelegate = async () =>
            {
                int result = await controller.CreateCustomer(mockDBConnection.Object, name, phone, email, mockDbTransaction.Object);
                return result;
            };

            // Assert
            Assert.ThrowsAsync<Exception>(async () => await testDelegate());
        }
    }
}
