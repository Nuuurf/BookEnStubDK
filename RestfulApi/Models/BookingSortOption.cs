using System.ComponentModel;

namespace RestfulApi.Models
{
    public enum BookingSortOption
    {
        [Description("Sort by Booking ID")]
        BookingId,

        [Description("Sort by Date")]
        Date,

        [Description("Sort by Order ID")]
        OrderId,

        [Description("Sort by Customer ID")]
        CustomerId
    }
}
