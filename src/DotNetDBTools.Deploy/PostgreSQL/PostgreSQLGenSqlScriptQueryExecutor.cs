using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using DotNetDBTools.Deploy.Core;

namespace DotNetDBTools.Deploy.PostgreSQL;

internal class PostgreSQLGenSqlScriptQueryExecutor : GenSqlScriptQueryExecutor
{
    protected override string CreateQueryText(IQuery query)
    {
        string queryWithParametersReplacedWithValues = ReplaceParameters(query);
        string execQueryStatement = $"EXECUTE '{queryWithParametersReplacedWithValues.Replace("'", "''")}';";
        return execQueryStatement;
    }

    protected override string CreateBeginTransactionText()
    {
        return
@"DO $DNDBTGeneratedScriptTransactionBlock$
BEGIN";
    }

    protected override string CreateCommitTransactionText()
    {
        return
@"END;
$DNDBTGeneratedScriptTransactionBlock$";
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
            DbType.String or DbType.Guid => $"'{queryParameter.Value}'",
            _ => throw new InvalidOperationException($"Invalid query parameter type: '{queryParameter.Type}'")
        };
    }
}
