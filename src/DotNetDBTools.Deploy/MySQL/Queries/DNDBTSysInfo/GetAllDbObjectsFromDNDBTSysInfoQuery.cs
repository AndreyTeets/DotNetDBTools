using System;
using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MySQL;

namespace DotNetDBTools.Deploy.MySQL.Queries.DNDBTSysInfo
{
    internal class GetAllDbObjectsFromDNDBTSysInfoQuery : IQuery
    {
        public string Sql =>
$@"SELECT
    {DNDBTSysTables.DNDBTDbObjects.ID},
    {DNDBTSysTables.DNDBTDbObjects.ParentID},
    {DNDBTSysTables.DNDBTDbObjects.Type},
    {DNDBTSysTables.DNDBTDbObjects.Name}
FROM {DNDBTSysTables.DNDBTDbObjects};";

        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();

        internal class DNDBTDbObjectRecord
        {
            public Guid ID { get; set; }
            public Guid? ParentID { get; set; }
            public string Type { get; set; }
            public string Name { get; set; }
        }

        internal static class ResultsInterpreter
        {
            public static void ReplaceDbModelObjectsIDsWithRecordOnes(
                MySQLDatabase database,
                IEnumerable<DNDBTDbObjectRecord> dbObjectRecords)
            {
                Dictionary<string, Guid> dbObjectIDsMap = new();
                foreach (DNDBTDbObjectRecord dbObjRec in dbObjectRecords)
                    dbObjectIDsMap.Add($"{dbObjRec.Type}_{dbObjRec.Name}_{dbObjRec.ParentID}", dbObjRec.ID);

                foreach (Table table in database.Tables)
                {
                    table.ID = dbObjectIDsMap[$"{MySQLDbObjectsTypes.Table}_{table.Name}_{null}"];
                    foreach (Column column in table.Columns)
                        column.ID = dbObjectIDsMap[$"{MySQLDbObjectsTypes.Column}_{column.Name}_{table.ID}"];
                    if (table.PrimaryKey is not null)
                        table.PrimaryKey.ID = dbObjectIDsMap[$"{MySQLDbObjectsTypes.PrimaryKey}_{table.PrimaryKey.Name}_{table.ID}"];
                    foreach (UniqueConstraint uc in table.UniqueConstraints)
                        uc.ID = dbObjectIDsMap[$"{MySQLDbObjectsTypes.UniqueConstraint}_{uc.Name}_{table.ID}"];
                    foreach (ForeignKey fk in table.ForeignKeys)
                        fk.ID = dbObjectIDsMap[$"{MySQLDbObjectsTypes.ForeignKey}_{fk.Name}_{table.ID}"];
                }
            }
        }
    }
}
