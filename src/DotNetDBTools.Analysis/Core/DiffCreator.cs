using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.Core;

internal abstract class DiffCreator
{
    private readonly DNDBTModelsEqualityComparer _comparer = new();

    public DiffCreator()
    {
        _comparer.IgnoredProperties.Add(new PropInfo { Name = nameof(Table.Indexes), InType = nameof(Table) });
        _comparer.IgnoredProperties.Add(new PropInfo { Name = nameof(Table.Triggers), InType = nameof(Table) });
    }

    public abstract DatabaseDiff CreateDatabaseDiff(Database newDatabase, Database oldDatabase);

    protected void BuildTablesDiff<TTableDiff>(DatabaseDiff dbDiff)
        where TTableDiff : TableDiff, new()
    {
        List<Table> addedTables = null;
        List<Table> removedTables = null;
        List<TableDiff> changedTables = new();
        FillAddedAndRemovedItemsAndApplyActionToChangedItems(
            dbDiff.NewDatabase.Tables, dbDiff.OldDatabase.Tables,
            ref addedTables, ref removedTables,
            (newTable, oldTable) =>
            {
                TableDiff tableDiff = CreateTableDiff<TTableDiff>(newTable, oldTable);
                changedTables.Add(tableDiff);
            });

        dbDiff.AddedTables = addedTables;
        dbDiff.RemovedTables = removedTables;
        dbDiff.ChangedTables = changedTables;
    }

    protected void BuildViewsDiff(DatabaseDiff dbDiff)
    {
        FillAddedAndRemovedItemsAndAddChangedToBoth(
            dbDiff.NewDatabase.Views,
            dbDiff.OldDatabase.Views,
            out List<View> viewsToCreate,
            out List<View> viewsToDrop);

        dbDiff.ViewsToCreate = viewsToCreate;
        dbDiff.ViewsToDrop = viewsToDrop;
    }

    protected void BuildScriptsDiff(DatabaseDiff dbDiff)
    {
        FillAddedAndRemovedItemsAndAddChangedToBoth(
            dbDiff.NewDatabase.Scripts,
            dbDiff.OldDatabase.Scripts,
            out List<Script> addedScripts,
            out List<Script> removedScripts);

        dbDiff.AddedScripts = addedScripts;
        dbDiff.RemovedScripts = removedScripts;
    }

    private TTableDiff CreateTableDiff<TTableDiff>(Table newDbTable, Table oldDbTable)
        where TTableDiff : TableDiff, new()
    {
        TTableDiff tableDiff = new()
        {
            NewTable = newDbTable,
            OldTable = oldDbTable,
        };

        BuildColumnsDiff(tableDiff, newDbTable, oldDbTable);
        BuildPrimaryKeyDiff(tableDiff, newDbTable, oldDbTable);
        BuildUniqueConstraintsDiff(tableDiff, newDbTable, oldDbTable);
        BuildCheckConstraintsDiff(tableDiff, newDbTable, oldDbTable);
        BuildForeignKeysDiff(tableDiff, newDbTable, oldDbTable);
        return tableDiff;
    }

    private void BuildColumnsDiff<TTableDiff>(TTableDiff tableDiff, Table newDbTable, Table oldDbTable)
        where TTableDiff : TableDiff, new()
    {
        List<Column> addedColumns = null;
        List<Column> removedColumns = null;
        List<ColumnDiff> changedColumns = new();
        FillAddedAndRemovedItemsAndApplyActionToChangedItems(
            newDbTable.Columns, oldDbTable.Columns,
            ref addedColumns, ref removedColumns,
            (newColumn, oldColumn) =>
            {
                ColumnDiff columnDiff = new()
                {
                    NewColumn = newColumn,
                    OldColumn = oldColumn,
                };
                if (!AreEqual(columnDiff.NewColumn.DataType, columnDiff.OldColumn.DataType))
                    columnDiff.DataTypeToSet = columnDiff.NewColumn.DataType;
                if (!AreEqual(columnDiff.NewColumn.Default, columnDiff.OldColumn.Default))
                {
                    if (columnDiff.NewColumn.Default.Code != null)
                        columnDiff.DefaultToSet = columnDiff.NewColumn.Default;
                    if (columnDiff.OldColumn.Default.Code != null)
                        columnDiff.DefaultToDrop = columnDiff.OldColumn.Default;
                }
                changedColumns.Add(columnDiff);
            });

        tableDiff.ColumnsToAdd = addedColumns;
        tableDiff.ColumnsToDrop = removedColumns;
        tableDiff.ColumnsToAlter = changedColumns;
    }

