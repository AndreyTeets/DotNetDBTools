using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Deploy.SQLite.Queries.DNDBTSysInfo
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
            public string ID { get; set; }
            public string ParentID { get; set; }
            public string Type { get; set; }
            public string Name { get; set; }
        }

        internal static class ResultsInterpreter
        {
            public static void ReplaceDbModelObjectsIDsWithRecordOnes(
                SQLiteDatabaseInfo databaseInfo,
                IEnumerable<DNDBTDbObjectRecord> dbObjectRecords)
            {
                foreach (TableInfo table in databaseInfo.Tables)
                {
                    table.ID = new Guid(dbObjectRecords.Single(x =>
                        x.Name == table.Name &&
                        x.Type == $"{SQLiteDbObjectsTypes.Table}").ID);

                    foreach (ColumnInfo column in table.Columns)
                    {
                        column.ID = new Guid(dbObjectRecords.Single(x =>
                            x.ParentID == table.ID.ToString() &&
                            x.Name == column.Name &&
                            x.Type == $"{SQLiteDbObjectsTypes.Column}").ID);
                    }

                    if (table.PrimaryKey is not null)
                    {
                        table.PrimaryKey.ID = new Guid(dbObjectRecords.Single(x =>
                            x.ParentID == table.ID.ToString() &&
                            x.Name == table.PrimaryKey.Name &&
                            x.Type == $"{SQLiteDbObjectsTypes.PrimaryKey}").ID);
                    }

                    foreach (UniqueConstraintInfo uniqueConstraint in table.UniqueConstraints)
                    {
                        uniqueConstraint.ID = new Guid(dbObjectRecords.Single(x =>
                            x.ParentID == table.ID.ToString() &&
                            x.Name == uniqueConstraint.Name &&
                            x.Type == $"{SQLiteDbObjectsTypes.UniqueConstraint}").ID);
                    }

                    foreach (ForeignKeyInfo foreignKey in table.ForeignKeys)
                    {
                        foreignKey.ID = new Guid(dbObjectRecords.Single(x =>
                            x.ParentID == table.ID.ToString() &&
                            x.Name == foreignKey.Name &&
                            x.Type == $"{SQLiteDbObjectsTypes.ForeignKey}").ID);
                    }
                }
            }
        }
    }
}
