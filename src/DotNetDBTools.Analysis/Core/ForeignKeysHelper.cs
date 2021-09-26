using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.Core
{
    public static class ForeignKeysHelper
    {
        public static Dictionary<Guid, TableInfo> CreateFKToTableMap(IEnumerable<TableInfo> tables)
        {
            Dictionary<Guid, TableInfo> fkToTableMap = new();
            foreach (TableInfo table in tables)
            {
                foreach (ForeignKeyInfo fk in table.ForeignKeys)
                    fkToTableMap.Add(fk.ID, table);
            }
            return fkToTableMap;
        }

        public static void BuildAllForeignKeysToBeDroppedAndAdded(DatabaseDiff databaseDiff)
        {
            HashSet<ForeignKeyInfo> allAddedForeignKeys = GetAllAddedForeignKeys(databaseDiff);
            HashSet<ForeignKeyInfo> allRemovedForeignKeys = GetAllRemovedForeignKeys(databaseDiff);

            HashSet<ForeignKeyInfo> allForeignKeysToDrop = GetAllForeignKeysToDrop(databaseDiff, allRemovedForeignKeys);

            HashSet<ForeignKeyInfo> unchangedForeignKeysButReferencingChangedObjects = new(allForeignKeysToDrop);
            unchangedForeignKeysButReferencingChangedObjects.ExceptWith(allRemovedForeignKeys);

            HashSet<ForeignKeyInfo> allForeignKeysToAdd = new(allAddedForeignKeys);
            allForeignKeysToAdd.UnionWith(unchangedForeignKeysButReferencingChangedObjects);

            databaseDiff.AllForeignKeysToAdd = allForeignKeysToAdd;
            databaseDiff.AllForeignKeysToDrop = allForeignKeysToDrop;
        }

        private static HashSet<ForeignKeyInfo> GetAllAddedForeignKeys(DatabaseDiff databaseDiff)
        {
            HashSet<ForeignKeyInfo> allAddedForeignKeys = new();
            foreach (IEnumerable<ForeignKeyInfo> addedTableForeignKeys in databaseDiff.AddedTables.Select(t => t.ForeignKeys))
                allAddedForeignKeys.UnionWith(addedTableForeignKeys);
            foreach (TableDiff tableDiff in databaseDiff.ChangedTables)
                allAddedForeignKeys.UnionWith(tableDiff.AddedForeignKeys);
            return allAddedForeignKeys;
        }

        private static HashSet<ForeignKeyInfo> GetAllRemovedForeignKeys(DatabaseDiff databaseDiff)
        {
            HashSet<ForeignKeyInfo> allRemovedForeignKeys = new();
            foreach (IEnumerable<ForeignKeyInfo> removedTableForeignKeys in databaseDiff.RemovedTables.Select(t => t.ForeignKeys))
                allRemovedForeignKeys.UnionWith(removedTableForeignKeys);
            foreach (TableDiff tableDiff in databaseDiff.ChangedTables)
                allRemovedForeignKeys.UnionWith(tableDiff.RemovedForeignKeys);
            return allRemovedForeignKeys;
        }

        private static HashSet<ForeignKeyInfo> GetAllForeignKeysToDrop(
            DatabaseDiff databaseDiff,
            HashSet<ForeignKeyInfo> allRemovedForeignKeys)
        {
            HashSet<Guid> columnsChangedOrReferencedByChangedObjects = GetColumnsChangedOrReferencedByChangedObjects(databaseDiff);
            Dictionary<Guid, HashSet<ForeignKeyInfo>> colIDToReferencingFKMap = CreateColIDToReferencingFKMap(databaseDiff.OldDatabase.Tables);

            HashSet<ForeignKeyInfo> allForeignKeysToDrop = new(allRemovedForeignKeys);
            foreach (Guid columnID in columnsChangedOrReferencedByChangedObjects)
                allForeignKeysToDrop.UnionWith(colIDToReferencingFKMap[columnID]);

            return allForeignKeysToDrop;
        }

        private static HashSet<Guid> GetColumnsChangedOrReferencedByChangedObjects(DatabaseDiff databaseDiff)
        {
            HashSet<Guid> columnsChangedOrReferencedByChangedObjects = new();
            foreach (IEnumerable<ColumnInfo> removedTableColumns in databaseDiff.RemovedTables.Select(t => t.Columns))
                columnsChangedOrReferencedByChangedObjects.UnionWith(removedTableColumns.Select(c => c.ID));

            foreach (TableDiff tableDiff in databaseDiff.ChangedTables)
            {
                columnsChangedOrReferencedByChangedObjects.UnionWith(tableDiff.RemovedColumns.Select(c => c.ID));
                columnsChangedOrReferencedByChangedObjects.UnionWith(tableDiff.ChangedColumns.Select(cd => cd.OldColumn.ID));
                Dictionary<string, Guid> oldTableColumnIDs = tableDiff.OldTable.Columns.ToDictionary(c => c.Name, c => c.ID);
                if (tableDiff.RemovedPrimaryKey is not null)
                    columnsChangedOrReferencedByChangedObjects.UnionWith(tableDiff.RemovedPrimaryKey.Columns.Select(cn => oldTableColumnIDs[cn]));
                foreach (UniqueConstraintInfo uc in tableDiff.RemovedUniqueConstraints)
                    columnsChangedOrReferencedByChangedObjects.UnionWith(uc.Columns.Select(cn => oldTableColumnIDs[cn]));
            }

            return columnsChangedOrReferencedByChangedObjects;
        }

        private static Dictionary<Guid, HashSet<ForeignKeyInfo>> CreateColIDToReferencingFKMap(
            IEnumerable<TableInfo> tables)
        {
            Dictionary<string, TableInfo> tableNameToTableMap = new();
            foreach (TableInfo table in tables)
                tableNameToTableMap.Add(table.Name, table);

            Dictionary<Guid, HashSet<ForeignKeyInfo>> colIDToReferencingFKMap = new();
            foreach (TableInfo table in tables)
            {
                foreach (ForeignKeyInfo fk in table.ForeignKeys)
                {
                    Dictionary<string, Guid> tableColNameToColIDMap = tableNameToTableMap[fk.ReferencedTableName].Columns
                        .ToDictionary(c => c.Name, c => c.ID);
                    foreach (string cn in fk.ReferencedTableColumnNames)
                    {
                        Guid columnID = tableColNameToColIDMap[cn];
                        if (colIDToReferencingFKMap.ContainsKey(columnID))
                            colIDToReferencingFKMap[columnID].Add(fk);
                        else
                            colIDToReferencingFKMap.Add(columnID, new HashSet<ForeignKeyInfo>() { fk });
                    }
                }
            }

            foreach (TableInfo table in tables)
            {
                foreach (ColumnInfo column in table.Columns)
                {
                    if (!colIDToReferencingFKMap.ContainsKey(column.ID))
                        colIDToReferencingFKMap.Add(column.ID, new HashSet<ForeignKeyInfo>());
                }
            }
            return colIDToReferencingFKMap;
        }
    }
}