    private void BuildPrimaryKeyDiff<TTableDiff>(TTableDiff tableDiff, Table newDbTable, Table oldDbTable)
        where TTableDiff : TableDiff, new()
    {
        PrimaryKey primaryKeyToCreate = null;
        PrimaryKey primaryKeyToDrop = null;
        if (!_comparer.Equals(newDbTable.PrimaryKey, oldDbTable.PrimaryKey))
        {
            primaryKeyToCreate = newDbTable.PrimaryKey;
            primaryKeyToDrop = oldDbTable.PrimaryKey;
        }

        tableDiff.PrimaryKeyToCreate = primaryKeyToCreate;
        tableDiff.PrimaryKeyToDrop = primaryKeyToDrop;
    }

    private void BuildUniqueConstraintsDiff<TTableDiff>(TTableDiff tableDiff, Table newDbTable, Table oldDbTable)
        where TTableDiff : TableDiff, new()
    {
        FillAddedAndRemovedItemsAndAddChangedToBoth(
            newDbTable.UniqueConstraints,
            oldDbTable.UniqueConstraints,
            out List<UniqueConstraint> uniqueConstraintsToCreate,
            out List<UniqueConstraint> uniqueConstraintsToDrop);

        tableDiff.UniqueConstraintsToCreate = uniqueConstraintsToCreate;
        tableDiff.UniqueConstraintsToDrop = uniqueConstraintsToDrop;
    }

    private void BuildCheckConstraintsDiff<TTableDiff>(TTableDiff tableDiff, Table newDbTable, Table oldDbTable)
        where TTableDiff : TableDiff, new()
    {
        FillAddedAndRemovedItemsAndAddChangedToBoth(
            newDbTable.CheckConstraints,
            oldDbTable.CheckConstraints,
            out List<CheckConstraint> checkConstraintsToCreate,
            out List<CheckConstraint> checkConstraintsToDrop);

        tableDiff.CheckConstraintsToCreate = checkConstraintsToCreate;
        tableDiff.CheckConstraintsToDrop = checkConstraintsToDrop;
    }

    protected void BuildIndexesDiff(DatabaseDiff dbDiff)
    {
        foreach (Table table in dbDiff.AddedTables)
            dbDiff.IndexesToCreate.AddRange(table.Indexes);
        foreach (Table table in dbDiff.RemovedTables)
            dbDiff.IndexesToDrop.AddRange(table.Indexes);

        Dictionary<Guid, Table> oldDbTableIdToTableMap = dbDiff.OldDatabase.Tables.ToDictionary(x => x.ID, x => x);
        foreach (Table table in dbDiff.NewDatabase.Tables.Except(dbDiff.AddedTables))
        {
            List<Index> indexesToCreate = null;
            List<Index> indexesToDrop = null;
            FillAddedAndRemovedItemsAndApplyActionToChangedItems(
                table.Indexes, oldDbTableIdToTableMap[table.ID].Indexes,
                ref indexesToCreate, ref indexesToDrop,
                (newIndex, oldIndex) =>
                {
                    indexesToCreate.Add(newIndex);
                    indexesToDrop.Add(oldIndex);
                });

            dbDiff.IndexesToCreate.AddRange(indexesToCreate);
            dbDiff.IndexesToDrop.AddRange(indexesToDrop);
        }
    }

