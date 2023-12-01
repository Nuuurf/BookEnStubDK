using RestfulApi.DTOs;
using RestfulApi.Models;
using System.Data;
using System.Diagnostics;

namespace RestfulApi.BusinessLogic {
    public interface ICustomerData {

        public Task<int> CreateCustomer(IDbConnection conn, Customer customer, IDbTransaction trans = null!);
        public Task<Customer> GetCustomer(string phone, IDbTransaction trans = null!);
        public Task<bool> AssociateCustomerWithBookingOrder(IDbConnection conn, int inBookingOrderId, int inCustomerId, IDbTransaction trans = null!);
    }
}
