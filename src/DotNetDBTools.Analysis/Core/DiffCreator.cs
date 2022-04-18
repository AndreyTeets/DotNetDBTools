using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.Core;

internal abstract class DiffCreator
{
    private readonly DNDBTModelsEqualityComparer _dndbtModelsEqualityComparer = new();

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

    protected void BuildScriptsDiff(DatabaseDiff dbDiff)
    {
        List<Script> addedScripts = null;
        List<Script> removedScripts = null;
        FillAddedAndRemovedItemsAndApplyActionToChangedItems(
            dbDiff.NewDatabase.Scripts, dbDiff.OldDatabase.Scripts,
            ref addedScripts, ref removedScripts,
            (newView, oldView) => { });

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
                SetDataTypeChanged(columnDiff);
                changedColumns.Add(columnDiff);
            });

        tableDiff.AddedColumns = addedColumns;
        tableDiff.RemovedColumns = removedColumns;
        tableDiff.ChangedColumns = changedColumns;
    }
    protected virtual void SetDataTypeChanged(ColumnDiff columnDiff)
    {
        if (columnDiff.NewColumn.DataType.Name != columnDiff.OldColumn.DataType.Name)
            columnDiff.DataTypeChanged = true;
    }

    private void BuildPrimaryKeyDiff<TTableDiff>(TTableDiff tableDiff, Table newDbTable, Table oldDbTable)
        where TTableDiff : TableDiff, new()
    {
        PrimaryKey primaryKeyToCreate = null;
        PrimaryKey primaryKeyToDrop = null;
        if (!_dndbtModelsEqualityComparer.Equals(newDbTable.PrimaryKey, oldDbTable.PrimaryKey))
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
                (!_dndbtModelsEqualityComparer.Equals(newCollectionItem, oldCollectionItem) ||
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
