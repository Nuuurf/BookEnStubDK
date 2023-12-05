using RestfulApi.Models;

namespace RestfulApi.DTOs
{
    public class UserCreationModel
    {
        public UserRoles Role { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        }
}
