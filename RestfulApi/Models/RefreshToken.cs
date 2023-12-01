namespace RestfulApi.Models
{
    public class RefreshToken
    {
        public Guid TokenId { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
        public DateTime IssuedUtc { get; set; }
        public DateTime ExpiresUtc { get; set; }
        public bool Revoked { get; set; }
        public bool Used { get; set; }
    }
}
