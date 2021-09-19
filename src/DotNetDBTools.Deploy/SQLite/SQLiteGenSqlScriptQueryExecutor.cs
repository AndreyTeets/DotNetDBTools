using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DotNetDBTools.Deploy.Shared;

namespace DotNetDBTools.Deploy.SQLite
{
    public class SQLiteGenSqlScriptQueryExecutor : IQueryExecutor
    {
        private readonly List<string> _queries = new();

        public int Execute(IQuery query)
        {
            string queryWithParametersReplacedWithValues = ReplaceParameters(query);
            _queries.Add(queryWithParametersReplacedWithValues);
            return 0;
        }

        public IEnumerable<TOut> Query<TOut>(IQuery query)
        {
            throw new System.NotImplementedException();
        }

        public TOut QuerySingleOrDefault<TOut>(IQuery query)
        {
            throw new System.NotImplementedException();
        }

        public string GetFinalScript()
        {
            return string.Join("\n\n\n", _queries);
        }

        private string ReplaceParameters(IQuery query)
        {
            string pattern = @"(@.+?)[\s|,|;|$]";
            string result = Regex.Replace(query.Sql, pattern, match =>
            {
                return Quote(query.Parameters.Single(x => x.Name == match.Groups[1].Value));
            });
            return result;
        }

        private string Quote(QueryParameter queryParameter)
        {
            // TODO quote depending on type
            return $"'{queryParameter.Value}'";
        }
    }
}
