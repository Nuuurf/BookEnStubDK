using Microsoft.AspNetCore.Razor.Hosting;
using RestfulApi.Models;

namespace RestfulApi.DTOs {
    public static class DTO {

        public static Customer ConvertToCustomer(DTOCustomer customerToConvert) {
            return new Customer {
                FullName = customerToConvert.FullName,
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

        public static List<DTONewBooking> ConvertToDTONewBooking(List<Booking> bookingToConvert) {
            return bookingToConvert.Select(itemA => new DTONewBooking {
                TimeStart = itemA.TimeStart,
                TimeEnd = itemA.TimeEnd,
                Notes = itemA.Notes,
            }).ToList();
        }

        public static DTOCustomer ConvertToDTOCustomer(Customer customerToConvert) {
            return new DTOCustomer {
                FullName = customerToConvert.FullName,
                Phone = customerToConvert.Phone,
                Email = customerToConvert.Email
            };
        }
    }
}
