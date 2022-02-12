using System;
using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.SQLite.Queries.DNDBTSysInfo;

internal class SQLiteGetAllDbObjectsFromDNDBTSysInfoQuery : GetAllDbObjectsFromDNDBTSysInfoQuery
{
    public override string Sql =>
$@"SELECT
    {DNDBTSysTables.DNDBTDbObjects.ID},
    {DNDBTSysTables.DNDBTDbObjects.ParentID},
    {DNDBTSysTables.DNDBTDbObjects.Type},
    {DNDBTSysTables.DNDBTDbObjects.Name},
    {DNDBTSysTables.DNDBTDbObjects.Code}
FROM {DNDBTSysTables.DNDBTDbObjects};";

    public override RecordsLoader Loader => new SQLiteRecordsLoader();

    public class SQLiteDNDBTDbObjectRecord : DNDBTDbObjectRecord
    {
        public string ID { get; set; }
        public string ParentID { get; set; }
        public override Guid GetID() => new(ID);
        public override Guid? GetParentID() => ParentID is null ? null : new(ParentID);
    }

    public class SQLiteRecordsLoader : RecordsLoader
    {
        public override IEnumerable<DNDBTDbObjectRecord> GetRecords(IQueryExecutor queryExecutor, GetAllDbObjectsFromDNDBTSysInfoQuery query)
        {
            return queryExecutor.Query<SQLiteDNDBTDbObjectRecord>(query);
        }
    }
}
