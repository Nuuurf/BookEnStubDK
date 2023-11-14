﻿using RestfulApi.Models;

namespace RestfulApi.BusinessLogic {
    public interface IBookingData {
        
        public Task<bool> CreateBooking(Booking booking);
        
        public Task<List<AvailableBookingsForTimeframe>> GetAvailableBookingsForGivenDate(DateTime date);
        
        public Task<List<Booking>> GetBookingsInTimeslot(DateTime start, DateTime end);

    }
}
