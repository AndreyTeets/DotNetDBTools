using System;
using System.Collections.Generic;
using System.Data.Common;
using Dapper;

namespace DotNetDBTools.Deploy.Core
{
    internal abstract class QueryExecutor : IQueryExecutor
    {
        private readonly Func<DbConnection> _createDbConnection;
        private DbTransaction _transaction;

        protected QueryExecutor(Func<DbConnection> createDbConnection)
        {
            _createDbConnection = createDbConnection;
        }

        public void BeginTransaction()
        {
            DbConnection connection = _createDbConnection();
            connection.Open();
            BeforeBeginTransaction(connection);
            _transaction = connection.BeginTransaction();
        }
        protected virtual void BeforeBeginTransaction(DbConnection connection) { }

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
            using DbConnection connection = _createDbConnection();
            return connection.Execute(query.Sql, dapperParameters);
        }

        public IEnumerable<TOut> Query<TOut>(IQuery query)
        {
            DynamicParameters dapperParameters = MapToDapperParameters(query.Parameters);
            if (_transaction is not null)
                return _transaction.Connection.Query<TOut>(query.Sql, dapperParameters, _transaction);
            using DbConnection connection = _createDbConnection();
            return connection.Query<TOut>(query.Sql, dapperParameters);
        }

        public TOut QuerySingleOrDefault<TOut>(IQuery query)
        {
            DynamicParameters dapperParameters = MapToDapperParameters(query.Parameters);
            if (_transaction is not null)
                return _transaction.Connection.QuerySingleOrDefault<TOut>(query.Sql, dapperParameters, _transaction);
            using DbConnection connection = _createDbConnection();
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
