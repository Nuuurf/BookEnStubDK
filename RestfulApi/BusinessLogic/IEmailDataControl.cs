using RestfulApi.Models;

namespace RestfulApi.BusinessLogic
{
    public interface IEmailDataControl
    {
        public Task SendEmail(string toEmail, List<Booking> bookings, Customer customer, int bookingOrderId);
    }
}
