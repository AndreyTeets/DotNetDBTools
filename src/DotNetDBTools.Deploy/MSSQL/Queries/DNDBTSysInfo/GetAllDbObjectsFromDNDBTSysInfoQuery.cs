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
    {DNDBTSysTables.DNDBTDbObjects.Name},
    {DNDBTSysTables.DNDBTDbObjects.ExtraInfo}
FROM {DNDBTSysTables.DNDBTDbObjects};";

        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();

        internal class DNDBTDbObjectRecord
        {
            public Guid ID { get; set; }
            public Guid? ParentID { get; set; }
            public string Type { get; set; }
            public string Name { get; set; }
            public string ExtraInfo { get; set; }
        }

        internal class DNDBTInfo
        {
            public Guid ID { get; set; }
            public string ExtraInfo { get; set; }
        }

        internal static class ResultsInterpreter
        {
            public static void ReplaceDbModelObjectsIDsWithRecordOnes(
                MSSQLDatabase database,
                IEnumerable<DNDBTDbObjectRecord> dbObjectRecords)
            {
                Dictionary<string, DNDBTInfo> dbObjectIDsMap = new();
                foreach (DNDBTDbObjectRecord dbObjRec in dbObjectRecords)
                {
                    DNDBTInfo dndbtInfo = new() { ID = dbObjRec.ID, ExtraInfo = dbObjRec.ExtraInfo };
                    dbObjectIDsMap.Add($"{dbObjRec.Type}_{dbObjRec.Name}_{dbObjRec.ParentID}", dndbtInfo);
                }

                foreach (Table table in database.Tables)
                {
                    table.ID = dbObjectIDsMap[$"{MSSQLDbObjectsTypes.Table}_{table.Name}_{null}"].ID;
                    foreach (Column column in table.Columns)
                    {
                        DNDBTInfo dndbtInfo = dbObjectIDsMap[$"{MSSQLDbObjectsTypes.Column}_{column.Name}_{table.ID}"];
                        column.ID = dndbtInfo.ID;
                        if (column.Default is DefaultValueAsFunction defaultValueAsFunction)
                            defaultValueAsFunction.FunctionText = dndbtInfo.ExtraInfo;
                    }

                    if (table.PrimaryKey is not null)
                        table.PrimaryKey.ID = dbObjectIDsMap[$"{MSSQLDbObjectsTypes.PrimaryKey}_{table.PrimaryKey.Name}_{table.ID}"].ID;
                    foreach (UniqueConstraint uc in table.UniqueConstraints)
                        uc.ID = dbObjectIDsMap[$"{MSSQLDbObjectsTypes.UniqueConstraint}_{uc.Name}_{table.ID}"].ID;
                    foreach (ForeignKey fk in table.ForeignKeys)
                        fk.ID = dbObjectIDsMap[$"{MSSQLDbObjectsTypes.ForeignKey}_{fk.Name}_{table.ID}"].ID;
                }

                foreach (MSSQLUserDefinedType udt in database.UserDefinedTypes)
                    udt.ID = dbObjectIDsMap[$"{MSSQLDbObjectsTypes.UserDefinedType}_{udt.Name}_{null}"].ID;
            }
        }
    }
}
