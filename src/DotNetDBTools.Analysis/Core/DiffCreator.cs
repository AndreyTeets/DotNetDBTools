using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.Core;

internal abstract class DiffCreator
{
    protected readonly DbObjectsEqualityComparer DbObjectsEqualityComparer = new();

    public abstract DatabaseDiff CreateDatabaseDiff(Database newDatabase, Database oldDatabase);

    protected void BuildTablesDiff<TTableDiff>(DatabaseDiff databaseDiff, Database newDatabase, Database oldDatabase)
        where TTableDiff : TableDiff, new()
    {
        List<Table> addedTables = null;
        List<Table> removedTables = null;
        List<TableDiff> changedTables = new();
        FillAddedAndRemovedItemsAndApplyActionToChangedItems(
            newDatabase.Tables, oldDatabase.Tables,
            ref addedTables, ref removedTables,
            (newTable, oldTable) =>
            {
                TableDiff tableDiff = CreateTableDiff<TTableDiff>(newTable, oldTable);
                changedTables.Add(tableDiff);
            });

        databaseDiff.AddedTables = addedTables;
        databaseDiff.RemovedTables = removedTables;
        databaseDiff.ChangedTables = changedTables;
    }

    protected void BuildViewsDiff(DatabaseDiff dbDiff)
    {
        List<View> viewsToCreate = null;
        List<View> viewsToDrop = null;
        FillAddedAndRemovedItemsAndApplyActionToChangedItems(
            dbDiff.NewDatabase.Views, dbDiff.OldDatabase.Views,
            ref viewsToCreate, ref viewsToDrop,
            (newView, oldView) =>
            {
                viewsToCreate.Add(newView);
                viewsToDrop.Add(oldView);
            });

        dbDiff.ViewsToCreate = viewsToCreate;
        dbDiff.ViewsToDrop = viewsToDrop;
    }

