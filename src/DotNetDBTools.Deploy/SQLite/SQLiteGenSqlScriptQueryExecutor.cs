using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
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

    private static string ReplaceParameters(IQuery query)
    {
        string pattern = @"(@.+?)([\s|,|;|$])";
        string result = Regex.Replace(query.Sql, pattern, match =>
        {
            return Quote(query.Parameters.Single(x => x.Name == match.Groups[1].Value)) + match.Groups[2].Value;
        });
        return result;
    }

    private static string Quote(QueryParameter queryParameter)
    {
        if (queryParameter.Value is null)
            return "NULL";
        return queryParameter.Type switch
        {
            DbType.String => $"'{queryParameter.Value}'",
            _ => throw new InvalidOperationException($"Invalid query parameter type: '{queryParameter.Type}'")
        };
    }
}
