using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Dapper;

namespace DotNetDBTools.Deploy.Core
{
    internal abstract class QueryExecutor : IQueryExecutor
    {
        private readonly DbConnection _connection;
        private DbTransaction _transaction;

        protected QueryExecutor(DbConnection connection)
        {
            _connection = connection;
        }

        public void BeginTransaction()
        {
            OpenConnectionIfNotOpened();
            BeforeBeginTransaction(_connection);
            _transaction = _connection.BeginTransaction();
        }
        protected virtual void BeforeBeginTransaction(DbConnection connection) { }

        public void CommitTransaction()
        {
            _transaction.Commit();
            _transaction.Dispose();
            _transaction = null;
        }

        public void RollbackTransaction()
        {
            try
            {
                _transaction.Rollback();
            }
            catch (InvalidOperationException)
            {
            }
            _transaction.Dispose();
            _transaction = null;
        }

        public int Execute(IQuery query)
        {
            DynamicParameters dapperParameters = MapToDapperParameters(query.Parameters);
            if (_transaction is not null)
                return _transaction.Connection.Execute(query.Sql, dapperParameters, _transaction);

            OpenConnectionIfNotOpened();
            return _connection.Execute(query.Sql, dapperParameters);
        }

        public IEnumerable<TOut> Query<TOut>(IQuery query)
        {
            DynamicParameters dapperParameters = MapToDapperParameters(query.Parameters);
            if (_transaction is not null)
                return _transaction.Connection.Query<TOut>(query.Sql, dapperParameters, _transaction);

            OpenConnectionIfNotOpened();
            return _connection.Query<TOut>(query.Sql, dapperParameters);
        }

        public TOut QuerySingleOrDefault<TOut>(IQuery query)
        {
            DynamicParameters dapperParameters = MapToDapperParameters(query.Parameters);
            if (_transaction is not null)
                return _transaction.Connection.QuerySingleOrDefault<TOut>(query.Sql, dapperParameters, _transaction);

            OpenConnectionIfNotOpened();
            return _connection.QuerySingleOrDefault<TOut>(query.Sql, dapperParameters);
        }

        private void OpenConnectionIfNotOpened()
        {
            if (_connection.State != ConnectionState.Open)
                _connection.Open();
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
