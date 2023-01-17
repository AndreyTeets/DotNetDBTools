using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

    protected string ReplaceParameters(IQuery query)
    {
        if (query.Parameters.Count() > 0)
        {
            // No user sql code is called with parameters, only dndbt system queries do, and since the latter
            // don't contain any comments or any other complicated stuff, this simple regex is good enough.
            if (!IsDNDBTDbObjectRecordQuery(query))
                throw new Exception($"Query type '{query.GetType()}' us not expected to have parameters");

            int replacedCount = 0;
            string res = Regex.Replace(query.Sql, @"(@.+?)([\s|\)|,|;|$])", match =>
            {
                replacedCount++;
                string pName = match.Groups[1].Value;
                string strAfterPName = match.Groups[2].Value;
                QueryParameter queryParameter = query.Parameters.Single(x => x.Name == pName);
                return GetQuotedParameterValue(queryParameter) + strAfterPName;
            });

            if (query.Parameters.Count() != replacedCount)
                throw new Exception($"Failed to replace all parameters for query type '{query.GetType()}'");
            return res;
        }
        else
        {
            return query.Sql;
        }
    }
    protected abstract string GetQuotedParameterValue(QueryParameter queryParameter);

    private static bool IsDNDBTDbObjectRecordQuery(IQuery query)
    {
        string queryNamespace = query.GetType().Namespace;
        bool isDNDBTDbObjectRecordQuery = queryNamespace.EndsWith(nameof(Queries.DNDBTSysInfo), StringComparison.Ordinal);

        if (!isDNDBTDbObjectRecordQuery &&
            !queryNamespace.EndsWith(nameof(Queries.DDL), StringComparison.Ordinal) &&
            !(query.GetType() == typeof(GenericQuery)))
        {
            throw new Exception($"Invalid query type '{query.GetType()}' during script generation");
        }

        return isDNDBTDbObjectRecordQuery;
    }
}
