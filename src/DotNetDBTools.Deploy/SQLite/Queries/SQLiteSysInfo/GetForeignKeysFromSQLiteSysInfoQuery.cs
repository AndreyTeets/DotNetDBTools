using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Deploy.SQLite.Queries.SQLiteSysInfo
{
    internal class GetForeignKeysFromSQLiteSysInfoQuery : IQuery
    {
        public string Sql =>
$@"SELECT
    sm.name AS {nameof(ForeignKeyRecord.ThisTableName)},
    'FK_' || sm.name || '_' || fkl.[table] || '_' || fkl.id AS {nameof(ForeignKeyRecord.ForeignKeyName)},
    fkl.[from] AS {nameof(ForeignKeyRecord.ThisColumnName)},
    fkl.seq AS {nameof(ForeignKeyRecord.ThisColumnPosition)},
    fkl.[table] AS {nameof(ForeignKeyRecord.ReferencedTableName)},
    fkl.[to] AS {nameof(ForeignKeyRecord.ReferencedColumnName)},
    fkl.seq AS {nameof(ForeignKeyRecord.ReferencedColumnPosition)},
    fkl.on_update AS {nameof(ForeignKeyRecord.OnUpdate)},
    fkl.on_delete AS {nameof(ForeignKeyRecord.OnDelete)}
FROM sqlite_master sm
INNER JOIN pragma_foreign_key_list(sm.name) fkl
WHERE sm.type = 'table'
    AND sm.name != 'sqlite_sequence'
    AND sm.name != '{DNDBTSysTables.DNDBTDbObjects}';";

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
                Dictionary<string, SQLiteTableInfo> tables,
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

                foreach (SQLiteTableInfo table in tables.Values)
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
                        "NO ACTION" => "NoAction",
                        "CASCADE" => "Cascade",
                        "SET DEFAULT" => "SetDefault",
                        "SET NULL" => "SetNull",
                        _ => throw new InvalidOperationException($"Invalid sqlActionName: '{sqlActionName}'")
                    };
            }
        }
    }
}
