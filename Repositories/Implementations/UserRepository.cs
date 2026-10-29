using Microsoft.Data.SqlClient;
using System.Data;
using TaskManagamanet.DB;
using TaskManagamanet.Entities;
using TaskManagamanet.Repositories.Interfaces;

namespace TaskManagamanet.Repositories.Implementations
{

    public sealed class UserRepository : IUserRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public UserRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<User?> GetByIdAsync(int userId)
        {
            await using var connection = _dbConnectionFactory.CreateConnection();
            await connection.OpenAsync();

            await using var command = new SqlCommand(
                "dbo.P_GET_USER_BY_ID",
                connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@USER_ID", SqlDbType.Int).Value = userId;

            await using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

            return MapUser(reader);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            await using var connection = _dbConnectionFactory.CreateConnection();
            await connection.OpenAsync();

            await using var command = new SqlCommand(
                "dbo.P_GET_USER_BY_USERNAME",
                connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@USER_NAME", SqlDbType.NVarChar, 100).Value =
                username.Trim();

            await using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

            return MapUser(reader);
        }

        public async Task<bool> RegisterFailedLoginAsync(
            int userId,
            int maxAttempts,
            int lockoutMinutes)
        {
            await using var connection = _dbConnectionFactory.CreateConnection();
            await connection.OpenAsync();

            await using var command = new SqlCommand(
                "dbo.P_REGISTER_FAILED_LOGIN",
                connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@USER_ID", SqlDbType.Int).Value = userId;
            command.Parameters.Add("@MAX_ATTEMPTS", SqlDbType.Int).Value = maxAttempts;
            command.Parameters.Add("@LOCKOUT_MINUTES", SqlDbType.Int).Value =
                lockoutMinutes;

            var isLockedParameter = command.Parameters.Add(
                "@IS_LOCKED",
                SqlDbType.Bit);

            isLockedParameter.Direction = ParameterDirection.Output;

            await command.ExecuteNonQueryAsync();

            return isLockedParameter.Value != DBNull.Value
                && Convert.ToBoolean(isLockedParameter.Value);
        }

        public async Task<bool> ResetLoginAttemptsAsync(int userId)
        {
            await using var connection = _dbConnectionFactory.CreateConnection();
            await connection.OpenAsync();

            await using var command = new SqlCommand(
                "dbo.P_RESET_LOGIN_ATTEMPTS",
                connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@USER_ID", SqlDbType.Int).Value = userId;

            var result = await command.ExecuteScalarAsync();

            return result != null
                && result != DBNull.Value
                && Convert.ToBoolean(result);
        }

        public async Task<bool> UpdatePasswordAsync(
            int userId,
            string passHash,
            bool isPassChangeRequired)
        {
            await using var connection = _dbConnectionFactory.CreateConnection();
            await connection.OpenAsync();

            await using var command = new SqlCommand(
                "dbo.P_UPDATE_USER_PASSWORD",
                connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@USER_ID", SqlDbType.Int).Value = userId;

            command.Parameters.Add("@PASS_HASH", SqlDbType.NVarChar, 500).Value =
                passHash;

            command.Parameters.Add("@IS_PASS_CHANGE_REQUIRED", SqlDbType.Bit).Value =
                isPassChangeRequired;

            var result = await command.ExecuteScalarAsync();

            return result != null
                && result != DBNull.Value
                && Convert.ToBoolean(result);
        }

        private static User MapUser(SqlDataReader reader)
        {
            return new User
            {
                UserId = reader.GetInt32(reader.GetOrdinal("USER_ID")),
                UserName = reader.GetString(reader.GetOrdinal("USER_NAME")),
                Name = reader.GetString(reader.GetOrdinal("NAME")),
                LastName = reader.GetString(reader.GetOrdinal("LAST_NAME")),
                FatherName = reader.GetString(reader.GetOrdinal("FATHER_NAME")),

                // Stored procedure PASS_HASH AS PASSWORD qaytarmalıdır.
                Password = GetNullableString(reader, "PASSWORD") ?? string.Empty,

                SystemRoleId = reader.GetInt32(
                    reader.GetOrdinal("SYSTEM_ROLE_ID")),

                PositionId = reader.GetInt32(
                    reader.GetOrdinal("POSITION_ID")),

                DepartmentId = reader.GetInt32(
                    reader.GetOrdinal("DEPARTMENT_ID")),

                SectionId = GetNullableInt(reader, "SECTION_ID"),
                ManagerUserId = GetNullableInt(reader, "MANAGER_USER_ID"),

                IsActive = reader.GetBoolean(reader.GetOrdinal("IS_ACTIVE")),

                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CREATED_AT")),

                UpdatedAt = GetNullableDateTime(reader, "UPDATED_AT")
            };
        }

        private static string? GetNullableString(
            SqlDataReader reader,
            string columnName)
        {
            int ordinal = reader.GetOrdinal(columnName);

            return reader.IsDBNull(ordinal)
                ? null
                : reader.GetString(ordinal);
        }

        private static int? GetNullableInt(
            SqlDataReader reader,
            string columnName)
        {
            int ordinal = reader.GetOrdinal(columnName);

            return reader.IsDBNull(ordinal)
                ? null
                : reader.GetInt32(ordinal);
        }

        private static DateTime? GetNullableDateTime(
            SqlDataReader reader,
            string columnName)
        {
            int ordinal = reader.GetOrdinal(columnName);

            return reader.IsDBNull(ordinal)
                ? null
                : reader.GetDateTime(ordinal);
        }
    }

}
