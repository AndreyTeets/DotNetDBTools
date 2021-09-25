﻿using System;
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
    thisKey.TABLE_NAME AS {nameof(ForeignKeyRecord.ThisTableName)},
    thisKey.CONSTRAINT_NAME AS {nameof(ForeignKeyRecord.ForeignKeyName)},
    thisKey.COLUMN_NAME AS {nameof(ForeignKeyRecord.ThisColumnName)},
    thisKey.ORDINAL_POSITION AS {nameof(ForeignKeyRecord.ThisColumnPosition)},
    referencedKey.TABLE_NAME AS {nameof(ForeignKeyRecord.ReferencedTableName)},
    referencedKey.COLUMN_NAME AS {nameof(ForeignKeyRecord.ReferencedColumnName)},
    referencedKey.ORDINAL_POSITION AS {nameof(ForeignKeyRecord.ReferencedColumnPosition)},
    keyMap.UPDATE_RULE AS {nameof(ForeignKeyRecord.OnUpdate)},
    keyMap.DELETE_RULE AS {nameof(ForeignKeyRecord.OnDelete)}
FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS keyMap
INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE thisKey
    ON thisKey.CONSTRAINT_NAME = keyMap.CONSTRAINT_NAME
INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE referencedKey
    ON referencedKey.CONSTRAINT_NAME = keyMap.UNIQUE_CONSTRAINT_NAME;";

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
