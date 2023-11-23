
using RestfulApi.DAL;
using RestfulApi.Models;
using System.Data;

namespace RestfulApi.BusinessLogic {
    public class CustomerDataControl : ICustomerData {
        private readonly IDBCustomer _dbCustomer;
        private readonly IDbConnection _connection;

        public CustomerDataControl(IDBCustomer dbCustomer, IDbConnection connection) {
            _dbCustomer = dbCustomer;
            _connection = connection;
        }

        // TODO : Write simple test for fail and success
        public async Task<bool> AssociateCustomerWithBookingOrder(IDbConnection conn, int customerId, int orderId, IDbTransaction trans = null) {
            bool result = false;
            try {
                result = await _dbCustomer.AssociateCustomerWithBookingOrder(conn, orderId, customerId, trans);
            }
            catch {
                throw;
            }
            return result;
        }

        public async Task<int> CreateCustomer(IDbConnection conn, Customer customer, IDbTransaction trans = null) {
            int result = -1;
            //we assume that inputs are sanitiezed somewhere before here
            try {
                result = await _dbCustomer.CreateCustomer(conn, customer, trans);
                //Extra precaution
                if(result <= 0) {
                    throw new Exception("IDBCusomter returned invalid customer id. Id: " + result);
                }
            }
            catch {
                //Pass the exception up
                throw;
            }
            return result;
        }
    }
}
