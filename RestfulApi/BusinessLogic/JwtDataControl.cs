using System.Data;
using Microsoft.IdentityModel.Tokens;
using RestfulApi.DAL;
using RestfulApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using RestfulApi.DTOs;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using RestfulApi.Exceptions;

namespace RestfulApi.BusinessLogic
{
    public class JwtDataControl : IJwtDataControl
    {
        private readonly IConfiguration _configuration;
        private readonly IDBRefreshToken _dbRefreshToken;
private readonly IDbConnection _dbConnection;
        public JwtDataControl(IConfiguration configuration, IDBRefreshToken dbRefreshToken, IDbConnection dbConnection)
        {
            _configuration = configuration;
            _dbRefreshToken = dbRefreshToken;
            _dbConnection = dbConnection;
        }

        public async Task<AccessTokenResponse> Login(UserLoginModel login)
        {
            User? user = await _dbRefreshToken.FindUser(_dbConnection, login.Username.ToLower());
            if (user != null! && VerifyPassword(user, login.Password))
            {
                AuthTokenClaims claims = new AuthTokenClaims { ID = user.UserId.ToString(), Role = user.Role, Username = user.Username };
                string newAccessToken = GenerateToken(_configuration, claims);
                string newRefreshToken = GenerateRefreshToken();
                await SaveRefreshToken(user.UserId.ToString(), newRefreshToken);

                AccessTokenResponse responseToken = new AccessTokenResponse
                {
                    RefreshToken = newRefreshToken,
                    AccessToken = newAccessToken
                };
                return responseToken;
            }
            return null!;
        }

        public async Task<AccessTokenResponse> Register(UserCreationModel newUserModel)
        {
            string salt = GenerateSalt();
            User newUser = new User
            {
                Role = newUserModel.Role,
                Username = newUserModel.Username,
                PasswordSalt = salt,
                PasswordHash = HashPassword(newUserModel.Password, salt)
            };

            bool success = await _dbRefreshToken.RegisterUser(_dbConnection, newUser);

            if (success)
            {
                AuthTokenClaims claims = new AuthTokenClaims { ID = newUser.UserId.ToString(), Role = newUser.Role, Username = newUser.Username };
                string newAccessToken = GenerateToken(_configuration, claims);
                string newRefreshToken = GenerateRefreshToken();
                await SaveRefreshToken(newUser.UserId.ToString(), newRefreshToken);

                AccessTokenResponse responseToken = new AccessTokenResponse
                {
                    RefreshToken = newRefreshToken,
                    AccessToken = newAccessToken
                };
                return responseToken;
            }
            return null!;
        }

        public async Task<AccessTokenResponse> UseRefreshToken(string refreshToken)
        {
            AuthTokenClaims claims = null!;

            using (IDbTransaction transaction = _dbConnection.BeginTransaction())
            {
                try
                {
                    var (token, foundClaims)
                        = await _dbRefreshToken.FindToken(_dbConnection, refreshToken, transaction);

                    if (token == null)
                    {
                        throw new TokenNotFoundException("Token Not Found");
                        }
                    if (token.Used || token.Revoked || token.ExpiresUtc < DateTime.UtcNow)
                    {
                        await _dbRefreshToken.RevokeAllTokensForUser(_dbConnection,
                            token.UserId.ToString(),
                            transaction);

                        throw new ReusedTokenException();
                    }
                    else
                    {
                        await _dbRefreshToken.SetTokenAsUsed(_dbConnection, refreshToken, transaction);
                        claims = foundClaims;
                    }

                    transaction.Commit();

                }
                catch (ReusedTokenException)
                {
                    transaction.Commit();
                    throw;
                }
                catch
                {
transaction.Rollback();
throw;
                }
                }
            string newAccessToken = GenerateToken(_configuration, claims);
            string newRefreshToken = GenerateRefreshToken();
            Task.Run(() => SaveRefreshToken(claims.ID, newRefreshToken));

            AccessTokenResponse responseToken = new AccessTokenResponse
            {
                RefreshToken = newRefreshToken,
                AccessToken = newAccessToken
            };
            return responseToken;
        }

        private string GenerateToken(IConfiguration configuration, AuthTokenClaims user)
        {
            var issuer = configuration["Jwt:Issuer"];
            var audience = configuration["Jwt:Audience"];
            var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);

            var claims = new List<Claim>
            {
                new Claim("Id", user.ID),
                new Claim("name", user.Username),
                new Claim(ClaimTypes.Role, user.Role.GetDescription())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(5),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);

                return Convert.ToBase64String(randomNumber);
            }
        }

        private async Task SaveRefreshToken(string userId, string refreshToken)
        {
            var tokenEntity = new RefreshToken
            {
                UserId = int.Parse(userId),
                Token = refreshToken,
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.AddHours(5)
            };

            await _dbRefreshToken.AddToken(_dbConnection, tokenEntity);
        }

        private bool VerifyPassword(User user, string passwordToVerify)
        {
            string enteredHash = HashPassword(passwordToVerify, user.PasswordSalt);
            return enteredHash == user.PasswordHash;
        }

        private string GenerateSalt()
        {
            byte[] saltBytes = new byte[32];
            using (var provider = new RNGCryptoServiceProvider())
            {
                provider.GetNonZeroBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        private string HashPassword(string password, string salt)
        {
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var saltBytes = Convert.FromBase64String(salt);
            using (var hasher = new HMACSHA256(saltBytes))
            {
                var hashedBytes = hasher.ComputeHash(passwordBytes);
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}
