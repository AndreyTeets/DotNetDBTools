using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.SQLite.Queries.DBMSSysInfo
{
    internal class GetTablesDefinitionsFromDBMSSysInfoQuery : IQuery
    {
        public string Sql =>
$@"SELECT
    sm.name AS {nameof(TableRecord.TableName)},
    sm.sql AS {nameof(TableRecord.TableDefinition)}
FROM sqlite_master sm
WHERE sm.type = 'table'
    AND sm.name != 'sqlite_sequence'
    AND sm.name != '{DNDBTSysTables.DNDBTDbObjects}';";

        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();

        internal class TableRecord
        {
            public string TableName { get; set; }
            public string TableDefinition { get; set; }
        }

        internal static class ResultsInterpreter
        {
            public static void BuildTablesConstraintNames(
                 Dictionary<string, Table> tables,
                IEnumerable<TableRecord> tableRecords)
            {
                foreach (TableRecord tableRecord in tableRecords)
                {
                    foreach (Column c in tables[tableRecord.TableName].Columns)
                        c.DefaultConstraintName = GetDefaultConstraintName(tableRecord.TableDefinition, c) ?? c.DefaultConstraintName;
                    foreach (UniqueConstraint uc in tables[tableRecord.TableName].UniqueConstraints)
                        uc.Name = GetUniqueConstraintName(tableRecord.TableDefinition, uc.Columns) ?? uc.Name;
                    foreach (ForeignKey fk in tables[tableRecord.TableName].ForeignKeys)
                        fk.Name = GetForeignKeyConstraintName(tableRecord.TableDefinition, fk.ThisColumnNames) ?? fk.Name;
                }
            }

            public static void ProcessTablesIdentityColumnCandidates(
                 Dictionary<string, Table> tables,
                IEnumerable<TableRecord> tableRecords)
            {
                foreach (TableRecord tableRecord in tableRecords)
                {
                    foreach (Column column in tables[tableRecord.TableName].Columns.Where(c => c.Identity == true))
                    {
                        bool hasAutoincrementKeyWord = tableRecord.TableDefinition
                            .IndexOf("AUTOINCREMENT", StringComparison.OrdinalIgnoreCase) > 0;
                        if (!hasAutoincrementKeyWord)
                            column.Identity = false;
                    }
                }
            }

            private static string GetDefaultConstraintName(string tableDefinition, Column column)
            {
                if (column.Default is null)
                    return null;
                string pattern = @$" {column.Name} [\s|\w|\d|_]+ NULL CONSTRAINT (?<constraintName>[\w|\d|_]+) DEFAULT ";
                Match match = Regex.Match(tableDefinition, pattern);
                if (match.Groups["constraintName"].Success)
                    return match.Groups["constraintName"].Value;
                return null;
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
