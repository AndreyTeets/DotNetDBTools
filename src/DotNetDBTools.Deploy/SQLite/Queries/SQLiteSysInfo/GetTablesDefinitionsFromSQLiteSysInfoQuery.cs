using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Deploy.SQLite.Queries.SQLiteSysInfo
{
    internal class GetTablesDefinitionsFromSQLiteSysInfoQuery : IQuery
    {
        public string Sql =>
$@"SELECT
    sm.name AS {nameof(TableRecord.TableName)},
    sm.sql AS {nameof(TableRecord.TableDefinition)}
FROM sqlite_master sm
WHERE sm.type = 'table'
    AND sm.name!='sqlite_sequence';";

        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();

        internal class TableRecord
        {
            public string TableName { get; set; }
            public string TableDefinition { get; set; }
        }

        internal static class ResultsInterpreter
        {
            public static void BuildTablesConstraintNames(
                 Dictionary<string, SQLiteTableInfo> tables,
                IEnumerable<TableRecord> tableRecords)
            {
                foreach (TableRecord tableRecord in tableRecords)
                {
                    foreach (UniqueConstraintInfo uc in tables[tableRecord.TableName].UniqueConstraints)
                        uc.Name = GetUniqueConstraintName(tableRecord.TableDefinition, uc.Columns) ?? uc.Name;
                    foreach (ForeignKeyInfo fk in tables[tableRecord.TableName].ForeignKeys)
                        fk.Name = GetForeignKeyConstraintName(tableRecord.TableDefinition, fk.ThisColumnNames) ?? fk.Name;
                }
            }

            public static void ProcessTablesIdentityColumnCandidates(
                 Dictionary<string, SQLiteTableInfo> tables,
                IEnumerable<TableRecord> tableRecords)
            {
                foreach (TableRecord tableRecord in tableRecords)
                {
                    foreach (ColumnInfo column in tables[tableRecord.TableName].Columns.Where(c => c.Identity == true))
                    {
                        bool hasAutoincrementKeyWord = tableRecord.TableDefinition
                            .IndexOf("AUTOINCREMENT", StringComparison.OrdinalIgnoreCase) > 0;
                        if (!hasAutoincrementKeyWord)
                            column.Identity = false;
                    }
                }
            }

            private static string GetUniqueConstraintName(string tableDefinition, IEnumerable<string> columns)
            {
                string columnsList = string.Join(", ", columns);
                string pattern = @$"CONSTRAINT (?<constraintName>[\w|\d|_]+) UNIQUE \({columnsList}\)";
                Match match = Regex.Match(tableDefinition, pattern);
                if (match.Groups["constraintName"].Success)
                    return match.Groups["constraintName"].Value;
                return null;
            }

            private static string GetForeignKeyConstraintName(string tableDefinition, IEnumerable<string> thisColumns)
            {
                string thisColumnsList = string.Join(", ", thisColumns);
                string pattern = @$"CONSTRAINT (?<constraintName>[\w|\d|_]+) FOREIGN KEY \({thisColumnsList}\)";
                Match match = Regex.Match(tableDefinition, pattern);
                if (match.Groups["constraintName"].Success)
                    return match.Groups["constraintName"].Value;
                return null;
            }
        }
    }
}
