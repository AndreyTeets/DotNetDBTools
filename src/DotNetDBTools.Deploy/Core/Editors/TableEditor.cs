﻿using System;
using System.Linq;
using DotNetDBTools.Deploy.Core.Queries.DDL;
using DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;
using DotNetDBTools.Generation.Core;
using DotNetDBTools.Models.Core;
using static DotNetDBTools.Deploy.Core.InstanceCreator;

namespace DotNetDBTools.Deploy.Core.Editors;

internal abstract class TableEditor<
    TInsertDNDBTDbObjectRecordQuery,
    TDeleteDNDBTDbObjectRecordQuery,
    TUpdateDNDBTDbObjectRecordQuery,
    TCreateTableQuery,
    TAlterTableQuery>
    : ITableEditor
    where TInsertDNDBTDbObjectRecordQuery : InsertDNDBTDbObjectRecordQuery
    where TDeleteDNDBTDbObjectRecordQuery : DeleteDNDBTDbObjectRecordQuery
    where TUpdateDNDBTDbObjectRecordQuery : UpdateDNDBTDbObjectRecordQuery
    where TCreateTableQuery : CreateTableQuery
    where TAlterTableQuery : AlterTableQuery
{
    protected readonly IQueryExecutor QueryExecutor;

    protected TableEditor(IQueryExecutor queryExecutor)
    {
        QueryExecutor = queryExecutor;
    }

    public void CreateTables(DatabaseDiff dbDiff)
    {
        foreach (Table table in dbDiff.AddedTables)
            CreateTable(table);
    }

    public void DropTables(DatabaseDiff dbDiff)
    {
        foreach (Table table in dbDiff.RemovedTables)
            DropTable(table);
    }

    public void AlterTables(DatabaseDiff dbDiff)
    {
        foreach (TableDiff tableDiff in dbDiff.ChangedTables)
            AlterTable(tableDiff);
    }

    protected virtual void CreateTable(Table table)
    {
        QueryExecutor.Execute(Create<TCreateTableQuery>(table));
        QueryExecutor.Execute(Create<TInsertDNDBTDbObjectRecordQuery>(table.ID, null, DbObjectType.Table, table.Name));
        foreach (Column c in table.Columns)
            QueryExecutor.Execute(Create<TInsertDNDBTDbObjectRecordQuery>(c.ID, table.ID, DbObjectType.Column, c.Name, c.GetDefault()));
        PrimaryKey pk = table.PrimaryKey;
        if (pk is not null)
            QueryExecutor.Execute(Create<TInsertDNDBTDbObjectRecordQuery>(pk.ID, table.ID, DbObjectType.PrimaryKey, pk.Name));
        foreach (UniqueConstraint uc in table.UniqueConstraints)
            QueryExecutor.Execute(Create<TInsertDNDBTDbObjectRecordQuery>(uc.ID, table.ID, DbObjectType.UniqueConstraint, uc.Name));
        foreach (CheckConstraint ck in table.CheckConstraints)
            QueryExecutor.Execute(Create<TInsertDNDBTDbObjectRecordQuery>(ck.ID, table.ID, DbObjectType.CheckConstraint, ck.Name, ck.GetExpression()));
    }

    protected virtual void DropTable(Table table)
    {
        QueryExecutor.Execute(new DropTableQuery(table));
        foreach (CheckConstraint ck in table.CheckConstraints)
            QueryExecutor.Execute(Create<TDeleteDNDBTDbObjectRecordQuery>(ck.ID));
        foreach (UniqueConstraint uc in table.UniqueConstraints)
            QueryExecutor.Execute(Create<TDeleteDNDBTDbObjectRecordQuery>(uc.ID));
        PrimaryKey pk = table.PrimaryKey;
        if (pk is not null)
            QueryExecutor.Execute(Create<TDeleteDNDBTDbObjectRecordQuery>(pk.ID));
        foreach (Column column in table.Columns)
            QueryExecutor.Execute(Create<TDeleteDNDBTDbObjectRecordQuery>(column.ID));
        QueryExecutor.Execute(Create<TDeleteDNDBTDbObjectRecordQuery>(table.ID));
    }

    protected virtual void AlterTable(TableDiff tableDiff)
    {
        QueryExecutor.Execute(Create<TAlterTableQuery>(tableDiff));

        foreach (CheckConstraint ck in tableDiff.CheckConstraintsToDrop)
            QueryExecutor.Execute(Create<TDeleteDNDBTDbObjectRecordQuery>(ck.ID));
        foreach (UniqueConstraint uc in tableDiff.UniqueConstraintsToDrop)
            QueryExecutor.Execute(Create<TDeleteDNDBTDbObjectRecordQuery>(uc.ID));
        if (tableDiff.PrimaryKeyToDrop is not null)
            QueryExecutor.Execute(Create<TDeleteDNDBTDbObjectRecordQuery>(tableDiff.PrimaryKeyToDrop.ID));
        foreach (Column column in tableDiff.ColumnsToDrop)
            QueryExecutor.Execute(Create<TDeleteDNDBTDbObjectRecordQuery>(column.ID));

        Guid tableID = tableDiff.TableID;
        if (tableDiff.NewTableName != tableDiff.OldTableName)
            QueryExecutor.Execute(Create<TUpdateDNDBTDbObjectRecordQuery>(tableID, tableDiff.NewTableName));

        foreach (ColumnDiff cDiff in tableDiff.ColumnsToAlter.Where(x =>
            x.NewColumnName != x.OldColumnName
            || DefaultCodeChanged(x)))
        {
            string objectCode = cDiff.DefaultToSet is not null ? cDiff.DefaultToSet.Code : null;
            QueryExecutor.Execute(Create<TUpdateDNDBTDbObjectRecordQuery>(
                cDiff.ColumnID, cDiff.NewColumnName, DefaultCodeChanged(cDiff), objectCode));
        }

        foreach (Column c in tableDiff.ColumnsToAdd)
            QueryExecutor.Execute(Create<TInsertDNDBTDbObjectRecordQuery>(c.ID, tableID, DbObjectType.Column, c.Name, c.GetDefault()));
        PrimaryKey pk = tableDiff.PrimaryKeyToCreate;
        if (pk is not null)
            QueryExecutor.Execute(Create<TInsertDNDBTDbObjectRecordQuery>(pk.ID, tableID, DbObjectType.PrimaryKey, pk.Name));
        foreach (UniqueConstraint uc in tableDiff.UniqueConstraintsToCreate)
            QueryExecutor.Execute(Create<TInsertDNDBTDbObjectRecordQuery>(uc.ID, tableID, DbObjectType.UniqueConstraint, uc.Name));
        foreach (CheckConstraint ck in tableDiff.CheckConstraintsToCreate)
            QueryExecutor.Execute(Create<TInsertDNDBTDbObjectRecordQuery>(ck.ID, tableID, DbObjectType.CheckConstraint, ck.Name, ck.GetExpression()));

        static bool DefaultCodeChanged(ColumnDiff cDiff)
        {
            return cDiff.DefaultToSet is not null && cDiff.DefaultToDrop is null
                || cDiff.DefaultToSet is null && cDiff.DefaultToDrop is not null
                || cDiff.DefaultToSet is not null && cDiff.DefaultToDrop is not null
                    && cDiff.DefaultToSet.Code != cDiff.DefaultToDrop.Code;
        }
    }
}
