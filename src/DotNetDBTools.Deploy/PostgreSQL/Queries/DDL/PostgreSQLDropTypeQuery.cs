using System;
using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DDL
{
    internal class PostgreSQLDropTypeQuery : IQuery
    {
        public string Sql => _sql;
        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();

        private readonly string _sql;

        public PostgreSQLDropTypeQuery(DBObject type)
        {
            _sql = GetSql(type);
        }

        private static string GetSql(DBObject type)
        {
            switch (type)
            {
                case PostgreSQLDomainType:
                    return $@"DROP DOMAIN ""{type.Name}"";";
                case PostgreSQLCompositeType:
                case PostgreSQLEnumType:
                case PostgreSQLRangeType:
                    return $@"DROP TYPE ""{type.Name}"";";
                default:
                    throw new InvalidOperationException($"Invalid user defined type csharp-type: '{type.GetType()}'");
            }
        }
    }
}
