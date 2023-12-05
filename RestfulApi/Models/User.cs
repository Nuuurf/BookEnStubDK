namespace RestfulApi.Models
{
    public class User
    {
        public int UserId { get; set; }
        public UserRoles Role { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
    }
}
