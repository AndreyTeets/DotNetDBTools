using DotNetDBTools.Deploy.Core.Queries;
using DotNetDBTools.Models.MSSQL.UserDefinedTypes;

namespace DotNetDBTools.Deploy.MSSQL.Queries.DDL;

internal class MSSQLRenameUserDefinedDataTypeQuery : NoParametersQuery
{
    public override string Sql => _sql;
    private readonly string _sql;

    public MSSQLRenameUserDefinedDataTypeQuery(MSSQLUserDefinedType userDefinedType)
    {
        _sql = GetSql(userDefinedType);
    }

    private static string GetSql(MSSQLUserDefinedType userDefinedType)
    {
        string query =
$@"EXEC sp_rename '{userDefinedType.Name}', '_DNDBTTemp_{userDefinedType.Name}', 'USERDATATYPE';";

        return query;
    }
}
