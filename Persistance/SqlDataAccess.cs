using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;

namespace FinancialTracker.Persistance
{
    public interface ISqlDataAccess
    {
        Task<IEnumerable<TReturn>> LoadData<TReturn, TParam>(string storedProcedure, TParam parameters, string connectionString = "Default");
        Task<int> SaveData<TParam>(string storedProcedure, TParam parameters, string connectionString = "Default");
    }

    public class SqlDataAccess : ISqlDataAccess
    {
        private readonly IConfiguration _configuration;

        public SqlDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<TReturn>> LoadData<TReturn, TParam>(string storedProcedure, TParam parameters, string connectionString = "Default")
        {
            await using var connection = new SqlConnection(_configuration.GetConnectionString(connectionString));
            return await connection.QueryAsync<TReturn>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> SaveData<TParam>(string storedProcedure, TParam parameters, string connectionString = "Default")
        {
            await using var connection = new SqlConnection(_configuration.GetConnectionString(connectionString));
            return await connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
        }
    }
}
