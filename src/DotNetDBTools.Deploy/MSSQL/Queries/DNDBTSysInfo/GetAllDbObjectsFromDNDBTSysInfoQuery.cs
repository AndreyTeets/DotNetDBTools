using System;
using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Deploy.MSSQL.Queries.DNDBTSysInfo
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
                MSSQLDatabaseInfo databaseInfo,
                IEnumerable<DNDBTDbObjectRecord> dbObjectRecords)
            {
                Dictionary<string, Guid> dbObjectIDsMap = new();
                foreach (DNDBTDbObjectRecord dbObjRec in dbObjectRecords)
                    dbObjectIDsMap.Add($"{dbObjRec.Type}_{dbObjRec.Name}_{dbObjRec.ParentID}", dbObjRec.ID);

                foreach (TableInfo table in databaseInfo.Tables)
                {
                    table.ID = dbObjectIDsMap[$"{MSSQLDbObjectsTypes.Table}_{table.Name}_{null}"];
                    foreach (ColumnInfo column in table.Columns)
                        column.ID = dbObjectIDsMap[$"{MSSQLDbObjectsTypes.Column}_{column.Name}_{table.ID}"];
                    if (table.PrimaryKey is not null)
                        table.PrimaryKey.ID = dbObjectIDsMap[$"{MSSQLDbObjectsTypes.PrimaryKey}_{table.PrimaryKey.Name}_{table.ID}"];
                    foreach (UniqueConstraintInfo uc in table.UniqueConstraints)
                        uc.ID = dbObjectIDsMap[$"{MSSQLDbObjectsTypes.UniqueConstraint}_{uc.Name}_{table.ID}"];
                    foreach (ForeignKeyInfo fk in table.ForeignKeys)
                        fk.ID = dbObjectIDsMap[$"{MSSQLDbObjectsTypes.ForeignKey}_{fk.Name}_{table.ID}"];
                }

                foreach (MSSQLUserDefinedTypeInfo udt in databaseInfo.UserDefinedTypes)
                    udt.ID = dbObjectIDsMap[$"{MSSQLDbObjectsTypes.UserDefinedType}_{udt.Name}_{null}"];
            }
        }
    }
}
