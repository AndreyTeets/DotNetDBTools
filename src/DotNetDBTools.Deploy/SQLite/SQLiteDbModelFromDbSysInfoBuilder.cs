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
        SQLiteGetForeignKeysFromDBMSSysInfoQuery,
        SQLiteGetAllDbObjectsFromDNDBTSysInfoQuery>
    {
        public SQLiteDbModelFromDbSysInfoBuilder(IQueryExecutor queryExecutor)
            : base(queryExecutor) { }

        protected override void BuildAdditionalTablesAttributes(Dictionary<string, Table> tables)
        {
            SQLiteGetTablesDefinitionsFromDBMSSysInfoQuery query = new();
            IEnumerable<TableRecord> tableRecords = QueryExecutor.Query<TableRecord>(query);
            BuildTablesConstraintNames(tables, tableRecords);
            ProcessTablesIdentityColumnCandidates(tables, tableRecords);
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
