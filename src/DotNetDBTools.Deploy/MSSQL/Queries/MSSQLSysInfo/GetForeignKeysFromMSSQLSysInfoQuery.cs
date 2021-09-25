using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Deploy.MSSQL.Queries.MSSQLSysInfo
{
    internal class GetForeignKeysFromMSSQLSysInfoQuery : IQuery
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

        internal class ForeignKeyRecord
        {
            public string ThisTableName { get; set; }
            public string ForeignKeyName { get; set; }
            public string ThisColumnName { get; set; }
            public int ThisColumnPosition { get; set; }
            public string ReferencedTableName { get; set; }
            public string ReferencedColumnName { get; set; }
            public int ReferencedColumnPosition { get; set; }
            public string OnUpdate { get; set; }
            public string OnDelete { get; set; }
        }

        internal static class ResultsInterpreter
        {
            public static void BuildTablesForeignKeys(
                Dictionary<string, MSSQLTableInfo> tables,
                IEnumerable<ForeignKeyRecord> foreignKeyRecords)
            {
                Dictionary<string, SortedDictionary<int, string>> thisColumnNames = new();
                Dictionary<string, SortedDictionary<int, string>> referencedColumnNames = new();
                Dictionary<string, HashSet<string>> addedForeignKeysForTable = new();
                foreach (ForeignKeyRecord fkr in foreignKeyRecords)
                {
                    if (!addedForeignKeysForTable.ContainsKey(fkr.ThisTableName))
                        addedForeignKeysForTable.Add(fkr.ThisTableName, new HashSet<string>());

                    if (!thisColumnNames.ContainsKey(fkr.ForeignKeyName))
                        thisColumnNames.Add(fkr.ForeignKeyName, new SortedDictionary<int, string>());
                    if (!referencedColumnNames.ContainsKey(fkr.ForeignKeyName))
                        referencedColumnNames.Add(fkr.ForeignKeyName, new SortedDictionary<int, string>());

                    thisColumnNames[fkr.ForeignKeyName].Add(
                        fkr.ThisColumnPosition, fkr.ThisColumnName);
                    referencedColumnNames[fkr.ForeignKeyName].Add(
                        fkr.ReferencedColumnPosition, fkr.ReferencedColumnName);

                    if (!addedForeignKeysForTable[fkr.ThisTableName].Contains(fkr.ForeignKeyName))
                    {
                        ForeignKeyInfo foreignKeyInfo = MapExceptColumnsToForeignKeyInfo(fkr);
                        ((List<ForeignKeyInfo>)tables[fkr.ThisTableName].ForeignKeys).Add(foreignKeyInfo);
                        addedForeignKeysForTable[fkr.ThisTableName].Add(fkr.ForeignKeyName);
                    }
                }

                foreach (MSSQLTableInfo table in tables.Values)
                {
                    foreach (ForeignKeyInfo foreignKeyInfo in table.ForeignKeys)
                    {
                        foreignKeyInfo.ThisColumnNames = thisColumnNames[foreignKeyInfo.Name].Select(x => x.Value).ToList();
                        foreignKeyInfo.ReferencedTableColumnNames = referencedColumnNames[foreignKeyInfo.Name].Select(x => x.Value).ToList();
                    }
                }
            }

            private static ForeignKeyInfo MapExceptColumnsToForeignKeyInfo(ForeignKeyRecord foreignKeyRecord)
            {
                return new ForeignKeyInfo()
                {
                    ID = Guid.NewGuid(),
                    Name = foreignKeyRecord.ForeignKeyName,
                    ReferencedTableName = foreignKeyRecord.ReferencedTableName,
                    OnUpdate = MapUpdateActionName(foreignKeyRecord.OnUpdate),
                    OnDelete = MapUpdateActionName(foreignKeyRecord.OnDelete),
                };

                static string MapUpdateActionName(string sqlActionName) =>
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
}
