using Dapper;
using RestfulApi.DTOs;
using RestfulApi.Models;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace RestfulApi.DAL {
    public class DBCustomer : IDBCustomer {

        //associates customer with bookingOrder using ids for both
        public async Task<bool> AssociateCustomerWithBookingOrder(IDbConnection conn, int inBookingOrderId, int inCustomerId, IDbTransaction trans = null!) {
            bool result = false;
            int[] rawResult = new int[2];

            string scriptIfAssociated = "SELECT Id, Customer_Id_FK FROM BookingOrder WHERE Id = @id";
            string scriptIfNotAssociated = "UPDATE BookingOrder SET Customer_Id_FK = @customerId WHERE Id = @id";

            try {
                var parameters = new { id = inBookingOrderId, customerId = inCustomerId };
                var resultTemp = await conn.QueryFirstOrDefaultAsync<(int Id, int Customer_Id_FK)>(scriptIfAssociated, parameters, transaction: trans);

                //parse values from resultTemp because you cannot directly convert from query to int[]
                try {
                    rawResult[0] = resultTemp.Id;
                    rawResult[1] = resultTemp.Customer_Id_FK;
                }
                catch (Exception ex) { throw new Exception("Some kind of convertion error occured while parsing from query" + ex.Message); }

                if (rawResult.Length == 2) {
                    if (rawResult[0] == 0) {
                        throw new Exception("The BookingOrderId does not match any existing BookingOrders in the database");
                    }
                    else if (rawResult[1] == 0) {
                        await conn.ExecuteAsync(scriptIfNotAssociated, parameters, transaction: trans);
                        result = true;
                    }
                    else {
                        throw new Exception("Booking order is already associated with a customer. Associated id: " + rawResult[1]);
                    }
                }
                else {
                    throw new Exception("Invalid result retrieved from the database");
                }
            }
            catch (Exception ex) {
                throw new Exception($"Error while associating booking order with id: {inBookingOrderId} with customerId: {inCustomerId}\n{ex.Message}");
            }

            return result;
        }

        //Persists customer information in database.
        public async Task<int> CreateCustomer(IDbConnection conn, Customer customer, IDbTransaction trans = null!) {
            int result = -1; // Default fail value
            string scriptIfExists = "SELECT Id FROM customer WHERE phone = @phone AND email = @email";
            string scriptInsertCustomer = "INSERT INTO customer (name, phone, email) OUTPUT INSERTED.Id VALUES (@name, @phone, @email)";

            try {
                var parameters = new { phone = customer.Phone, email = customer.Email};
                result = await conn.QueryFirstOrDefaultAsync<int>(scriptIfExists, parameters, transaction: trans);

                if (result == 0) {
                    var parametersCreateNew = new { name = customer.FullName, phone = customer.Phone, email = customer.Email };
                    result = (int)(await conn.ExecuteScalarAsync(scriptInsertCustomer, parametersCreateNew, transaction: trans))!;
                }
            }
            catch (Exception ex) {
                throw new Exception("An error occurred while creating or finding a customer.\n" + ex.Message);
            }

            return result;
        }

        public async Task<Customer> GetCustomer(IDbConnection conn, string phoneInput, IDbTransaction trans = null!)
        {
            Customer? result = null;
            string script = "SELECT name as FullName, phone, email FROM customer where phone = @phone";

            try
            {
                var parameters = new { phone = phoneInput};
                result = await conn.QueryFirstOrDefaultAsync<Customer>(script, parameters, transaction: trans);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating or finding a customer.\n" + ex.Message);
            }

            return result;
        }
    }
}
