using System;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.MSSQL.Queries.DNDBTSysInfo;

internal class MSSQLDeleteDNDBTDbObjectRecordQuery : DeleteDNDBTDbObjectRecordQuery
{
    public MSSQLDeleteDNDBTDbObjectRecordQuery(Guid objectID)
        : base(objectID) { }

    protected override string GetSql(Guid objectID)
    {
        string query =
$@"DELETE FROM [{DNDBTSysTables.DNDBTDbObjects}]
WHERE [{DNDBTSysTables.DNDBTDbObjects.ID}] = '{objectID}';";

        return query;
    }
}
