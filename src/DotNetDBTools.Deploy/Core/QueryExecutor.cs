using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Dapper;

namespace DotNetDBTools.Deploy.Core;

internal abstract class QueryExecutor : IQueryExecutor
{
    private readonly Events _events;
    private readonly DbConnection _connection;
    private DbTransaction _transaction;

    protected QueryExecutor(DbConnection connection, Events events)
    {
        _events = events;
        _connection = connection;
    }

    public void BeginTransaction()
    {
        _events.InvokeEventFired(EventType.BeginTransactionBegan);
        OpenConnectionIfNotOpened();
        BeforeBeginTransaction(_connection);
        _transaction = _connection.BeginTransaction();
        _events.InvokeEventFired(EventType.BeginTransactionFinished);
    }
    protected virtual void BeforeBeginTransaction(DbConnection connection) { }

    public void CommitTransaction()
    {
        _events.InvokeEventFired(EventType.CommitTransactionBegan);
        _transaction.Commit();
        _transaction.Dispose();
        _transaction = null;
        _events.InvokeEventFired(EventType.CommitTransactionFinished);
    }

    public void RollbackTransaction()
    {
        _events.InvokeEventFired(EventType.RollbackTransactionBegan);
        try
        {
            _transaction.Rollback();
        }
        catch (InvalidOperationException)
        {
        }
        _transaction.Dispose();
        _transaction = null;
        _events.InvokeEventFired(EventType.RollbackTransactionFinished);
    }

    public int Execute(IQuery query)
    {
        _events.InvokeEventFired(EventType.QueryBegan, query.GetDisplayText());
        DynamicParameters dapperParameters = MapToDapperParameters(query.Parameters);
        if (_transaction is not null)
            return _transaction.Connection.Execute(query.Sql, dapperParameters, _transaction);

        OpenConnectionIfNotOpened();
        int res = _connection.Execute(query.Sql, dapperParameters);
        _events.InvokeEventFired(EventType.QueryFinished, query.GetName());
        return res;
    }

    public IEnumerable<TOut> Query<TOut>(IQuery query)
    {
        _events.InvokeEventFired(EventType.QueryBegan, query.GetDisplayText());
        DynamicParameters dapperParameters = MapToDapperParameters(query.Parameters);
        if (_transaction is not null)
            return _transaction.Connection.Query<TOut>(query.Sql, dapperParameters, _transaction);

        OpenConnectionIfNotOpened();
        IEnumerable<TOut> res = _connection.Query<TOut>(query.Sql, dapperParameters);
        _events.InvokeEventFired(EventType.QueryFinished, query.GetName());
        return res;
    }

    public TOut QuerySingleOrDefault<TOut>(IQuery query)
    {
        _events.InvokeEventFired(EventType.QueryBegan, query.GetDisplayText());
        DynamicParameters dapperParameters = MapToDapperParameters(query.Parameters);
        if (_transaction is not null)
            return _transaction.Connection.QuerySingleOrDefault<TOut>(query.Sql, dapperParameters, _transaction);

        OpenConnectionIfNotOpened();
        TOut res = _connection.QuerySingleOrDefault<TOut>(query.Sql, dapperParameters);
        _events.InvokeEventFired(EventType.QueryFinished, query.GetName());
        return res;
    }

    private void OpenConnectionIfNotOpened()
    {
        if (_connection.State != ConnectionState.Open)
        {
            _events.InvokeEventFired(EventType.OpenDbConnectionBegan);
            _connection.Open();
            _events.InvokeEventFired(EventType.OpenDbConnectionFinished);
        }
    }

    private static DynamicParameters MapToDapperParameters(IEnumerable<QueryParameter> parameters)
    {
        DynamicParameters dapperParameters = new();
        foreach (QueryParameter parameter in parameters)
            dapperParameters.Add(parameter.Name, parameter.Value, parameter.Type);
        return dapperParameters;
    }
}
