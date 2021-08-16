using System.Collections.Generic;
using Dapper;
using Microsoft.Data.Sqlite;

namespace DotNetDBTools.DeployInteractor.SQLite
{
    public class SQLiteQueryExecutor : IQueryExecutor
    {
        private readonly string _connectionString;

        public SQLiteQueryExecutor(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int Execute(string query, params QueryParameter[] parameters)
        {
            using SqliteConnection connection = new(_connectionString);
            DynamicParameters dapperParameters = MapToDapperParameters(parameters);
            int count = connection.Execute(query, dapperParameters);
            return count;
        }

        public IEnumerable<TOut> Query<TOut>(string query, params QueryParameter[] parameters)
        {
            using SqliteConnection connection = new(_connectionString);
            DynamicParameters dapperParameters = MapToDapperParameters(parameters);
            IEnumerable<TOut> results = connection.Query<TOut>(query, dapperParameters);
            return results;
        }

        public TOut QuerySingleOrDefault<TOut>(string query, params QueryParameter[] parameters)
        {
            using SqliteConnection connection = new(_connectionString);
            DynamicParameters dapperParameters = MapToDapperParameters(parameters);
            TOut result = connection.QuerySingleOrDefault<TOut>(query, dapperParameters);
            return result;
        }

        private static DynamicParameters MapToDapperParameters(QueryParameter[] parameters)
        {
            DynamicParameters dapperParameters = new();
            foreach (QueryParameter parameter in parameters)
                dapperParameters.Add(parameter.Name, parameter.Value);
            return dapperParameters;
        }
    }
}
