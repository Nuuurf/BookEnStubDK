﻿using System.Data;
using RestfulApi.Models;

namespace RestfulApi.DAL {
    public interface IDBBooking {

        public Task<int> CreateBooking(IDbConnection conn, Booking booking, int bookingOrderID, IDbTransaction transaction = null!);

        public Task<int> CreateNewBookingOrder(IDbConnection conn, IDbTransaction trans);

        //bool DeleteBooking(int bookingID);

        //bool UpdateBooking(int bookingID, Booking booking);

        public Task<List<Booking>> GetBookingsInTimeslot(IDbConnection conn, BookingRequestFilter req, IDbTransaction transaction = null!);

        public Task<List<int>> GetBookedStubsForHour(IDbConnection conn, DateTime hour, IDbTransaction transaction = null!);

        public Task<List<int>> GetAllStubs(IDbConnection conn, IDbTransaction transaction = null!);


    }
}
