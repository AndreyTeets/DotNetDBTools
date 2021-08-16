using System.Collections.Generic;
using System.Linq;

namespace DotNetDBTools.DeployInteractor.MSSQL
{
    public class MSSQLGenSqlScriptQueryExecutor : IQueryExecutor
    {
        private readonly List<string> _queries = new();

        public int Execute(string query, params QueryParameter[] parameters)
        {
            string paremetersDeclaration = string.Join("\n", parameters.Select(x => $"declare {x.Name} nvarchar(max) = '{x.Value}'"));
            string queryWithParametersDeclaration = $"{paremetersDeclaration}\n\n{query}";
            _queries.Add(queryWithParametersDeclaration);
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
    }
}
