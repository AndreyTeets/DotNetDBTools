using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.CodeParsing.Core.Models;
using DotNetDBTools.CodeParsing.SQLite;
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
        SQLiteView,
        SQLiteGetColumnsFromDBMSSysInfoQuery,
        SQLiteGetPrimaryKeysFromDBMSSysInfoQuery,
        SQLiteGetUniqueConstraintsFromDBMSSysInfoQuery,
        SQLiteGetCheckConstraintsFromDBMSSysInfoQuery,
        SQLiteGetIndexesFromDBMSSysInfoQuery,
        SQLiteGetTriggersFromDBMSSysInfoQuery,
        SQLiteGetForeignKeysFromDBMSSysInfoQuery,
        SQLiteGetViewsFromDBMSSysInfoQuery,
        SQLiteGetAllDbObjectsFromDNDBTSysInfoQuery>
    {
        public SQLiteDbModelFromDbSysInfoBuilder(IQueryExecutor queryExecutor)
            : base(queryExecutor) { }

        protected override void BuildAdditionalTablesAttributes(Dictionary<string, Table> tables)
        {
            SQLiteGetTablesDefinitionsFromDBMSSysInfoQuery query = new();
            IEnumerable<TableRecord> tableRecords = QueryExecutor.Query<TableRecord>(query);
            foreach (TableRecord tableRecord in tableRecords)
            {
                SQLiteCodeParser parser = new();
                TableInfo tableInfo = (TableInfo)parser.GetModelFromCreateStatement(tableRecord.TableDefinition);
                Table table = tables[tableRecord.TableName];
                BuildTableCheckConstraints(table, tableInfo);
                BuildTableConstraintNames(table, tableInfo);
                ProcessTableIdentityColumnCandidateIfExist(table, tableInfo);
            }
        }

        private static void BuildTableCheckConstraints(Table table, TableInfo tableInfo)
        {
            foreach (ConstraintInfo ckInfo in tableInfo.Constraints.Where(x => x.Type == ConstraintType.Check))
            {
                CheckConstraint ck = new()
                {
                    ID = Guid.NewGuid(),
                    Name = ckInfo.Name,
                    CodePiece = new CodePiece { Code = ckInfo.Code },
                };
                ((List<CheckConstraint>)table.CheckConstraints).Add(ck);
            }
        }

        private void BuildTableConstraintNames(Table table, TableInfo tableInfo)
        {
            Dictionary<string, string> uniqueConstraintColsToNameMap = new();
            Dictionary<string, string> fkConstraintColsToNameMap = new();
            foreach (ConstraintInfo ckInfo in tableInfo.Constraints.Where(x => x.Type == ConstraintType.Unique))
                uniqueConstraintColsToNameMap.Add(CreateKeyFromColumns(ckInfo.Columns), ckInfo.Name);
            foreach (ConstraintInfo fkInfo in tableInfo.Constraints.Where(x => x.Type == ConstraintType.ForeignKey))
                fkConstraintColsToNameMap.Add(CreateKeyFromColumns(fkInfo.Columns), fkInfo.Name);

            foreach (UniqueConstraint uc in table.UniqueConstraints)
                uc.Name = uniqueConstraintColsToNameMap[CreateKeyFromColumns(uc.Columns)] ?? uc.Name;
            foreach (ForeignKey fk in table.ForeignKeys)
                fk.Name = fkConstraintColsToNameMap[CreateKeyFromColumns(fk.ThisColumnNames)] ?? fk.Name;

            static string CreateKeyFromColumns(IEnumerable<string> columns) => string.Join(",", columns);
        }

        private void ProcessTableIdentityColumnCandidateIfExist(Table table, TableInfo tableInfo)
        {
            Column column = table.Columns.SingleOrDefault(c => c.Identity == true);
            if (column is not null)
            {
                ColumnInfo columnInfo = tableInfo.Columns.Single(x => x.Name == column.Name);
                if (columnInfo.Autoincrement == false)
                    column.Identity = false;
            }
        }
    }
}
