using System;
using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.ModelBuilders;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.MSSQL.Queries.DBMSSysInfo
{
    internal class GetForeignKeysFromDBMSSysInfoQuery : IQuery
    {
        public string Sql =>
$@"SELECT
    thisTable.name AS {nameof(ForeignKeyRecord.ThisTableName)},
    foreignKey.name AS {nameof(ForeignKeyRecord.ForeignKeyName)},
    thisColumns.name AS {nameof(ForeignKeyRecord.ThisColumnName)},
    fkColumnsMap.constraint_column_id AS {nameof(ForeignKeyRecord.ThisColumnPosition)},
    referencedTable.name AS {nameof(ForeignKeyRecord.ReferencedTableName)},
    referencedColumns.name AS {nameof(ForeignKeyRecord.ReferencedColumnName)},
    fkColumnsMap.constraint_column_id AS  {nameof(ForeignKeyRecord.ReferencedColumnPosition)},
    foreignKey.update_referential_action_desc AS {nameof(ForeignKeyRecord.OnUpdate)},
    foreignKey.delete_referential_action_desc AS {nameof(ForeignKeyRecord.OnDelete)}
FROM sys.tables thisTable
INNER JOIN sys.foreign_keys foreignKey
    ON foreignKey.parent_object_id = thisTable.object_id
INNER JOIN sys.tables referencedTable
    ON referencedTable.object_id = foreignKey.referenced_object_id
INNER JOIN sys.foreign_key_columns fkColumnsMap
    ON fkColumnsMap.constraint_object_id = foreignKey.object_id
INNER JOIN sys.columns thisColumns
    ON thisColumns.object_id = foreignKey.parent_object_id
        AND thisColumns.column_id = fkColumnsMap.parent_column_id
INNER JOIN sys.columns referencedColumns
    ON referencedColumns.object_id = foreignKey.referenced_object_id
        AND referencedColumns.column_id = fkColumnsMap.referenced_column_id
WHERE thisTable.name != '{DNDBTSysTables.DNDBTDbObjects}';";

        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();

        internal class ForeignKeyRecord : ForeignKeysBuilder.ForeignKeyRecord { }
        internal static class ResultsInterpreter
        {
            public static void BuildTablesForeignKeys(
                Dictionary<string, TableInfo> tables,
                IEnumerable<ForeignKeyRecord> foreignKeyRecords)
            {
                ForeignKeysBuilder.BuildTablesForeignKeys(tables, foreignKeyRecords, MapUpdateActionName);
            }

            private static string MapUpdateActionName(string sqlActionName) =>
                sqlActionName switch
                {
                    "NO_ACTION" => "NoAction",
                    "CASCADE" => "Cascade",
                    "SET_DEFAULT" => "SetDefault",
                    "SET_NULL" => "SetNull",
                    _ => throw new InvalidOperationException($"Invalid sqlActionName: '{sqlActionName}'")
                };
        }
    }
}
