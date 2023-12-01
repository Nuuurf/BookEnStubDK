using Dapper;
using RestfulApi.Models;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.SignalR;
using System.Data.Common;
using System.Transactions;

namespace RestfulApi.DAL
{
    public class DBRefreshToken : IDBRefreshToken
    {
        public async Task<User> FindUser(IDbConnection conn, string username, IDbTransaction transaction = null!)
        {
            string query = "SELECT * FROM [User] WHERE Username = @Username";
            var parameter = new { Username = username };
            var result = await conn.QueryAsync<User>(query, parameter, transaction: transaction);

            return result.ElementAt(0);
        }

        public async Task AddToken(IDbConnection conn, RefreshToken token, IDbTransaction transaction = null!)
        {
            string query
                = "INSERT INTO [RefreshTokens] (UserId, Token, IssuedUtc, ExpiresUtc) VALUES (@UserId, @Token, @IssuedUtc, @ExpiresUtc)";

            var parameters = new
            {
                UserId = token.UserId,
                Token = token.Token,
                IssuedUtc = token.IssuedUtc,
                ExpiresUtc = token.ExpiresUtc
            };

            var result = await conn.QueryAsync(query, parameters, transaction: transaction);
        }

        public async Task<(RefreshToken, AuthTokenClaims)> FindToken(IDbConnection conn, string token, IDbTransaction transaction = null!)
        {
            string query = @"
                SELECT rt.*, u.UserId as ID, u.Username
                FROM RefreshTokens rt
                INNER JOIN [User] u ON rt.UserId = u.UserId
                WHERE rt.Token = @Token";

            var parameters = new { Token = token };
            var result = await conn.QueryAsync<RefreshToken, AuthTokenClaims, (RefreshToken, AuthTokenClaims)>(
                query,
                (refreshToken, user) => (refreshToken, user),
                parameters,
                transaction: transaction,
                splitOn: "ID");

            return result.FirstOrDefault();
        }


        public async Task RevokeAllTokensForUser(IDbConnection conn, string userId, IDbTransaction transaction = null!)
        {
            string query = "UPDATE RefreshTokens SET Revoked = 1 WHERE UserId = @UserId";
            var parameters = new { UserId = userId };
            var rowsAffected = await conn.ExecuteAsync(query, parameters, transaction: transaction);
        }

        public async Task SetTokenAsUsed(IDbConnection conn, string token, IDbTransaction transaction = null!)
        {
            string query = "UPDATE [dbo].[RefreshTokens] SET Used = 1 WHERE Token = @Token";
            var parameters = new { Token = token };
            var rowsAffected = await conn.ExecuteAsync(query, parameters, transaction: transaction);
        }
     }
}
