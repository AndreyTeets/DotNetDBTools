using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.SQLite.Queries.DBMSSysInfo;
using DotNetDBTools.Deploy.SQLite.Queries.DNDBTSysInfo;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;
using static DotNetDBTools.Deploy.SQLite.Queries.DBMSSysInfo.SQLiteGetTablesDefinitionsFromDBMSSysInfoQuery;

namespace DotNetDBTools.Deploy.SQLite
{
    internal class SQLiteDbModelFromDbSysInfoBuilder : DbModelFromDbSysInfoBuilder<
        SQLiteDatabase,
        SQLiteTable,
        SQLiteGetColumnsFromDBMSSysInfoQuery,
        SQLiteGetPrimaryKeysFromDBMSSysInfoQuery,
        SQLiteGetUniqueConstraintsFromDBMSSysInfoQuery,
        SQLiteGetCheckConstraintsFromDBMSSysInfoQuery,
        SQLiteGetForeignKeysFromDBMSSysInfoQuery,
        SQLiteGetAllDbObjectsFromDNDBTSysInfoQuery>
    {
        public SQLiteDbModelFromDbSysInfoBuilder(IQueryExecutor queryExecutor)
            : base(queryExecutor) { }

        protected override void BuildAdditionalTablesAttributes(Dictionary<string, Table> tables)
        {
            SQLiteGetTablesDefinitionsFromDBMSSysInfoQuery query = new();
            IEnumerable<TableRecord> tableRecords = QueryExecutor.Query<TableRecord>(query);
            BuildCheckConstraints(tables, tableRecords);
            BuildTablesConstraintNames(tables, tableRecords);
            ProcessTablesIdentityColumnCandidates(tables, tableRecords);
        }

        private static void BuildCheckConstraints(
            Dictionary<string, Table> tables,
            IEnumerable<TableRecord> tableRecords)
        {
            foreach (TableRecord tableRecord in tableRecords)
            {
                string pattern = @$"CONSTRAINT (?<ConstraintName>[\[|\]|\w|\d|_]+) CHECK \((?<ConstraintCode>.+)\)";
                MatchCollection matches = Regex.Matches(tableRecord.TableDefinition, pattern);
                foreach (Match match in matches)
                {
                    CheckConstraint ck = new()
                    {
                        ID = Guid.NewGuid(),
                        Name = GetIdentifierName(match.Groups["ConstraintName"].Value),
                        CodePiece = new CodePiece { Code = $@"CHECK ({match.Groups["ConstraintCode"].Value})" },
                    };
                    ((List<CheckConstraint>)tables[tableRecord.TableName].CheckConstraints).Add(ck);
                }
                // TODO constraint that was defined without name or for a column
            }
        }

        private void BuildTablesConstraintNames(
            Dictionary<string, Table> tables,
            IEnumerable<TableRecord> tableRecords)
        {
            foreach (TableRecord tableRecord in tableRecords)
            {
                foreach (UniqueConstraint uc in tables[tableRecord.TableName].UniqueConstraints)
                    uc.Name = GetUniqueConstraintName(tableRecord.TableDefinition, uc.Columns) ?? uc.Name;
                foreach (ForeignKey fk in tables[tableRecord.TableName].ForeignKeys)
                    fk.Name = GetForeignKeyConstraintName(tableRecord.TableDefinition, fk.ThisColumnNames) ?? fk.Name;
            }
        }

        private void ProcessTablesIdentityColumnCandidates(
            Dictionary<string, Table> tables,
            IEnumerable<TableRecord> tableRecords)
        {
            foreach (TableRecord tableRecord in tableRecords)
            {
                foreach (Column column in tables[tableRecord.TableName].Columns.Where(c => c.Identity == true))
                {
                    bool hasAutoincrementKeyWord = tableRecord.TableDefinition
                        .IndexOf("AUTOINCREMENT", StringComparison.OrdinalIgnoreCase) > -1;
                    if (!hasAutoincrementKeyWord)
                        column.Identity = false;
                }
            }
        }

        private static string GetUniqueConstraintName(string tableDefinition, IEnumerable<string> columns)
        {
            string columnsList = string.Join(", ", columns);
            string pattern = @$"CONSTRAINT (?<ConstraintName>[\[|\]|\w|\d|_]+) UNIQUE \({columnsList}\)";
            Match match = Regex.Match(tableDefinition, pattern);
            if (match.Groups["ConstraintName"].Success)
                return GetIdentifierName(match.Groups["ConstraintName"].Value);
            return null;
        }

        private static string GetForeignKeyConstraintName(string tableDefinition, IEnumerable<string> thisColumns)
        {
            string thisColumnsList = string.Join(", ", thisColumns);
            string pattern = @$"CONSTRAINT (?<ConstraintName>[\[|\]|\w|\d|_]+) FOREIGN KEY \({thisColumnsList}\)";
            Match match = Regex.Match(tableDefinition, pattern);
            if (match.Groups["ConstraintName"].Success)
                return GetIdentifierName(match.Groups["ConstraintName"].Value);
            return null;
        }

        private static string GetIdentifierName(string value)
        {
            return value.Replace("[", "").Replace("]", "");
        }
    }
}
