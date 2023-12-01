using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp.Models
{
    public class BookingRequestFilter
    {
        public DateTime Start { get; set; } = DateTime.Now.Date;
        public DateTime End { get; set; } = DateTime.Now.Date.AddDays(1);
        public bool ShowAvailable { get; set; } = false;
        public int? StubId { get; set; } = null;
        public int? OrderId { get; set; } = null;
        public string? CustomerEmail { get; set; } = "";
        public string? CustomerPhone { get; set; } = "";

        public BookingRequestFilter()
        {
            
        }

        public BookingRequestFilter(DateTime start, DateTime end, bool showAvailable, int? stubId, int? orderId, string? customerEmail, string? customerPhone)
        {
            Start = start.Date;
            End = end.Date.AddDays(1);
            ShowAvailable = showAvailable;
            StubId = stubId;
            OrderId = orderId;
            CustomerEmail = customerEmail;
            CustomerPhone = customerPhone;
        }
    }
}
