using System;
using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.ModelBuilders;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.SQLite.Queries.DBMSSysInfo
{
    internal class GetForeignKeysFromDBMSSysInfoQuery : IQuery
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

        internal class ForeignKeyRecord : ForeignKeysBuilder.ForeignKeyRecord { }
        internal static class ResultsInterpreter
        {
            public static void BuildTablesForeignKeys(
                Dictionary<string, Table> tables,
                IEnumerable<ForeignKeyRecord> foreignKeyRecords)
            {
                ForeignKeysBuilder.BuildTablesForeignKeys(tables, foreignKeyRecords, MapUpdateActionName);
            }

            private static string MapUpdateActionName(string sqlActionName) =>
                sqlActionName switch
                {
                    "NO ACTION" => "NoAction",
                    "RESTRICT" => "Restrict",
                    "CASCADE" => "Cascade",
                    "SET DEFAULT" => "SetDefault",
                    "SET NULL" => "SetNull",
                    _ => throw new InvalidOperationException($"Invalid sqlActionName: '{sqlActionName}'")
                };
        }
    }
}
