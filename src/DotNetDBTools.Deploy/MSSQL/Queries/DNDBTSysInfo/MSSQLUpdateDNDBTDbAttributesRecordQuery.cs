using System.Collections.Generic;
using System.Data;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.MSSQL.Queries.DNDBTSysInfo;

internal class MSSQLUpdateDNDBTDbAttributesRecordQuery : UpdateDNDBTDbAttributesRecordQuery
{
    public MSSQLUpdateDNDBTDbAttributesRecordQuery(Database database)
        : base(database) { }

    protected override string GetSql()
    {
        string query =
$@"UPDATE [{DNDBTSysTables.DNDBTDbAttributes}] SET
    [{DNDBTSysTables.DNDBTDbAttributes.Version}] = {VersionParameterName};";

        return query;
    }

    protected override List<QueryParameter> GetParameters(Database database)
    {
        return new List<QueryParameter>
        {
            new QueryParameter(VersionParameterName, database.Version, DbType.Int64),
        };
    }
}
