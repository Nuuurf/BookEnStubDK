using RestfulApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.API.Utilities {
    public static class TestModels {

        public static Booking BookingValid() {
            return new Booking {
                TimeStart = DateTime.Now,
                TimeEnd = DateTime.Now.AddHours(1),
                Notes = "Delete me please",
                StubId = 1

            };
        }

        public static Booking BookingInvalid() {
            return new Booking {
                TimeStart = DateTime.Now.AddYears(-1000),
                TimeEnd = DateTime.Now.AddHours(1).AddYears(-1000),
                Notes = "Delete me please",
                StubId = 1
            };
        }
    }
}
