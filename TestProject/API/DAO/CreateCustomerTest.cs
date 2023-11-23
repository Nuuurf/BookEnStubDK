﻿using RestfulApi.DAL;
using RestfulApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TestProject.API.DAO {
    public class CreateCustomerTest {

        private DBCustomer _DbCustomer = new DBCustomer();

        //[SetUp]
        //public void Setup() {
        //    using (IDbConnection con = DBConnection.Instance.GetOpenConnection()) {
        //        string script = "INSERT INTO customer values(Testperson, 73277327, testperson@test.test;";
        //    }
        //}

        [Test]
        public async Task CreateCustomer_ShouldReturnPassValue() {
            //Arrange
            string name = "Lars";
            string phone = "88888888";
            string email = "Lars@lortemail.dk";

            Customer customer = new Customer {
                FirstName = name,
                Phone = phone,
                Email = email
            };
            int result = -1;
            IDbConnection conn = DBConnection.Instance.GetOpenConnection();

            //Act
            using (IDbTransaction trans = conn.BeginTransaction()) {
                result = await _DbCustomer.CreateCustomer(conn, customer, trans);
                trans.Rollback();
            }

            //Assert
            Assert.Greater(result, 0);
        }

        //Maybe refactor method to assert that it throws an exception
        [Test]
        public async Task CreateCustomer_ShouldReturnSameIdDueToExistingInformationInDatabase() {
            // Arrange
            string name = "Lars";
            string phone = "88888887";
            string email = "Lars2@lortemail.dk";

            Customer customer = new Customer {
                FirstName = name,
                Phone = phone,
                Email = email
            };

            int resultCompare = -2;
            int result = -1;
            IDbConnection conn = DBConnection.Instance.GetOpenConnection();

            

            //Act 
            using (IDbTransaction trans = conn.BeginTransaction(System.Data.IsolationLevel.Serializable)) {
                //insert customer first time retrieve unique id.
                resultCompare = await _DbCustomer.CreateCustomer(conn, customer, trans);

                //insert same customer information again, retrieve same id as first time.
                result = await _DbCustomer.CreateCustomer(conn, customer, trans);

                trans.Rollback();
            }

            //Assert
            Assert.AreEqual(resultCompare, result);
        }

        [Test]
        public async Task AssociateCustomerWithBookingOrder_ShouldReturnTrue() {
            //Arrange
            //Make sure that there is a booking order with id 1 and customer with id 1 in test database
            int bookingOrderId = 1;
            int customerId = 21; //for some reason
            bool result = false;
            IDbConnection conn = DBConnection.Instance.GetOpenConnection();

            //Act
            using (IDbTransaction trans = conn.BeginTransaction()) {
                result = await _DbCustomer.AssociateCustomerWithBookingOrder(conn, bookingOrderId, customerId, trans);
                trans.Rollback();
            }
            //Assert
            Assert.True(result);
        }

        [Test]
        public async Task AssociateCustomerWithBookingOrder_ShouldThrowException() {
            // Arrange
            int bookingOrderId = 1;
            int customerId = 1;
            IDbConnection conn = DBConnection.Instance.GetOpenConnection();

            // Act & Assert
            using (IDbTransaction trans = conn.BeginTransaction()) {
                // Use Assert.ThrowsAsync to assert that the method throws an exception

                AsyncTestDelegate testDelegate = async () =>
                    await _DbCustomer.AssociateCustomerWithBookingOrder(conn, bookingOrderId, customerId, trans);

                // Assert that the method throws an exception of a specific type
                Assert.ThrowsAsync<Exception>(testDelegate);

                //Rollback on test database.
                trans.Rollback();
            }
        }
    }
}