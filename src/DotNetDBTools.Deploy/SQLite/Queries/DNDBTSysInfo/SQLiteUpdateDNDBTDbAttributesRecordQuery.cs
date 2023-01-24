using System.Collections.Generic;
using System.Data;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.SQLite.Queries.DNDBTSysInfo;

internal class SQLiteUpdateDNDBTDbAttributesRecordQuery : UpdateDNDBTDbAttributesRecordQuery
{
    public SQLiteUpdateDNDBTDbAttributesRecordQuery(long databaseVersion)
        : base(databaseVersion) { }

    protected override string GetSql()
    {
        string query =
$@"UPDATE [{DNDBTSysTables.DNDBTDbAttributes}] SET
    [{DNDBTSysTables.DNDBTDbAttributes.Version}] = {VersionParameterName};";

        return query;
    }

    protected override List<QueryParameter> GetParameters(long databaseVersion)
    {
        return new List<QueryParameter>
        {
            new QueryParameter(VersionParameterName, databaseVersion, DbType.Int64),
        };
    }
}
