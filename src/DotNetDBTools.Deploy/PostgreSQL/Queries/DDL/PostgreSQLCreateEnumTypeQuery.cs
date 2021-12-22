using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DDL
{
    internal class PostgreSQLCreateEnumTypeQuery : IQuery
    {
        public string Sql => _sql;
        public IEnumerable<QueryParameter> Parameters => _parameters;

        private readonly string _sql;
        private readonly List<QueryParameter> _parameters;

        public PostgreSQLCreateEnumTypeQuery(PostgreSQLEnumType type)
        {
            _sql = GetSql(type);
            _parameters = new List<QueryParameter>();
        }

        private static string GetSql(PostgreSQLEnumType type)
        {
            string query =
$@"CREATE TYPE ""{type.Name}"" AS ENUM
(
{GetAllowedValuesDefinitionsText(type)}
);";

            return query;
        }

        private static string GetAllowedValuesDefinitionsText(PostgreSQLEnumType type)
        {
            List<string> allowedValuesDefinitions = new();

            allowedValuesDefinitions.AddRange(type.AllowedValues.Select(av =>
$@"    '{av}'"));

            return string.Join(",\n", allowedValuesDefinitions);
        }
    }
}
