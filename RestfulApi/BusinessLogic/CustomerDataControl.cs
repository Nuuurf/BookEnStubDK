
using RestfulApi.DAL;
using System.Data;

namespace RestfulApi.BusinessLogic {
    public class CustomerDataControl : ICustomerData {
        private readonly IDBCustomer _dbCustomer;
        private readonly IDbConnection _connection;

        public CustomerDataControl(IDBCustomer dbCustomer, IDbConnection connection) {
            _dbCustomer = dbCustomer;
            _connection = connection;
        }

        public Task<bool> AssociateCustomerWithBookingOrder(IDbConnection conn, int inBookingOrderId, int inCustomerId, IDbTransaction trans = null) {
            throw new NotImplementedException();
        }

        public async Task<int> CreateCustomer(IDbConnection conn, string cname, string cphone, string cemail, IDbTransaction trans = null) {
            int result = -1;
            //we assume that inputs are sanitiezed somewhere before here
            try {
                result = await _dbCustomer.CreateCustomer(conn, cname, cphone, cemail, trans);
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
