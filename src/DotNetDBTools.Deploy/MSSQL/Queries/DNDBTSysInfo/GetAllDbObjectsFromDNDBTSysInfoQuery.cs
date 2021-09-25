using System;
using System.Collections.Generic;
using System.Linq;
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
            public Guid ParentID { get; set; }
            public string Type { get; set; }
            public string Name { get; set; }
        }

        internal static class ResultsInterpreter
        {
            public static void ReplaceDbModelObjectsIDsWithRecordOnes(
                MSSQLDatabaseInfo databaseInfo,
                IEnumerable<DNDBTDbObjectRecord> dbObjectRecords)
            {
                foreach (TableInfo table in databaseInfo.Tables)
                {
                    table.ID = dbObjectRecords.Single(x =>
                        x.Name == table.Name &&
                        x.Type == $"{MSSQLDbObjectsTypes.Table}").ID;

                    foreach (ColumnInfo column in table.Columns)
                    {
                        column.ID = dbObjectRecords.Single(x =>
                            x.ParentID == table.ID &&
                            x.Name == column.Name &&
                            x.Type == $"{MSSQLDbObjectsTypes.Column}").ID;
                    }

                    if (table.PrimaryKey is not null)
                    {
                        table.PrimaryKey.ID = dbObjectRecords.Single(x =>
                            x.ParentID == table.ID &&
                            x.Name == table.PrimaryKey.Name &&
                            x.Type == $"{MSSQLDbObjectsTypes.PrimaryKey}").ID;
                    }

                    foreach (UniqueConstraintInfo uniqueConstraint in table.UniqueConstraints)
                    {
                        uniqueConstraint.ID = dbObjectRecords.Single(x =>
                            x.ParentID == table.ID &&
                            x.Name == uniqueConstraint.Name &&
                            x.Type == $"{MSSQLDbObjectsTypes.UniqueConstraint}").ID;
                    }

                    foreach (ForeignKeyInfo foreignKey in table.ForeignKeys)
                    {
                        foreignKey.ID = dbObjectRecords.Single(x =>
                            x.ParentID == table.ID &&
                            x.Name == foreignKey.Name &&
                            x.Type == $"{MSSQLDbObjectsTypes.ForeignKey}").ID;
                    }
                }

                foreach (MSSQLUserDefinedTypeInfo userDefinedType in databaseInfo.UserDefinedTypes)
                {
                    userDefinedType.ID = dbObjectRecords.Single(x =>
                        x.Name == userDefinedType.Name &&
                        x.Type == $"{MSSQLDbObjectsTypes.UserDefinedType}").ID;
                }
            }
        }
    }
}
