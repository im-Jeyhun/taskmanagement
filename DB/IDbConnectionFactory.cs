using Microsoft.Data.SqlClient;

namespace TaskManagamanet.DB
{
    public interface IDbConnectionFactory
    {
        SqlConnection CreateConnection();
    }
}
