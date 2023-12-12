using RestfulApi.BusinessLogic;
using RestfulApi.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestfulApi.Controllers;
using RestfulApi.Models;
using TestProject.API.Utilities;
using NUnit.Framework.Internal;
using RestfulApi.DTOs;

namespace TestProject.API.IntegrationTest
{
    public class FindCustomerFromPhoneIntegrationTest
    {
        private static IDbConnection _connection = DBConnection.Instance.GetOpenConnection();
        private static IDBCustomer _customer = new DBCustomer();
        private static ICustomerData _customerControl = new CustomerDataControl(_customer, _connection);
        private static CustomerController _controller = new CustomerController(_customerControl);

        [Test]
        public async Task FindCustomerFromPhone_ShouldReturnCustomer_WithValidData()
        {
            //Arrange
            string phone = "73277327";

            //Act
            var result = await _controller.FindCustomerFromPhone(phone);

            //Assert
Assert.NotNull(result);
var okResult = result as OkObjectResult;
var customer = okResult.Value as DTOCustomer;
Assert.NotNull(customer);
Assert.That(customer.Phone, Is.EqualTo(phone));
Assert.That(customer.Email, Is.EqualTo("testperson@test.test"));
Assert.That(customer.FullName, Is.EqualTo("testPerson"));
            }
    }
}
