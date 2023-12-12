using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp.Models
{
    public class BookingRequest
    {
        public List<NewBooking> Appointments { get; set; }
        public Customer? Customer { get; set; }

        public BookingRequest()
        {
            Appointments = new List<NewBooking>();
            Customer = null;
        }
    }
}
