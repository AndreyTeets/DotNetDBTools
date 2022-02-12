using System;
using System.Collections.Generic;

namespace DotNetDBTools.Deploy.Core;

internal abstract class GenSqlScriptQueryExecutor : IGenSqlScriptQueryExecutor
{
    private int _executeQueriesCount = 0;
    private readonly List<string> _queries = new();

    public int Execute(IQuery query)
    {
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
}
