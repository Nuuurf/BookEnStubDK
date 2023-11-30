using RestfulApi.DTOs;
using RestfulApi.Models;
using System.Data;

namespace RestfulApi.DAL {
    public interface IDBCustomer {
        public Task<int> CreateCustomer(IDbConnection conn, Customer customer, IDbTransaction trans);
        public Task<DTOCustomer> GetCustomer(IDbConnection conn, string phone);

        public Task<bool> AssociateCustomerWithBookingOrder(IDbConnection conn, int bookingOrderId, int customerId, IDbTransaction trans);
    }
}
