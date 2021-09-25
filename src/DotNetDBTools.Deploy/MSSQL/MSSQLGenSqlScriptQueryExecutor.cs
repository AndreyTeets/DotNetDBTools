using System;
using System.Collections.Generic;
using System.Data;
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
            string paremeterDeclarations = GetParameterDeclarations(query);
            string queryWithParameterDeclarations = $"{paremeterDeclarations}{query.Sql}";
            _queries.Add($"--QUERY START: {queryName}\n{queryWithParameterDeclarations}\n--QUERY END: {queryName}");
            return 0;
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
            return string.Join("\n\n", _queries);
        }

        private string GetParameterDeclarations(IQuery query)
        {
            return string.Join("", query.Parameters.Select(x => $"DECLARE {x.Name} {GetSqlType(x)} = {Quote(x)};\n"));
        }

        private string GetSqlType(QueryParameter queryParameter)
        {
            return queryParameter.Type switch
            {
                DbType.String => "NVARCHAR(MAX)",
                DbType.Guid => "UNIQUEIDENTIFIER",
                _ => throw new InvalidOperationException($"Invalid query parameter type: '{queryParameter.Type}'")
            };
        }

        private string Quote(QueryParameter queryParameter)
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
}
