using System;
using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DDL;

internal class PostgreSQLRenameTypeToTempQuery : IQuery
{
    public string Sql => _sql;
    public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();

    private readonly string _sql;

    public PostgreSQLRenameTypeToTempQuery(DbObject type)
    {
        _sql = GetSql(type);
    }

    private static string GetSql(DbObject type)
    {
        string tempPrefix = "_DNDBTTemp_";
        switch (type)
        {
            case PostgreSQLDomainType:
                return $@"ALTER DOMAIN ""{type.Name}"" RENAME TO ""{tempPrefix}{type.Name}"";";
            case PostgreSQLCompositeType:
            case PostgreSQLEnumType:
                return $@"ALTER TYPE ""{type.Name}"" RENAME TO ""{tempPrefix}{type.Name}"";";
            case PostgreSQLRangeType rt:
                return
$@"ALTER TYPE ""{rt.Name}"" RENAME TO ""{tempPrefix}{type.Name}"";
ALTER FUNCTION ""{rt.Name}""({rt.Subtype.GetQuotedName()},{rt.Subtype.GetQuotedName()}) RENAME TO ""{tempPrefix}{type.Name}"";
ALTER FUNCTION ""{rt.Name}"" RENAME TO ""{tempPrefix}{type.Name}"";
ALTER TYPE ""{rt.MultirangeTypeName}"" RENAME TO ""{tempPrefix}{rt.MultirangeTypeName}"";
ALTER FUNCTION ""{rt.MultirangeTypeName}""() RENAME TO ""{tempPrefix}{rt.MultirangeTypeName}"";
ALTER FUNCTION ""{rt.MultirangeTypeName}""(""{tempPrefix}{type.Name}"") RENAME TO ""{tempPrefix}{rt.MultirangeTypeName}"";
ALTER FUNCTION ""{rt.MultirangeTypeName}"" RENAME TO ""{tempPrefix}{rt.MultirangeTypeName}"";";
            default:
                throw new InvalidOperationException($"Invalid user defined type csharp-type: '{type.GetType()}'");
        }
    }
}
