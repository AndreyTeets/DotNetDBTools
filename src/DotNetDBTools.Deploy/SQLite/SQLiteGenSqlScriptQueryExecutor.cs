using System;
using System.Data;
using DotNetDBTools.Deploy.Core;

namespace DotNetDBTools.Deploy.SQLite;

internal class SQLiteGenSqlScriptQueryExecutor : GenSqlScriptQueryExecutor
{
    protected override string CreateQueryText(IQuery query)
    {
        string queryWithParametersReplacedWithValues = ReplaceParameters(query);
        return queryWithParametersReplacedWithValues;
    }

    protected override string CreateBeginTransactionText()
    {
        return
@"PRAGMA foreign_keys=off;
BEGIN TRANSACTION;";
    }

    protected override string CreateCommitTransactionText()
    {
        return
@"COMMIT TRANSACTION;";
    }

    protected override string GetQuotedParameterValue(QueryParameter queryParameter)
    {
        if (queryParameter.Value is null)
            return "NULL";
        return queryParameter.Type switch
        {
            DbType.String => $"'{queryParameter.Value.ToString().Replace("'", "''")}'",
            DbType.Int64 => $"{queryParameter.Value}",
            _ => throw new InvalidOperationException($"Invalid query parameter type: '{queryParameter.Type}'")
        };
    }
}
