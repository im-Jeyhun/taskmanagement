using Microsoft.Data.SqlClient;

namespace TaskManagamanet.DB
{
    public sealed class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly IConfiguration _config;

        public DbConnectionFactory(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection CreateConnection()
            => new SqlConnection(_config.GetConnectionString("MsSqlDb"));

    }
}
