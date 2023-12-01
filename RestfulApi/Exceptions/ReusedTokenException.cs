namespace RestfulApi.Exceptions
{
    public class ReusedTokenException : Exception
    {
        public ReusedTokenException() { }

        public ReusedTokenException(string? message) : base(message)
        {
        }
    }
}
