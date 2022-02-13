﻿using System;
using System.Collections.Generic;
using DotNetDBTools.Deploy.Core.Queries;

namespace DotNetDBTools.Deploy.Core;

internal abstract class GenSqlScriptQueryExecutor : IGenSqlScriptQueryExecutor
{
    public bool DDLOnly { get; set; } = false;

    private int _executeQueriesCount = 0;
    private readonly List<string> _queries = new();

    public int Execute(IQuery query)
    {
        if (DDLOnly && !IsDDLQuery(query))
            return 0;
        string queryName = query.GetType().Name;
        string queryText = CreateQueryText(query);
        _queries.Add($"-- QUERY START: {queryName}\n{queryText}\n-- QUERY END: {queryName}");
        _executeQueriesCount++;
        return 0;
    }
    protected abstract string CreateQueryText(IQuery query);

    public void BeginTransaction()
    {
        _queries.Add(CreateBeginTransactionText());
    }
    protected abstract string CreateBeginTransactionText();

    public void CommitTransaction()
    {
        _queries.Add(CreateCommitTransactionText());
    }
    protected abstract string CreateCommitTransactionText();

    public void RollbackTransaction()
    {
    }

    public IEnumerable<TOut> Query<TOut>(IQuery query)
    {
        throw new NotImplementedException();
    }

    public TOut QuerySingleOrDefault<TOut>(IQuery query)
    {
        throw new NotImplementedException();
    }

    public string GetFinalScript()
    {
        if (_executeQueriesCount == 0)
            return "";
        return string.Join("\n\n", _queries);
    }

    private static bool IsDDLQuery(IQuery query)
    {
        string queryNamespace = query.GetType().Namespace;
        bool isDDLQuery = queryNamespace.EndsWith("DDL", StringComparison.Ordinal) ||
            query.GetType() == typeof(GenericQuery);

        if (!isDDLQuery &&
            !queryNamespace.EndsWith("DNDBTSysInfo", StringComparison.Ordinal))
        {
            throw new Exception($"Invalid query type '{query.GetType()}' during script generation");
        }

        return isDDLQuery;
    }
}
