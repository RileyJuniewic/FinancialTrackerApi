﻿using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;

namespace FinancialTracker.Persistance
{
    public interface ISqlDataAccess
    {
        Task<IEnumerable<T>> LoadData<T, TU>(string storedProcedure, TU parameters, string connectionString = "Default");
        Task<int> SaveData<TU>(string storedProcedure, TU parameters, string connectionString = "Default");
    }

    public class SqlDataAccess : ISqlDataAccess
    {
        public async Task<IEnumerable<T>> LoadData<T, TU>(string storedProcedure, TU parameters, string connectionString = "Default")
        {
            await using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> SaveData<TU>(string storedProcedure, TU parameters, string connectionString = "Default")
        {
            await using var connection = new SqlConnection(connectionString);
            return await connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
        }
    }
}
