using System.Data;

namespace RestfulApi.DAL {
    public interface IDBCustomer {
        public Task<int> CreateCustomer(IDbConnection conn, string name, string phone, string email, IDbTransaction trans);

        public Task<bool> AssociateCustomerWithBookingOrder(IDbConnection conn, int bookingOrderId, int customerId, IDbTransaction trans);
    }
}
