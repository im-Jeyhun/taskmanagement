using Microsoft.Data.SqlClient;
using System.Data;
using TaskManagamanet.DB;
using TaskManagamanet.Entities;
using TaskManagamanet.Repositories.Interfaces;

namespace TaskManagamanet.Repositories.Implementations
{
    public sealed class TokenRepository : ITokenRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public TokenRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task CreateAsync(
            int userId,
            string tokenHash,
            DateTime expiresAt)
        {
            await using var connection =
                _dbConnectionFactory.CreateConnection();

            await connection.OpenAsync();

            await using var command = new SqlCommand(
                "dbo.P_UPSERT_USER_TOKEN",
                connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@USER_ID", SqlDbType.Int)
                .Value = userId;

            command.Parameters.Add("@TOKEN_HASH", SqlDbType.NVarChar, 500)
                .Value = tokenHash;

            command.Parameters.Add("@EXPIRES_AT", SqlDbType.DateTime2)
                .Value = expiresAt;

            await command.ExecuteNonQueryAsync();
        }

        public async Task<UserToken?> GetActiveByHashAsync(
            string tokenHash)
        {
            await using var connection =
                _dbConnectionFactory.CreateConnection();

            await connection.OpenAsync();

            await using var command = new SqlCommand(
                "dbo.P_GET_ACTIVE_USER_TOKEN",
                connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@TOKEN_HASH", SqlDbType.NVarChar, 500)
                .Value = tokenHash;

            await using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

            return new UserToken
            {
                Id = reader.GetInt64(reader.GetOrdinal("ID")),
                UserId = reader.GetInt32(reader.GetOrdinal("USER_ID")),
                TokenHash = reader.GetString(reader.GetOrdinal("TOKEN_HASH")),
                ExpiresAt = reader.GetDateTime(reader.GetOrdinal("EXPIRES_AT")),
                IsRevoked = reader.GetBoolean(reader.GetOrdinal("IS_REVOKED")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CREATED_AT"))
            };
        }

        public async Task RevokeAllForUserAsync(int userId)
        {
            await using var connection =
                _dbConnectionFactory.CreateConnection();

            await connection.OpenAsync();

            await using var command = new SqlCommand(
                "dbo.P_REVOKE_ALL_USER_TOKENS",
                connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@USER_ID", SqlDbType.Int)
                .Value = userId;

            await command.ExecuteNonQueryAsync();
        }

        public async Task<bool> RevokeAsync(long tokenId)
        {
            await using var connection =
                _dbConnectionFactory.CreateConnection();

            await connection.OpenAsync();

            await using var command = new SqlCommand(
                "dbo.P_REVOKE_USER_TOKEN",
                connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@ID", SqlDbType.BigInt)
                .Value = tokenId;

            var result = await command.ExecuteScalarAsync();

            return result is not null
                && result != DBNull.Value
                && Convert.ToBoolean(result);
        }
    }
}