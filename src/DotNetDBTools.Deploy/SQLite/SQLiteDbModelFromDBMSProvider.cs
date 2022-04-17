using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis.SQLite;
using DotNetDBTools.CodeParsing.Core.Models;
using DotNetDBTools.CodeParsing.SQLite;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.SQLite.Queries.DBMSSysInfo;
using DotNetDBTools.Deploy.SQLite.Queries.DNDBTSysInfo;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;
using static DotNetDBTools.Deploy.SQLite.Queries.DBMSSysInfo.SQLiteGetTablesDefinitionsFromDBMSSysInfoQuery;

namespace DotNetDBTools.Deploy.SQLite;

internal class SQLiteDbModelFromDBMSProvider : DbModelFromDBMSProvider<
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
    SQLiteGetDNDBTDbAttributesRecordQuery,
    SQLiteGetDNDBTDbObjectRecordsQuery,
    SQLiteGetDNDBTScriptExecutionRecordsQuery>
{
    public SQLiteDbModelFromDBMSProvider(IQueryExecutor queryExecutor)
        : base(queryExecutor, new SQLiteDbModelPostProcessor()) { }

    protected override void BuildAdditionalTablesAttributes(Dictionary<string, Table> tables)
    {
        SQLiteGetTablesDefinitionsFromDBMSSysInfoQuery query = new();
        IEnumerable<TableRecord> tableRecords = QueryExecutor.Query<TableRecord>(query);
        foreach (TableRecord tableRecord in tableRecords)
        {
            SQLiteCodeParser parser = new();
            TableInfo tableInfo = (TableInfo)parser.GetObjectInfo(tableRecord.TableDefinition);
            Table table = tables[tableRecord.TableName];
            BuildTableCheckConstraints(table, tableInfo);
            BuildTableConstraintNames(table, tableInfo);
            SetTableIdentityColumnIfAny(table, tableInfo);
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

    private void SetTableIdentityColumnIfAny(Table table, TableInfo tableInfo)
    {
        foreach (ColumnInfo columnInfo in tableInfo.Columns.Where(c => c.Identity == true))
        {
            Column column = table.Columns.Single(x => x.Name == columnInfo.Name);
            column.Identity = true;
        }
    }
}
