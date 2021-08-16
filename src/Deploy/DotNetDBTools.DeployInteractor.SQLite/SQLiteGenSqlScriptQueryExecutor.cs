using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetDBTools.DeployInteractor.SQLite
{
    public class SQLiteGenSqlScriptQueryExecutor : IQueryExecutor
    {
        private readonly List<string> _queries = new();

        public Task<object> Execute(string query, params QueryParameter[] parameters)
        {
            string paremetersDeclaration = string.Join("\n", parameters.Select(x => $"declare {x.Name} nvarchar(max) = '{x.Value}'"));
            string queryWithParametersDeclaration = $"{paremetersDeclaration}\n\n{query}";
            _queries.Add(queryWithParametersDeclaration);
            return null;
        }

        public string GetFinalScript()
        {
            return string.Join("\n\n\n", _queries);
        }
    }
}
