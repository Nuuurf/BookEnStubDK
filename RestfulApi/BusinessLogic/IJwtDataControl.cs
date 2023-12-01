using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RestfulApi.DTOs;
using RestfulApi.Models;
using System.Text;

namespace RestfulApi.BusinessLogic
{
    public interface IJwtDataControl
    {
        public Task<AccessTokenResponse> Login(UserLoginModel login);

        public Task<AccessTokenResponse> UseRefreshToken(string refreshToken);

    }
}
