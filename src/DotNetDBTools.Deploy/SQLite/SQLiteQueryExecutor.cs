using System;
using System.Collections.Generic;
using System.Data.Common;
using Dapper;
using DotNetDBTools.Deploy.Core;
using Microsoft.Data.Sqlite;

namespace DotNetDBTools.Deploy.SQLite
{
    internal class SQLiteQueryExecutor : IQueryExecutor
    {
        private readonly string _connectionString;
        private DbTransaction _transaction;

        public SQLiteQueryExecutor(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void BeginTransaction()
        {
            SqliteConnection connection = new(_connectionString);
            connection.Open();
            connection.Execute("PRAGMA foreign_keys=off;");
            _transaction = connection.BeginTransaction();
        }

        public void CommitTransaction()
        {
            DbConnection connection = _transaction.Connection;
            _transaction.Commit();
            _transaction.Dispose();
            connection.Dispose();
            _transaction = null;
        }

        public void RollbackTransaction()
        {
            DbConnection connection = _transaction.Connection;
            try
            {
                _transaction.Rollback();
            }
            catch (InvalidOperationException)
            {
            }
            _transaction.Dispose();
            connection?.Dispose();
            _transaction = null;
        }

        public int Execute(IQuery query)
        {
            DynamicParameters dapperParameters = MapToDapperParameters(query.Parameters);
            if (_transaction is not null)
                return _transaction.Connection.Execute(query.Sql, dapperParameters, _transaction);
            using SqliteConnection connection = new(_connectionString);
            return connection.Execute(query.Sql, dapperParameters);
        }

        public IEnumerable<TOut> Query<TOut>(IQuery query)
        {
            DynamicParameters dapperParameters = MapToDapperParameters(query.Parameters);
            if (_transaction is not null)
                return _transaction.Connection.Query<TOut>(query.Sql, dapperParameters, _transaction);
            using SqliteConnection connection = new(_connectionString);
            return connection.Query<TOut>(query.Sql, dapperParameters);
        }

        public TOut QuerySingleOrDefault<TOut>(IQuery query)
        {
            DynamicParameters dapperParameters = MapToDapperParameters(query.Parameters);
            if (_transaction is not null)
                return _transaction.Connection.QuerySingleOrDefault<TOut>(query.Sql, dapperParameters, _transaction);
            using SqliteConnection connection = new(_connectionString);
            return connection.QuerySingleOrDefault<TOut>(query.Sql, dapperParameters);
        }

        private static DynamicParameters MapToDapperParameters(IEnumerable<QueryParameter> parameters)
        {
            DynamicParameters dapperParameters = new();
            foreach (QueryParameter parameter in parameters)
                dapperParameters.Add(parameter.Name, parameter.Value, parameter.Type);
            return dapperParameters;
        }
    }
}
