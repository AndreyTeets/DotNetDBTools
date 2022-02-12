using System;
using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.MySQL.Queries.DNDBTSysInfo;

internal class MySQLGetAllDbObjectsFromDNDBTSysInfoQuery : GetAllDbObjectsFromDNDBTSysInfoQuery
{
    public override string Sql =>
$@"SELECT
    CAST(`{DNDBTSysTables.DNDBTDbObjects.ID}` AS CHAR(36)) AS {nameof(MySQLDNDBTDbObjectRecord.ID)},
    CAST(`{DNDBTSysTables.DNDBTDbObjects.ParentID}` AS CHAR(36)) AS {nameof(MySQLDNDBTDbObjectRecord.ParentID)},
    `{DNDBTSysTables.DNDBTDbObjects.Type}`,
    `{DNDBTSysTables.DNDBTDbObjects.Name}`,
    `{DNDBTSysTables.DNDBTDbObjects.Code}`
FROM `{DNDBTSysTables.DNDBTDbObjects}`;";

    public override RecordsLoader Loader => new MySQLRecordsLoader();

    public class MySQLDNDBTDbObjectRecord : DNDBTDbObjectRecord
    {
        public string ID { get; set; }
        public string ParentID { get; set; }
        public override Guid GetID() => new(ID);
        public override Guid? GetParentID() => ParentID is null ? null : new(ParentID);
    }

    public class MySQLRecordsLoader : RecordsLoader
    {
        public override IEnumerable<DNDBTDbObjectRecord> GetRecords(IQueryExecutor queryExecutor, GetAllDbObjectsFromDNDBTSysInfoQuery query)
        {
            return queryExecutor.Query<MySQLDNDBTDbObjectRecord>(query);
        }
    }
}
