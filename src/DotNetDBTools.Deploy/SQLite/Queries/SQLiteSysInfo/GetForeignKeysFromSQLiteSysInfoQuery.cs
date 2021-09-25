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
    AND sm.name!='sqlite_sequence';";

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
                foreach (ForeignKeyRecord foreignKeyRecord in foreignKeyRecords)
                {
                    if (!thisColumnNames.ContainsKey(foreignKeyRecord.ForeignKeyName))
                        thisColumnNames.Add(foreignKeyRecord.ForeignKeyName, new SortedDictionary<int, string>());
                    if (!referencedColumnNames.ContainsKey(foreignKeyRecord.ForeignKeyName))
                        referencedColumnNames.Add(foreignKeyRecord.ForeignKeyName, new SortedDictionary<int, string>());

                    thisColumnNames[foreignKeyRecord.ForeignKeyName].Add(
                        foreignKeyRecord.ThisColumnPosition, foreignKeyRecord.ThisColumnName);
                    referencedColumnNames[foreignKeyRecord.ForeignKeyName].Add(
                        foreignKeyRecord.ReferencedColumnPosition, foreignKeyRecord.ReferencedColumnName);

                    ForeignKeyInfo foreignKeyInfo = MapExceptColumnsToForeignKeyInfo(foreignKeyRecord);
                    ((List<ForeignKeyInfo>)tables[foreignKeyRecord.ThisTableName].ForeignKeys).Add(foreignKeyInfo);
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
                    ThisTableName = foreignKeyRecord.ThisTableName,
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
                        "NOT NULL" => "SetNull",
                        _ => throw new InvalidOperationException($"Invalid sqlActionName: '{sqlActionName}'")
                    };
            }
        }
    }
}