    protected void BuildTriggersDiff(DatabaseDiff dbDiff)
    {
        foreach (Table table in dbDiff.AddedTables)
            dbDiff.TriggersToCreate.AddRange(table.Triggers);
        foreach (Table table in dbDiff.RemovedTables)
            dbDiff.TriggersToDrop.AddRange(table.Triggers);

        Dictionary<Guid, Table> oldDbTableIdToTableMap = dbDiff.OldDatabase.Tables.ToDictionary(x => x.ID, x => x);
        foreach (Table table in dbDiff.NewDatabase.Tables.Except(dbDiff.AddedTables))
        {
            FillAddedAndRemovedItemsAndAddChangedToBoth(
                table.Triggers,
                oldDbTableIdToTableMap[table.ID].Triggers,
                out List<Trigger> triggersToCreate,
                out List<Trigger> triggersToDrop);

            dbDiff.TriggersToCreate.AddRange(triggersToCreate);
            dbDiff.TriggersToDrop.AddRange(triggersToDrop);
        }
    }

    private void BuildForeignKeysDiff<TTableDiff>(TTableDiff tableDiff, Table newDbTable, Table oldDbTable)
        where TTableDiff : TableDiff, new()
    {
        FillAddedAndRemovedItemsAndAddChangedToBoth(
            newDbTable.ForeignKeys,
            oldDbTable.ForeignKeys,
            out List<ForeignKey> foreignKeysToCreate,
            out List<ForeignKey> foreignKeysToDrop);

        tableDiff.ForeignKeysToCreate = foreignKeysToCreate;
        tableDiff.ForeignKeysToDrop = foreignKeysToDrop;
    }

    protected void FillAddedAndRemovedItemsAndAddChangedToBoth<TCollection, TItem>(
        TCollection newCollection,
        TCollection oldCollection,
        out List<TItem> addedItems,
        out List<TItem> removedItems)
        where TCollection : IEnumerable<TItem>
        where TItem : DbObject
    {
        List<TItem> addedItemsRes = null;
        List<TItem> removedItemsRes = null;
        FillAddedAndRemovedItemsAndApplyActionToChangedItems(
            newCollection,
            oldCollection,
            ref addedItemsRes,
            ref removedItemsRes,
            (newItem, oldItem) =>
            {
                addedItemsRes.Add(newItem);
                removedItemsRes.Add(oldItem);
            });
        addedItems = addedItemsRes;
        removedItems = removedItemsRes;
    }

    protected void FillAddedAndRemovedItemsAndApplyActionToChangedItems<TCollection, TItem>(
        TCollection newCollection,
        TCollection oldCollection,
        ref List<TItem> addedItems,
        ref List<TItem> removedItems,
        Action<TItem, TItem> changedItemFoundAction)
        where TCollection : IEnumerable<TItem>
        where TItem : DbObject
    {
        HashSet<Guid> newCollectionItemIDs = new(newCollection.Select(x => x.ID));
        Dictionary<Guid, TItem> oldCollectionItemIDToItemMap = oldCollection.ToDictionary(x => x.ID, x => x);

        addedItems = newCollection
            .Where(newCollectionItem => !oldCollectionItemIDToItemMap.ContainsKey(newCollectionItem.ID))
            .ToList();
        removedItems = oldCollection
            .Where(oldCollectionItem => !newCollectionItemIDs.Contains(oldCollectionItem.ID))
            .ToList();

        foreach (TItem newCollectionItem in newCollection)
        {
            bool itemExistsInBoth = oldCollectionItemIDToItemMap.TryGetValue(newCollectionItem.ID, out TItem oldCollectionItem);
            if (!itemExistsInBoth)
            {
                OnAddedItemProcessed(newCollectionItem);
                continue;
            }

            if (!AreEqual(newCollectionItem, oldCollectionItem))
            {
                changedItemFoundAction(newCollectionItem, oldCollectionItem);
                OnChangedItemProcessed(newCollectionItem, oldCollectionItem);
            }
            else
            {
                OnUnchangedItemProcessed(newCollectionItem);
            }
        }
    }
    protected virtual void OnAddedItemProcessed<TItem>(TItem item) where TItem : DbObject { }
    protected virtual void OnChangedItemProcessed<TItem>(TItem newItem, TItem oldItem) where TItem : DbObject { }
    protected virtual void OnUnchangedItemProcessed<TItem>(TItem item) where TItem : DbObject { }

    protected bool AreEqual(object x, object y)
    {
        return _comparer.Equals(x, y);
    }
}
