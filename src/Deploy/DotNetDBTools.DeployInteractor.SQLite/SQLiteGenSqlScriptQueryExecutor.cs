using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DotNetDBTools.DeployInteractor.SQLite
{
    public class SQLiteGenSqlScriptQueryExecutor : IQueryExecutor
    {
        private readonly List<string> _queries = new();

        public int Execute(string query, params QueryParameter[] parameters)
        {
            string queryWithParametersReplacedWithValues = ReplaceParameters(query, parameters);
            _queries.Add(queryWithParametersReplacedWithValues);
            return 0;
        }

        public IEnumerable<TOut> Query<TOut>(string query, params QueryParameter[] parameters)
        {
            throw new System.NotImplementedException();
        }

        public TOut QuerySingleOrDefault<TOut>(string query, params QueryParameter[] parameters)
        {
            throw new System.NotImplementedException();
        }

        public string GetFinalScript()
        {
            return string.Join("\n\n\n", _queries);
        }

        private string ReplaceParameters(string query, params QueryParameter[] parameters)
        {
            string pattern = @"(@.+?)[\s|,|;|$]";
            string result = Regex.Replace(query, pattern, match =>
            {
                return Quote(parameters.Single(x => x.Name == match.Groups[1].Value));
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
