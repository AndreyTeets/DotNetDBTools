using System;
using DotNetDBTools.Deploy.Core.Queries;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DDL;

internal class PostgreSQLRenameProgrammableObjectToTempQuery : NoParametersQuery
{
    public override string Sql => _sql;
    private readonly string _sql;

    public PostgreSQLRenameProgrammableObjectToTempQuery(DbObject dbObject)
    {
        _sql = GetSql(dbObject);
    }

    private static string GetSql(DbObject dbObject)
    {
        string tempPrefix = "_DNDBTTemp_";
        switch (dbObject)
        {
            case PostgreSQLView:
                return $@"ALTER VIEW ""{dbObject.Name}"" RENAME TO ""{tempPrefix}{dbObject.Name}"";";
            case PostgreSQLFunction:
                // TODO ALTER FUNCTION ""{func.Name}""({string.Join(",", func.ArgsTypes)}) RENAME TO
                return $@"ALTER FUNCTION ""{dbObject.Name}"" RENAME TO ""{tempPrefix}{dbObject.Name}"";";
            case PostgreSQLProcedure:
                return $@"ALTER PROCEDURE ""{dbObject.Name}"" RENAME TO ""{tempPrefix}{dbObject.Name}"";";
            default:
                throw new InvalidOperationException($"Invalid programmable object type: '{dbObject.GetType()}'");
        }
    }
}
