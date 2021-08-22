using System.Collections.Generic;
using System.Linq;

namespace DotNetDBTools.DeployInteractor.MSSQL
{
    public class MSSQLGenSqlScriptQueryExecutor : IQueryExecutor
    {
        private readonly List<string> _queries = new();

        public int Execute(IQuery query)
        {
            string paremetersDeclaration = string.Join("\n", query.Parameters.Select(x => $"DECLARE {x.Name} NVARCHAR(MAX) = '{x.Value}';"));
            string queryWithParametersDeclaration = $"{paremetersDeclaration}\n\n{query.Sql}";
            _queries.Add(queryWithParametersDeclaration);
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
    }
}
