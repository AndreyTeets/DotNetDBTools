using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis;
using DotNetDBTools.Analysis.Extensions.PostgreSQL;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Editors;
using DotNetDBTools.Deploy.Core.Queries.DDL;
using DotNetDBTools.Deploy.PostgreSQL.Queries.DNDBTSysInfo;
using DotNetDBTools.Models;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;
using DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;

namespace DotNetDBTools.Deploy.PostgreSQL.Editors;

internal class PostgreSQLTablesEditor : TableEditor<
    PostgreSQLInsertDNDBTDbObjectRecordQuery,
    PostgreSQLDeleteDNDBTDbObjectRecordQuery,
    PostgreSQLUpdateDNDBTDbObjectRecordQuery,
    CreateTableQuery,
    AlterTableQuery>
{
    public PostgreSQLTablesEditor(IQueryExecutor queryExecutor)
        : base(queryExecutor) { }

    public void CreateTables_Without_ColumnsDefault_CK_FK(PostgreSQLDatabaseDiff dbDiff)
    {
        foreach (Table table in GetStrippedTableModels(dbDiff.AddedTables))
            CreateTable(table);
    }

    public void AlterTables_Except_ColumnsDefault_CK_FK(PostgreSQLDatabaseDiff dbDiff)
    {
        foreach (TableDiff tableDiff in GetStrippedTableDiffModels(dbDiff.ChangedTables))
            AlterTable(tableDiff);
    }

    public void SetColumnsDefault(PostgreSQLDatabaseDiff dbDiff)
    {
        foreach (TableDiff tableDiff in GetTableDiffsForSettingColumnsDefault(dbDiff))
            AlterTable(tableDiff);
    }

    public void DropColumnsDefault(PostgreSQLDatabaseDiff dbDiff)
    {
        foreach (TableDiff tableDiff in GetTableDiffsForDroppingColumnsDefault(dbDiff))
            AlterTable(tableDiff);
    }

    public void AddCheckConstraints(PostgreSQLDatabaseDiff dbDiff)
    {
        foreach (TableDiff tableDiff in GetTableDiffsForAddingCheckConstraints(dbDiff))
            AlterTable(tableDiff);
    }

    public void DropCheckConstraints(PostgreSQLDatabaseDiff dbDiff)
    {
        foreach (TableDiff tableDiff in GetTableDiffsForDroppingCheckConstraints(dbDiff))
            AlterTable(tableDiff);
    }

    private static List<Table> GetStrippedTableModels(IEnumerable<Table> tables)
    {
        List<Table> res = new();
        foreach (Table table in tables)
        {
            Table strippedTableModel = table.CopyModel();
            foreach (Column column in strippedTableModel.Columns.Where(x => x.Default is not null))
            {
                if (column.Default.DependsOn.Any(IsComplexDependency))
                    column.Default = null;
            }
            strippedTableModel.CheckConstraints.RemoveAll(x => x.Expression.DependsOn.Any(IsComplexDependency));
            res.Add(strippedTableModel);
        }
        return res;
    }

    private static List<TableDiff> GetStrippedTableDiffModels(IEnumerable<TableDiff> tableDiffs)
    {
        List<TableDiff> res = new();
        foreach (TableDiff tableDiff in tableDiffs)
        {
            TableDiff strippedTableDiffModel = tableDiff.CopyModel();
            foreach (Column column in strippedTableDiffModel.ColumnsToAdd.Where(x => x.Default is not null))
            {
                if (column.Default.DependsOn.Any(IsComplexDependency))
                    column.Default = null;
            }
            foreach (ColumnDiff columnDiff in strippedTableDiffModel.ColumnsToAlter.Where(x => x.DefaultToSet is not null))
                columnDiff.DefaultToSet = null;
            foreach (ColumnDiff columnDiff in strippedTableDiffModel.ColumnsToAlter.Where(x => x.DefaultToDrop is not null))
                columnDiff.DefaultToDrop = null;
            strippedTableDiffModel.ColumnsToAlter.RemoveAll(AnalysisManager.DiffIsEmpty);

            strippedTableDiffModel.CheckConstraintsToCreate.Clear();
            strippedTableDiffModel.CheckConstraintsToDrop.Clear();

            if (!AnalysisManager.DiffIsEmpty(strippedTableDiffModel))
                res.Add(strippedTableDiffModel);
        }
        return res;
    }

    private static List<TableDiff> GetTableDiffsForSettingColumnsDefault(PostgreSQLDatabaseDiff dbDiff)
    {
        List<TableDiff> res = new();
        foreach (Table table in dbDiff.AddedTables)
        {
            PostgreSQLTableDiff tableDiffForSettingColumnsDefault = table.CreateEmptyTableDiff();
            foreach (Column column in table.Columns.Where(x => x.Default is not null))
            {
                if (column.Default.DependsOn.Any(IsComplexDependency))
                {
                    ColumnDiff columnDiffWithDefaultChange = column.CreateEmptyColumnDiff();
                    columnDiffWithDefaultChange.DefaultToSet = column.Default;
                    tableDiffForSettingColumnsDefault.ColumnsToAlter.Add(columnDiffWithDefaultChange);
                }
            }

            if (tableDiffForSettingColumnsDefault.ColumnsToAlter.Count > 0)
                res.Add(tableDiffForSettingColumnsDefault);
        }
        foreach (TableDiff tableDiff in dbDiff.ChangedTables)
        {
            PostgreSQLTableDiff tableDiffForSettingColumnsDefault = new()
            {
                TableID = tableDiff.TableID,
                NewTableName = tableDiff.NewTableName,
                OldTableName = tableDiff.NewTableName,
            };
            foreach (Column column in tableDiff.ColumnsToAdd.Where(x => x.Default is not null))
            {
                ColumnDiff columnDiffWithDefaultChange = column.CreateEmptyColumnDiff();
                columnDiffWithDefaultChange.DefaultToSet = column.Default;
                tableDiffForSettingColumnsDefault.ColumnsToAlter.Add(columnDiffWithDefaultChange);
            }

            foreach (ColumnDiff columnDiff in tableDiff.ColumnsToAlter.Where(x => x.DefaultToSet is not null))
            {
                PostgreSQLColumnDiff columnDiffWithDefaultChange = new()
                {
                    ColumnID = columnDiff.ColumnID,
                    NewColumnName = columnDiff.NewColumnName,
                    OldColumnName = columnDiff.NewColumnName,
                };
                columnDiffWithDefaultChange.DefaultToSet = columnDiff.DefaultToSet;
                tableDiffForSettingColumnsDefault.ColumnsToAlter.Add(columnDiffWithDefaultChange);
            }

            if (tableDiffForSettingColumnsDefault.ColumnsToAlter.Count > 0)
                res.Add(tableDiffForSettingColumnsDefault);
        }
        return res;
    }

    private static List<TableDiff> GetTableDiffsForDroppingColumnsDefault(PostgreSQLDatabaseDiff dbDiff)
    {
        List<TableDiff> res = new();
        foreach (Table table in dbDiff.RemovedTables)
        {
            PostgreSQLTableDiff tableDiffForDroppingColumnsDefault = table.CreateEmptyTableDiff();
            foreach (Column column in table.Columns.Where(x => x.Default is not null))
            {
                if (column.Default.DependsOn.Any(IsComplexDependency))
                {
                    ColumnDiff columnDiffWithDefaultChange = column.CreateEmptyColumnDiff();
                    columnDiffWithDefaultChange.DefaultToDrop = column.Default;
                    tableDiffForDroppingColumnsDefault.ColumnsToAlter.Add(columnDiffWithDefaultChange);
                }
            }

            if (tableDiffForDroppingColumnsDefault.ColumnsToAlter.Count > 0)
                res.Add(tableDiffForDroppingColumnsDefault);
        }
        foreach (TableDiff tableDiff in dbDiff.ChangedTables)
        {
            PostgreSQLTableDiff tableDiffForDroppingColumnsDefault = new()
            {
                TableID = tableDiff.TableID,
                NewTableName = tableDiff.OldTableName,
                OldTableName = tableDiff.OldTableName,
            };
            foreach (Column column in tableDiff.ColumnsToDrop.Where(x => x.Default is not null))
            {
                ColumnDiff columnDiffWithDefaultChange = column.CreateEmptyColumnDiff();
                columnDiffWithDefaultChange.DefaultToDrop = column.Default;
                tableDiffForDroppingColumnsDefault.ColumnsToAlter.Add(columnDiffWithDefaultChange);
            }

            foreach (ColumnDiff columnDiff in tableDiff.ColumnsToAlter.Where(x => x.DefaultToDrop is not null))
            {
                PostgreSQLColumnDiff columnDiffWithDefaultChange = new()
                {
                    ColumnID = columnDiff.ColumnID,
                    NewColumnName = columnDiff.OldColumnName,
                    OldColumnName = columnDiff.OldColumnName,
                };
                columnDiffWithDefaultChange.DefaultToDrop = columnDiff.DefaultToDrop;
                tableDiffForDroppingColumnsDefault.ColumnsToAlter.Add(columnDiffWithDefaultChange);
            }

            if (tableDiffForDroppingColumnsDefault.ColumnsToAlter.Count > 0)
                res.Add(tableDiffForDroppingColumnsDefault);
        }
        return res;
    }

    private static List<TableDiff> GetTableDiffsForAddingCheckConstraints(PostgreSQLDatabaseDiff dbDiff)
    {
        List<TableDiff> res = new();
        foreach (Table table in dbDiff.AddedTables)
        {
            PostgreSQLTableDiff tableDiffForAddingCheckConstraints = table.CreateEmptyTableDiff();
            foreach (CheckConstraint ck in table.CheckConstraints.Where(x => x.Expression.DependsOn.Any(IsComplexDependency)))
                tableDiffForAddingCheckConstraints.CheckConstraintsToCreate.Add(ck);

            if (tableDiffForAddingCheckConstraints.CheckConstraintsToCreate.Count > 0)
                res.Add(tableDiffForAddingCheckConstraints);
        }
        foreach (TableDiff tableDiff in dbDiff.ChangedTables)
        {
            PostgreSQLTableDiff tableDiffForAddingCheckConstraints = new()
            {
                TableID = tableDiff.TableID,
                NewTableName = tableDiff.NewTableName,
                OldTableName = tableDiff.NewTableName,
            };
            foreach (CheckConstraint ck in tableDiff.CheckConstraintsToCreate)
                tableDiffForAddingCheckConstraints.CheckConstraintsToCreate.Add(ck);

            if (tableDiffForAddingCheckConstraints.CheckConstraintsToCreate.Count > 0)
                res.Add(tableDiffForAddingCheckConstraints);
        }
        return res;
    }

    private static List<TableDiff> GetTableDiffsForDroppingCheckConstraints(PostgreSQLDatabaseDiff dbDiff)
    {
        List<TableDiff> res = new();
        foreach (Table table in dbDiff.RemovedTables)
        {
            PostgreSQLTableDiff tableDiffForDroppingCheckConstraints = table.CreateEmptyTableDiff();
            foreach (CheckConstraint ck in table.CheckConstraints.Where(x => x.Expression.DependsOn.Any(IsComplexDependency)))
                tableDiffForDroppingCheckConstraints.CheckConstraintsToDrop.Add(ck);

            if (tableDiffForDroppingCheckConstraints.CheckConstraintsToDrop.Count > 0)
                res.Add(tableDiffForDroppingCheckConstraints);
        }
        foreach (TableDiff tableDiff in dbDiff.ChangedTables)
        {
            PostgreSQLTableDiff tableDiffForDroppingCheckConstraints = new()
            {
                TableID = tableDiff.TableID,
                NewTableName = tableDiff.OldTableName,
                OldTableName = tableDiff.OldTableName,
            };
            foreach (CheckConstraint ck in tableDiff.CheckConstraintsToDrop)
                tableDiffForDroppingCheckConstraints.CheckConstraintsToDrop.Add(ck);

            if (tableDiffForDroppingCheckConstraints.CheckConstraintsToDrop.Count > 0)
                res.Add(tableDiffForDroppingCheckConstraints);
        }
        return res;
    }

    private static bool IsComplexDependency(DbObject dbObject)
    {
        return dbObject switch
        {
            PostgreSQLSequence x => false,
            PostgreSQLCompositeType x => false,
            PostgreSQLDomainType x => false,
            PostgreSQLEnumType x => false,
            PostgreSQLRangeType x => false,
            PostgreSQLView x => x.CreateStatement.DependsOn.Any(IsComplexDependency),
            PostgreSQLFunction x => x.CreateStatement.DependsOn.Any(IsComplexDependency),
            PostgreSQLProcedure x => x.CreateStatement.DependsOn.Any(IsComplexDependency),
            _ => true,
        };
    }
}
