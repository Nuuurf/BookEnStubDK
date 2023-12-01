using System.Data;
using System.Security.Claims;
using RestfulApi.Models;

namespace RestfulApi.DAL
{
    public interface IDBRefreshToken
    {
        public Task AddToken(IDbConnection conn, RefreshToken token, IDbTransaction transaction = null!);

        public Task<User> FindUser(IDbConnection conn, string username, IDbTransaction transaction = null!);

        public Task<(RefreshToken, AuthTokenClaims)> FindToken(IDbConnection conn, string token, IDbTransaction transaction = null!);

        public Task RevokeAllTokensForUser(IDbConnection conn, string userId, IDbTransaction transaction = null!);

        public Task SetTokenAsUsed(IDbConnection conn, string token, IDbTransaction transaction = null!);
    }
}
