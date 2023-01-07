using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DotNetDBTools.Deploy.Core;

internal static class HelperExtensions
{
    public static string GetName(this IQuery query)
    {
        string typeName = query.GetType().Name;
        Match match = Regex.Match(typeName, @"^(?:MSSQL|MySQL|PostgreSQL|SQLite)(?<name>.+)$");
        if (match.Success)
            return match.Groups["name"].Value;
        else
            return typeName;
    }

    public static string GetDisplayText(this IQuery query)
    {
        string queryName = query.GetName();
        string querySqlText = query.Sql;
        string queryParametersText = GetParametersDisplayText(query.Parameters);
        string res = $"QueryName: {queryName}\nQuerySql:\n{querySqlText}";
        if (!string.IsNullOrEmpty(queryParametersText))
            res += $"\nQueryParameters:\n{queryParametersText}";
        return res;

        static string GetParametersDisplayText(IEnumerable<QueryParameter> parameters)
        {
            return string.Join("\n", parameters.Select(x => $"{x.Name} {x.Type} = {x.Value}"));
        }
    }

    public static string ParseOutCheckExpression(this string checkDefinition)
    {
        Match match = Regex.Match(checkDefinition, @"^CHECK\s*\((?<expr>.*)\)$");
        if (!match.Success)
            throw new Exception($"Failed to parse out check expression\ninput=[{checkDefinition}]");
        return match.Groups["expr"].Value.Trim();
    }

    public static void ExecuteInTransaction(this IQueryExecutor queryExecutor, Action action)
    {
        queryExecutor.BeginTransaction();
        try
        {
            action();
        }
        catch (Exception)
        {
            queryExecutor.RollbackTransaction();
            throw;
        }
        queryExecutor.CommitTransaction();
    }
}
