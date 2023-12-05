namespace RestfulApi.Models
{
    public class AuthTokenClaims
    {
        public string ID { get; set; }
public UserRoles Role { get; set; }
        public string Username { get; set; }
    }
}
