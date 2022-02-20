using System;
using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.MSSQL.Queries.DNDBTSysInfo;

internal class MSSQLGetDNDBTDbObjectRecordsQuery : GetDNDBTDbObjectRecordsQuery
{
    public override string Sql =>
$@"SELECT
    {DNDBTSysTables.DNDBTDbObjects.ID},
    {DNDBTSysTables.DNDBTDbObjects.ParentID},
    {DNDBTSysTables.DNDBTDbObjects.Type},
    {DNDBTSysTables.DNDBTDbObjects.Name},
    {DNDBTSysTables.DNDBTDbObjects.Code}
FROM {DNDBTSysTables.DNDBTDbObjects};";

    public override RecordsLoader Loader => new MSSQLRecordsLoader();

    public class MSSQLDNDBTDbObjectRecord : DNDBTDbObjectRecord
    {
        public Guid ID { get; set; }
        public Guid? ParentID { get; set; }
        public override Guid GetID() => ID;
        public override Guid? GetParentID() => ParentID;
    }

    public class MSSQLRecordsLoader : RecordsLoader
    {
        public override IEnumerable<DNDBTDbObjectRecord> GetRecords(IQueryExecutor queryExecutor, GetDNDBTDbObjectRecordsQuery query)
        {
            return queryExecutor.Query<MSSQLDNDBTDbObjectRecord>(query);
        }
    }
}
