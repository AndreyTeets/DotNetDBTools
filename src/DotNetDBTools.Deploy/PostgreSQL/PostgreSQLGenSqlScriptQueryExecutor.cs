using System;
using System.Data;
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

    protected override string GetQuotedParameterValue(QueryParameter queryParameter)
    {
        if (queryParameter.Value is null)
            return "NULL";
        return queryParameter.Type switch
        {
            DbType.String or DbType.Guid => $"'{queryParameter.Value.ToString().Replace("'", "''")}'",
            DbType.Int64 => $"{queryParameter.Value}",
            _ => throw new InvalidOperationException($"Invalid query parameter type: '{queryParameter.Type}'")
        };
    }
}
