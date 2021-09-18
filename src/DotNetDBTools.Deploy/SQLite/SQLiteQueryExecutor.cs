using System.Collections.Generic;
using Dapper;
using DotNetDBTools.Deploy.Common;
using Microsoft.Data.Sqlite;

namespace DotNetDBTools.Deploy.SQLite
{
    public class SQLiteQueryExecutor : IQueryExecutor
    {
        private readonly string _connectionString;

        public SQLiteQueryExecutor(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int Execute(IQuery query)
        {
            using SqliteConnection connection = new(_connectionString);
            DynamicParameters dapperParameters = MapToDapperParameters(query.Parameters);
            int count = connection.Execute(query.Sql, dapperParameters);
            return count;
        }

        public IEnumerable<TOut> Query<TOut>(IQuery query)
        {
            using SqliteConnection connection = new(_connectionString);
            DynamicParameters dapperParameters = MapToDapperParameters(query.Parameters);
            IEnumerable<TOut> results = connection.Query<TOut>(query.Sql, dapperParameters);
            return results;
        }

        public TOut QuerySingleOrDefault<TOut>(IQuery query)
        {
            using SqliteConnection connection = new(_connectionString);
            DynamicParameters dapperParameters = MapToDapperParameters(query.Parameters);
            TOut result = connection.QuerySingleOrDefault<TOut>(query.Sql, dapperParameters);
            return result;
        }

        private static DynamicParameters MapToDapperParameters(IEnumerable<QueryParameter> parameters)
        {
            DynamicParameters dapperParameters = new();
            foreach (QueryParameter parameter in parameters)
                dapperParameters.Add(parameter.Name, parameter.Value);
            return dapperParameters;
        }
    }
}
