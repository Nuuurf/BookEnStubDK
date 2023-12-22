namespace RestfulApi.Exceptions {
    public class EmptyResultException : Exception {
        public EmptyResultException() { }

        public EmptyResultException(string? message) : base(message) {
        }
    }
}
