using System.Net.Mail;
using System.Net;
using RestfulApi.Models;
using System.Text;

namespace RestfulApi.BusinessLogic
{
    public class EmailDataControl : IEmailDataControl
    {
        private readonly IConfiguration _configuration;

        public EmailDataControl(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmail(string toEmail, List<Booking> bookings, Customer customer, int bookingOrderId)
        {
            string subject = $"Booking Confirmation {bookingOrderId}";
            string body = BuildBookingConfirmationEmail(bookings, customer, bookingOrderId);
            // Basic configuration validation (optional but recommended)
            var emailSettings = _configuration.GetSection("EmailSettings");
            if (string.IsNullOrEmpty(emailSettings["MailServer"]) ||
                string.IsNullOrEmpty(emailSettings["MailPort"]) ||
                string.IsNullOrEmpty(emailSettings["Sender"]) ||
                string.IsNullOrEmpty(emailSettings["Password"]))
            {
                throw new InvalidOperationException("Email settings are not properly configured.");
            }

            // Using statement ensures the SmtpClient is disposed of correctly
            using (var client = new SmtpClient
                   {
                       Host = emailSettings["MailServer"],
                       Port = int.Parse(emailSettings["MailPort"]),
                       EnableSsl = true,
                       Timeout = 10000,
            Credentials = new NetworkCredential(emailSettings["Sender"], emailSettings["Password"])
                   })
            using (var mailMessage = new MailMessage
                   {
                       From = new MailAddress(emailSettings["Sender"], emailSettings["SenderName"]),
                       Subject = subject,
                       Body = body,
                       IsBodyHtml = true
                   })
            {
                mailMessage.To.Add(toEmail);

                try
                {
                    await client.SendMailAsync(mailMessage);
                }
                catch (Exception ex)
                {
                    // Log the exception, throw if necessary, or handle it based on your application's needs
                    // For example: _logger.LogError("Error sending email: {ExceptionMessage}", ex.Message);
                    throw; // Re-throwing the exception to be handled by the caller
                }
            }
        }
        private string BuildBookingConfirmationEmail(List<Booking> bookings, Customer customer, int bookingOrderId)
        {
            var sb = new StringBuilder();
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("<style>");
            sb.Append("table { width: 100%; border-collapse: collapse; }");
            sb.Append("th, td { border: 1px solid #ddd; padding: 8px; text-align: left; }");
            sb.Append("th { background-color: #f2f2f2; }");
            sb.Append("</style>");
            sb.Append("</head>");
            sb.Append("<body>");
            sb.Append($"<h1>Booking Confirmation for {customer.FullName}</h1>");
            sb.Append($"<p>Booking Order ID: {bookingOrderId}</p>");

            sb.Append("<table>");
            sb.Append("<tr><th>Date/Time</th><th>Stub</th></tr>");
            foreach (var booking in bookings)
            {
                sb.Append($"<tr><td>{booking.TimeStart}</td><td>{booking.StubId}</td></tr>");
            }
            sb.Append("</table>");

            sb.Append("</body>");
            sb.Append("</html>");

            return sb.ToString();
        }


    }
}
