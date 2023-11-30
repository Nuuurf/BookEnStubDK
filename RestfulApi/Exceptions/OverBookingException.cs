namespace RestfulApi.Exceptions
{
    public class OverBookingException : Exception
    {
        public OverBookingException() { }

        public OverBookingException(string? message) : base(message)
        {
        }
    }
}
