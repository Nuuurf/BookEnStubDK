﻿using RestfulApi.DTOs;
using RestfulApi.Models;
using System.Data;

namespace RestfulApi.BusinessLogic {
    public interface ICustomerData {

        public Task<int> CreateCustomer(IDbConnection conn, Customer customer, IDbTransaction trans = null!);
        public Task<DTOCustomer> GetCustomer(string phone);
        public Task<bool> AssociateCustomerWithBookingOrder(IDbConnection conn, int inBookingOrderId, int inCustomerId, IDbTransaction trans = null!);
    }
}
