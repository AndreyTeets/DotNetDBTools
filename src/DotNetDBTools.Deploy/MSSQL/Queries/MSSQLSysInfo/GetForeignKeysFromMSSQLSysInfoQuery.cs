using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Deploy.MSSQL.Queries.MSSQLSysInfo
{
    internal class GetForeignKeysFromMSSQLSysInfoQuery : IQuery
    {
        public string Sql =>
$@"SELECT
    thisKey.TABLE_NAME AS {nameof(ForeignKeyRecord.TableName)},
    thisKey.CONSTRAINT_NAME AS {nameof(ForeignKeyRecord.ForeignKeyName)},
    thisKey.COLUMN_NAME AS {nameof(ForeignKeyRecord.ThisColumnName)},
    thisKey.ORDINAL_POSITION AS {nameof(ForeignKeyRecord.ThisColumnPosition)},
    referencedKey.TABLE_NAME AS {nameof(ForeignKeyRecord.ForeignTableName)},
    referencedKey.COLUMN_NAME AS {nameof(ForeignKeyRecord.ForeignColumnName)},
    referencedKey.ORDINAL_POSITION AS {nameof(ForeignKeyRecord.ForeignColumnPosition)},
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
            public string TableName { get; set; }
            public string ForeignKeyName { get; set; }
            public string ThisColumnName { get; set; }
            public int ThisColumnPosition { get; set; }
            public string ForeignTableName { get; set; }
            public string ForeignColumnName { get; set; }
            public int ForeignColumnPosition { get; set; }
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
                Dictionary<string, SortedDictionary<int, string>> foreignColumnNames = new();
                foreach (ForeignKeyRecord foreignKeyRecord in foreignKeyRecords)
                {
                    if (!thisColumnNames.ContainsKey(foreignKeyRecord.ForeignKeyName))
                        thisColumnNames.Add(foreignKeyRecord.ForeignKeyName, new SortedDictionary<int, string>());
                    if (!foreignColumnNames.ContainsKey(foreignKeyRecord.ForeignKeyName))
                        foreignColumnNames.Add(foreignKeyRecord.ForeignKeyName, new SortedDictionary<int, string>());

                    thisColumnNames[foreignKeyRecord.ForeignKeyName].Add(
                        foreignKeyRecord.ThisColumnPosition, foreignKeyRecord.ThisColumnName);
                    foreignColumnNames[foreignKeyRecord.ForeignKeyName].Add(
                        foreignKeyRecord.ForeignColumnPosition, foreignKeyRecord.ForeignColumnName);

                    MSSQLForeignKeyInfo foreignKeyInfo = MapExceptColumnsToForeignKeyInfo(foreignKeyRecord);
                    ((List<MSSQLForeignKeyInfo>)tables[foreignKeyRecord.TableName].ForeignKeys).Add(foreignKeyInfo);
                }

                foreach (MSSQLTableInfo table in tables.Values)
                {
                    foreach (MSSQLForeignKeyInfo foreignKeyInfo in table.ForeignKeys)
                    {
                        foreignKeyInfo.ThisColumnNames = thisColumnNames[foreignKeyInfo.Name].Select(x => x.Value).ToList();
                        foreignKeyInfo.ForeignColumnNames = foreignColumnNames[foreignKeyInfo.Name].Select(x => x.Value).ToList();
                    }
                }
            }

            private static MSSQLForeignKeyInfo MapExceptColumnsToForeignKeyInfo(ForeignKeyRecord foreignKeyRecord)
            {
                return new MSSQLForeignKeyInfo()
                {
                    ID = Guid.NewGuid(),
                    Name = foreignKeyRecord.ForeignKeyName,
                    ForeignTableName = foreignKeyRecord.ForeignTableName,
                    OnUpdate = MapUpdateActionName(foreignKeyRecord.OnUpdate),
                    OnDelete = MapUpdateActionName(foreignKeyRecord.OnDelete),
                };

                static string MapUpdateActionName(string sqlActionName) =>
                    sqlActionName switch
                    {
                        "NO ACTION" => "NoAction",
                        "CASCADE" => "Cascade",
                        _ => throw new InvalidOperationException($"Invalid sqlActionName: '{sqlActionName}'")
                    };
            }
        }
    }
}
