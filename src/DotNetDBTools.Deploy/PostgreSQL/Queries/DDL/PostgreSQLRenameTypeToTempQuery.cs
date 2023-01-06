using System;
using DotNetDBTools.Deploy.Core.Queries;
using DotNetDBTools.Generation.PostgreSQL;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;
using QH = DotNetDBTools.Deploy.PostgreSQL.PostgreSQLQueriesHelper;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DDL;

internal class PostgreSQLRenameTypeToTempQuery : NoParametersQuery
{
    public override string Sql => _sql;
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
                string multirangeTypeName = rt.MultirangeTypeName ?? $"{rt.Name}_multirange";
                return
$@"ALTER TYPE ""{rt.Name}"" RENAME TO ""{tempPrefix}{type.Name}"";
ALTER FUNCTION ""{rt.Name}""({rt.Subtype.GetQuotedName()},{rt.Subtype.GetQuotedName()}) RENAME TO ""{tempPrefix}{type.Name}"";
ALTER FUNCTION ""{rt.Name}"" RENAME TO ""{tempPrefix}{type.Name}"";
IF ({QH.SelectDbmsVersionStatement}) >= {QH.MultirangeTypeNameAvailableDbmsVersion} THEN
ALTER TYPE ""{multirangeTypeName}"" RENAME TO ""{tempPrefix}{multirangeTypeName}"";
ALTER FUNCTION ""{multirangeTypeName}""() RENAME TO ""{tempPrefix}{multirangeTypeName}"";
ALTER FUNCTION ""{multirangeTypeName}""(""{tempPrefix}{type.Name}"") RENAME TO ""{tempPrefix}{multirangeTypeName}"";
ALTER FUNCTION ""{multirangeTypeName}"" RENAME TO ""{tempPrefix}{multirangeTypeName}"";
END IF;".InsideDoPlPgSqlBlock();
            default:
                throw new InvalidOperationException($"Invalid user defined type csharp-type: '{type.GetType()}'");
        }
    }
}
