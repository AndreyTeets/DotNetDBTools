using System;
using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.ModelBuilders;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.MySQL.Queries.DBMSSysInfo
{
    internal class GetForeignKeysFromDBMSSysInfoQuery : IQuery
    {
        public string Sql =>
$@"SELECT
    tc.TABLE_NAME AS {nameof(ForeignKeyRecord.ThisTableName)},
    tc.CONSTRAINT_NAME AS {nameof(ForeignKeyRecord.ForeignKeyName)},
    kcu.COLUMN_NAME AS {nameof(ForeignKeyRecord.ThisColumnName)},
    kcu.ORDINAL_POSITION AS {nameof(ForeignKeyRecord.ThisColumnPosition)},
    kcu.REFERENCED_TABLE_NAME AS {nameof(ForeignKeyRecord.ReferencedTableName)},
    kcu.REFERENCED_COLUMN_NAME AS {nameof(ForeignKeyRecord.ReferencedColumnName)},
    kcu.POSITION_IN_UNIQUE_CONSTRAINT AS {nameof(ForeignKeyRecord.ReferencedColumnPosition)},
    rc.UPDATE_RULE AS {nameof(ForeignKeyRecord.OnUpdate)},
    rc.DELETE_RULE AS {nameof(ForeignKeyRecord.OnDelete)}
FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc
INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE kcu
    ON kcu.CONSTRAINT_SCHEMA = tc.CONSTRAINT_SCHEMA
        AND kcu.TABLE_NAME = tc.TABLE_NAME
        AND kcu.CONSTRAINT_NAME = tc.CONSTRAINT_NAME
INNER JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS rc
    ON rc.CONSTRAINT_SCHEMA = tc.CONSTRAINT_SCHEMA
        AND rc.TABLE_NAME = tc.TABLE_NAME
        AND rc.CONSTRAINT_NAME = tc.CONSTRAINT_NAME
WHERE tc.CONSTRAINT_SCHEMA = (select DATABASE())
    AND tc.CONSTRAINT_TYPE = 'FOREIGN KEY'
    AND tc.TABLE_NAME != '{DNDBTSysTables.DNDBTDbObjects}';";

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
