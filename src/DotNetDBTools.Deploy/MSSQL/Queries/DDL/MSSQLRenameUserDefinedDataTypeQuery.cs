using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.MSSQL.UserDefinedTypes;

namespace DotNetDBTools.Deploy.MSSQL.Queries.DDL;

internal class MSSQLRenameUserDefinedDataTypeQuery : IQuery
{
    public string Sql => _sql;
    public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();

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
