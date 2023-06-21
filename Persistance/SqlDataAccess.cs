using Microsoft.Data.SqlClient;
using System.Data;

namespace FinancialTracker.Persistance
{
    public interface ISqlDataAccess
    {
        IDbConnection GetConnection();
    }

    public class SqlDataAccess : ISqlDataAccess
    {
        private readonly IDbConnection _connection;

        public SqlDataAccess(IConfiguration configuration, string connectionString = "Default")
        {
            _connection = new SqlConnection(configuration.GetConnectionString(connectionString));
        }

        public IDbConnection GetConnection()
            => _connection;
    }
}
