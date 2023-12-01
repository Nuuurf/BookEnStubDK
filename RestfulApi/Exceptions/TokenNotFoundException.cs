namespace RestfulApi.Exceptions
{
    public class TokenNotFoundException : Exception
    {
        public TokenNotFoundException() { }

        public TokenNotFoundException(string? message) : base(message)
        {
        }
    }
}
