using System;
using System.Collections.Generic;
using DotNetDBTools.Deploy.Core.Queries;
using DotNetDBTools.Generation.Core;

namespace DotNetDBTools.Deploy.Core;

internal abstract class GenSqlScriptQueryExecutor : IGenSqlScriptQueryExecutor
{
    public bool NoDNDBTInfo { get; set; } = false;

    private int _executeQueriesCount = 0;
    private readonly List<string> _queries = new();

    public int Execute(IQuery query)
    {
        if (NoDNDBTInfo && IsDNDBTDbObjectRecordQuery(query))
            return 0;
        string queryName = query.GetName();
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
        throw new NotImplementedException("Method should never be called on this class.");
    }

    public TOut QuerySingleOrDefault<TOut>(IQuery query)
    {
        throw new NotImplementedException("Method should never be called on this class.");
    }

    public string GetFinalScript()
    {
        if (_executeQueriesCount == 0)
            return "";
        return string.Join("\n\n", _queries).NormalizeLineEndings();
    }

    private static bool IsDNDBTDbObjectRecordQuery(IQuery query)
    {
        string queryNamespace = query.GetType().Namespace;
        bool isDNDBTDbObjectRecordQuery = queryNamespace.EndsWith("DNDBTSysInfo", StringComparison.Ordinal);

        if (!isDNDBTDbObjectRecordQuery &&
            !queryNamespace.EndsWith("DDL", StringComparison.Ordinal) &&
            !(query.GetType() == typeof(GenericQuery)))
        {
            throw new Exception($"Invalid query type '{query.GetType()}' during script generation");
        }

        return isDNDBTDbObjectRecordQuery;
    }
}
