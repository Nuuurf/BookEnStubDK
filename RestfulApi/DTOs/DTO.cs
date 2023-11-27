using Microsoft.AspNetCore.Razor.Hosting;
using RestfulApi.Models;

namespace RestfulApi.DTOs {
    public static class DTO {

        public static Customer ConvertToCustomer(DTOCustomer customerToConvert) {
            return new Customer {
                FirstName = customerToConvert.FullName,
                Phone = customerToConvert.Phone,
                Email = customerToConvert.Email
            };
        }

        public static List<Booking> ConvertToBookingList(List<DTONewBooking> bookingToConvert) {
            return bookingToConvert.Select(itemA => new Booking {
                TimeStart = itemA.TimeStart,
                TimeEnd = itemA.TimeEnd,
                Notes = itemA.Notes,
            }).ToList();
        }
    }
}
