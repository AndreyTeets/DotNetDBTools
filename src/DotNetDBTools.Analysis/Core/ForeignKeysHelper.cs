using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.Core
{
    public static class ForeignKeysHelper
    {
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
            Dictionary<Guid, HashSet<ForeignKeyInfo>> columnsReferencedByMap = CreateColumnsReferencedByMap(databaseDiff.OldDatabase.Tables);

            HashSet<ForeignKeyInfo> allForeignKeysToDrop = new(allRemovedForeignKeys);
            foreach (Guid columnID in columnsChangedOrReferencedByChangedObjects)
                allForeignKeysToDrop.UnionWith(columnsReferencedByMap[columnID]);
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
                columnsChangedOrReferencedByChangedObjects.UnionWith(tableDiff.RemovedPrimaryKey.Columns.Select(cn => oldTableColumnIDs[cn]));
                foreach (UniqueConstraintInfo uc in tableDiff.RemovedUniqueConstraints)
                    columnsChangedOrReferencedByChangedObjects.UnionWith(uc.Columns.Select(cn => oldTableColumnIDs[cn]));
            }

            return columnsChangedOrReferencedByChangedObjects;
        }

        private static Dictionary<Guid, HashSet<ForeignKeyInfo>> CreateColumnsReferencedByMap(
            IEnumerable<TableInfo> tables)
        {
            Dictionary<Guid, HashSet<ForeignKeyInfo>> columnsReferencedByMap = new();
            foreach (TableInfo table in tables)
            {
                Dictionary<string, Guid> tableColumnIDs = table.Columns.ToDictionary(c => c.Name, c => c.ID);
                foreach (ForeignKeyInfo fk in table.ForeignKeys)
                {
                    foreach (string cn in fk.ForeignColumnNames)
                    {
                        Guid columnID = tableColumnIDs[cn];
                        if (columnsReferencedByMap.ContainsKey(columnID))
                            columnsReferencedByMap[columnID].Add(fk);
                        else
                            columnsReferencedByMap.Add(columnID, new HashSet<ForeignKeyInfo>() { fk });
                    }
                }
            }

            foreach (TableInfo table in tables)
            {
                foreach (ColumnInfo column in table.Columns)
                {
                    if (!columnsReferencedByMap.ContainsKey(column.ID))
                        columnsReferencedByMap.Add(column.ID, new HashSet<ForeignKeyInfo>());
                }
            }
            return columnsReferencedByMap;
        }
    }
}