    private TTableDiff CreateTableDiff<TTableDiff>(Table newDbTable, Table oldDbTable)
        where TTableDiff : TableDiff, new()
    {
        TTableDiff tableDiff = new()
        {
            NewTable = newDbTable,
            OldTable = oldDbTable,
            CheckConstraintsToCreate = new List<CheckConstraint>(),
            CheckConstraintsToDrop = new List<CheckConstraint>(),
            IndexesToCreate = new List<Index>(),
            IndexesToDrop = new List<Index>(),
            TriggersToCreate = new List<Trigger>(),
            TriggersToDrop = new List<Trigger>(),
        };

        BuildColumnsDiff(tableDiff, newDbTable, oldDbTable);
        BuildPrimaryKeyDiff(tableDiff, newDbTable, oldDbTable);
        BuildUniqueConstraintsDiff(tableDiff, newDbTable, oldDbTable);
        BuildCheckConstraintsDiff(tableDiff, newDbTable, oldDbTable);
        BuildIndexesDiff(tableDiff);
        BuildTriggersDiff(tableDiff);
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
                changedColumns.Add(columnDiff);
            });

        tableDiff.AddedColumns = addedColumns;
        tableDiff.RemovedColumns = removedColumns;
        tableDiff.ChangedColumns = changedColumns;
    }

    private void BuildPrimaryKeyDiff<TTableDiff>(TTableDiff tableDiff, Table newDbTable, Table oldDbTable)
        where TTableDiff : TableDiff, new()
    {
        PrimaryKey primaryKeyToCreate = null;
        PrimaryKey primaryKeyToDrop = null;
        if (!DbObjectsEqualityComparer.Equals(newDbTable.PrimaryKey, oldDbTable.PrimaryKey))
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
        List<UniqueConstraint> uniqueConstraintsToCreate = null;
        List<UniqueConstraint> uniqueConstraintsToDrop = null;
        FillAddedAndRemovedItemsAndApplyActionToChangedItems(
            newDbTable.UniqueConstraints, oldDbTable.UniqueConstraints,
            ref uniqueConstraintsToCreate, ref uniqueConstraintsToDrop,
            (newUC, oldUC) =>
            {
                uniqueConstraintsToCreate.Add(newUC);
                uniqueConstraintsToDrop.Add(oldUC);
            });

        tableDiff.UniqueConstraintsToCreate = uniqueConstraintsToCreate;
        tableDiff.UniqueConstraintsToDrop = uniqueConstraintsToDrop;
    }

    private void BuildCheckConstraintsDiff<TTableDiff>(TTableDiff tableDiff, Table newDbTable, Table oldDbTable)
        where TTableDiff : TableDiff, new()
    {
        List<CheckConstraint> checkConstraintsToCreate = null;
        List<CheckConstraint> checkConstraintsToDrop = null;
        FillAddedAndRemovedItemsAndApplyActionToChangedItems(
            newDbTable.CheckConstraints, oldDbTable.CheckConstraints,
            ref checkConstraintsToCreate, ref checkConstraintsToDrop,
            (newCK, oldCK) =>
            {
                checkConstraintsToCreate.Add(newCK);
                checkConstraintsToDrop.Add(oldCK);
            });

        tableDiff.CheckConstraintsToCreate = checkConstraintsToCreate;
        tableDiff.CheckConstraintsToDrop = checkConstraintsToDrop;
    }

    private void BuildIndexesDiff<TTableDiff>(TTableDiff tableDiff)
        where TTableDiff : TableDiff, new()
    {
        List<Index> indexesToCreate = null;
        List<Index> indexesToDrop = null;
        FillAddedAndRemovedItemsAndApplyActionToChangedItems(
            tableDiff.NewTable.Indexes, tableDiff.OldTable.Indexes,
            ref indexesToCreate, ref indexesToDrop,
            (newIndex, oldIndex) =>
            {
                indexesToCreate.Add(newIndex);
                indexesToDrop.Add(oldIndex);
            });

        tableDiff.IndexesToCreate = indexesToCreate;
        tableDiff.IndexesToDrop = indexesToDrop;
    }

    private void BuildTriggersDiff<TTableDiff>(TTableDiff tableDiff)
        where TTableDiff : TableDiff, new()
    {
        List<Trigger> triggersToCreate = null;
        List<Trigger> triggersToDrop = null;
        FillAddedAndRemovedItemsAndApplyActionToChangedItems(
            tableDiff.NewTable.Triggers, tableDiff.OldTable.Triggers,
            ref triggersToCreate, ref triggersToDrop,
            (newTrigger, oldTrigger) =>
            {
                triggersToCreate.Add(newTrigger);
                triggersToDrop.Add(oldTrigger);
            });

        tableDiff.TriggersToCreate = triggersToCreate;
        tableDiff.TriggersToDrop = triggersToDrop;
    }

    private void BuildForeignKeysDiff<TTableDiff>(TTableDiff tableDiff, Table newDbTable, Table oldDbTable)
        where TTableDiff : TableDiff, new()
    {
        List<ForeignKey> foreignKeysToCreate = null;
        List<ForeignKey> foreignKeysToDrop = null;
        FillAddedAndRemovedItemsAndApplyActionToChangedItems(
            newDbTable.ForeignKeys, oldDbTable.ForeignKeys,
            ref foreignKeysToCreate, ref foreignKeysToDrop,
            (newFK, oldFK) =>
            {
                foreignKeysToCreate.Add(newFK);
                foreignKeysToDrop.Add(oldFK);
            });

        tableDiff.ForeignKeysToCreate = foreignKeysToCreate;
        tableDiff.ForeignKeysToDrop = foreignKeysToDrop;
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
            if (oldCollectionItemIDToItemMap.TryGetValue(newCollectionItem.ID, out TItem oldCollectionItem) &&
                (!DbObjectsEqualityComparer.Equals(newCollectionItem, oldCollectionItem) ||
                    AdditionalItemsNonEqualityConditionIsTrue(newCollectionItem, oldCollectionItem)))
            {
                changedItemFoundAction(newCollectionItem, oldCollectionItem);
            }
        }
    }

    protected virtual bool AdditionalItemsNonEqualityConditionIsTrue<TItem>(TItem newItem, TItem oldItem)
    {
        return false;
    }
}
