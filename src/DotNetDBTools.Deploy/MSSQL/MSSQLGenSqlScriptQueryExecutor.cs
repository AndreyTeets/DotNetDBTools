﻿using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Deploy.Core;

namespace DotNetDBTools.Deploy.MSSQL
{
    public class MSSQLGenSqlScriptQueryExecutor : IQueryExecutor
    {
        private readonly List<string> _queries = new();

        public int Execute(IQuery query)
        {
            string queryName = query.GetType().Name;
            string paremetersDeclaration = string.Join("", query.Parameters.Select(x => $"DECLARE {x.Name} NVARCHAR(MAX) = '{x.Value}';\n"));
            string queryWithParametersDeclaration = $"{paremetersDeclaration}{query.Sql}";
            _queries.Add($"--QUERY START: {queryName}\n{queryWithParametersDeclaration}\n--QUERY END: {queryName}");
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
            return string.Join("\n\n", _queries);
        }
    }
}
