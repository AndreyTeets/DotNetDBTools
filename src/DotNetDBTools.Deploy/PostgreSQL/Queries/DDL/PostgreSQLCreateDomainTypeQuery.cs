﻿using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;
using static DotNetDBTools.Deploy.PostgreSQL.PostgreSQLQueriesHelper;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DDL
{
    internal class PostgreSQLCreateDomainTypeQuery : IQuery
    {
        public string Sql => _sql;
        public IEnumerable<QueryParameter> Parameters => _parameters;

        private readonly string _sql;
        private readonly List<QueryParameter> _parameters;

        public PostgreSQLCreateDomainTypeQuery(PostgreSQLDomainType type)
        {
            _sql = GetSql(type);
            _parameters = new List<QueryParameter>();
        }

        private static string GetSql(PostgreSQLDomainType type)
        {
            string query =
$@"CREATE DOMAIN ""{type.Name}"" AS {type.UnderlyingType.GetQuotedName()}{GetDomainDefinitionsText(type)};";

            return query;
        }

        private static string GetDomainDefinitionsText(PostgreSQLDomainType type)
        {
            List<string> definitions = new();

            if (type.Default is not null)
            {
                definitions.Add(
$@"    DEFAULT {QuoteDefaultValue(type.Default)}");
            }

            definitions.Add(
$@"    {GetNullabilityStatement(type)}");

            foreach (CheckConstraint ck in type.CheckConstraints)
            {
                definitions.Add(
$@"    CONSTRAINT ""{ck.Name}"" {ck.GetCode()}");
            }

            return string.Join("\n", definitions);
        }

        private static string GetNullabilityStatement(PostgreSQLDomainType type) =>
            type.Nullable switch
            {
                true => "NULL",
                false => "NOT NULL",
            };
    }
}
