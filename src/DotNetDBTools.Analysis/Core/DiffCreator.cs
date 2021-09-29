using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.Core
{
    internal abstract class DiffCreator
    {
        protected readonly DbObjectsEqualityComparer DbObjectsEqualityComparer = new();

        public abstract DatabaseDiff CreateDatabaseDiff(DatabaseInfo newDatabase, DatabaseInfo oldDatabase);

        protected void BuildTablesDiff<TTableDiff>(DatabaseDiff databaseDiff, DatabaseInfo newDatabase, DatabaseInfo oldDatabase)
            where TTableDiff : TableDiff, new()
        {
            List<TableInfo> addedTables = null;
            List<TableInfo> removedTables = null;
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

        private TTableDiff CreateTableDiff<TTableDiff>(TableInfo newDbTable, TableInfo oldDbTable)
            where TTableDiff : TableDiff, new()
        {
            TTableDiff tableDiff = new()
            {
                NewTable = newDbTable,
                OldTable = oldDbTable,
                CheckConstraintsToCreate = new List<CheckConstraintInfo>(),
                CheckConstraintsToDrop = new List<CheckConstraintInfo>(),
                IndexesToCreate = new List<IndexInfo>(),
                IndexesToDrop = new List<IndexInfo>(),
                TriggersToCreate = new List<TriggerInfo>(),
                TriggersToDrop = new List<TriggerInfo>(),
            };

            BuildColumnsDiff(tableDiff, newDbTable, oldDbTable);
            BuildPrimaryKeyDiff(tableDiff, newDbTable, oldDbTable);
            BuildUniqueConstraintsDiff(tableDiff, newDbTable, oldDbTable);
            BuildForeignKeysDiff(tableDiff, newDbTable, oldDbTable);
            return tableDiff;
        }

        private void BuildColumnsDiff<TTableDiff>(TTableDiff tableDiff, TableInfo newDbTable, TableInfo oldDbTable)
            where TTableDiff : TableDiff, new()
        {
            List<ColumnInfo> addedColumns = null;
            List<ColumnInfo> removedColumns = null;
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

        private void BuildPrimaryKeyDiff<TTableDiff>(TTableDiff tableDiff, TableInfo newDbTable, TableInfo oldDbTable)
            where TTableDiff : TableDiff, new()
        {
            PrimaryKeyInfo primaryKeyToCreate = null;
            PrimaryKeyInfo primaryKeyToDrop = null;
            if (!DbObjectsEqualityComparer.Equals(newDbTable.PrimaryKey, oldDbTable.PrimaryKey))
            {
                primaryKeyToCreate = newDbTable.PrimaryKey;
                primaryKeyToDrop = oldDbTable.PrimaryKey;
            }

            tableDiff.PrimaryKeyToCreate = primaryKeyToCreate;
            tableDiff.PrimaryKeyToDrop = primaryKeyToDrop;
        }

        private void BuildUniqueConstraintsDiff<TTableDiff>(TTableDiff tableDiff, TableInfo newDbTable, TableInfo oldDbTable)
            where TTableDiff : TableDiff, new()
        {
            List<UniqueConstraintInfo> uniqueConstraintsToCreate = null;
            List<UniqueConstraintInfo> uniqueConstraintsToDrop = null;
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

        private void BuildForeignKeysDiff<TTableDiff>(TTableDiff tableDiff, TableInfo newDbTable, TableInfo oldDbTable)
            where TTableDiff : TableDiff, new()
        {
            List<ForeignKeyInfo> foreignKeysToCreate = null;
            List<ForeignKeyInfo> foreignKeysToDrop = null;
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
            where TItem : DBObjectInfo
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
                    !DbObjectsEqualityComparer.Equals(newCollectionItem, oldCollectionItem))
                {
                    changedItemFoundAction(newCollectionItem, oldCollectionItem);
                }
            }
        }
    }
}
