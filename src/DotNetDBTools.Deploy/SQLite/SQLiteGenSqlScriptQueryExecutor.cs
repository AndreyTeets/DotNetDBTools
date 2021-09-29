using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using DotNetDBTools.Deploy.Core;

namespace DotNetDBTools.Deploy.SQLite
{
    internal class SQLiteGenSqlScriptQueryExecutor : IGenSqlScriptQueryExecutor
    {
        private int _executeQueriesCount = 0;
        private readonly List<string> _queries = new();

        public int Execute(IQuery query)
        {
            string queryName = query.GetType().Name;
            string queryWithParametersReplacedWithValues = ReplaceParameters(query);
            _queries.Add($"--QUERY START: {queryName}\n{queryWithParametersReplacedWithValues}\n--QUERY END: {queryName}");
            _executeQueriesCount++;
            return 0;
        }

        public void BeginTransaction()
        {
            _queries.Add(
@"PRAGMA foreign_keys=off;
BEGIN TRANSACTION;");
        }

        public void CommitTransaction()
        {
            _queries.Add(
@"COMMIT TRANSACTION;");
        }

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

        private string ReplaceParameters(IQuery query)
        {
            string pattern = @"(@.+?)([\s|,|;|$])";
            string result = Regex.Replace(query.Sql, pattern, match =>
            {
                return Quote(query.Parameters.Single(x => x.Name == match.Groups[1].Value)) + match.Groups[2].Value;
            });
            return result;
        }

        private string Quote(QueryParameter queryParameter)
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
}
