using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.Common
{
    public static class ForeignKeysHelper
    {
        public static void BuildAllForeignKeysToBeDroppedAndCreated(DatabaseDiff databaseDiff)
        {
            HashSet<ForeignKey> allAddedForeignKeys = GetAllAddedForeignKeys(databaseDiff);
            HashSet<ForeignKey> allRemovedForeignKeys = GetAllRemovedForeignKeys(databaseDiff);

            HashSet<ForeignKey> allForeignKeysToDrop = GetAllForeignKeysToDrop(databaseDiff, allRemovedForeignKeys);

            HashSet<ForeignKey> unchangedForeignKeysButReferencingChangedObjects = new(allForeignKeysToDrop);
            unchangedForeignKeysButReferencingChangedObjects.ExceptWith(allRemovedForeignKeys);

            HashSet<ForeignKey> allForeignKeysToCreate = new(allAddedForeignKeys);
            allForeignKeysToCreate.UnionWith(unchangedForeignKeysButReferencingChangedObjects);

            databaseDiff.AllForeignKeysToCreate = allForeignKeysToCreate;
            databaseDiff.AllForeignKeysToDrop = allForeignKeysToDrop;
        }

        private static HashSet<ForeignKey> GetAllAddedForeignKeys(DatabaseDiff databaseDiff)
        {
            HashSet<ForeignKey> allAddedForeignKeys = new();
            foreach (IEnumerable<ForeignKey> addedTableForeignKeys in databaseDiff.AddedTables.Select(t => t.ForeignKeys))
                allAddedForeignKeys.UnionWith(addedTableForeignKeys);
            foreach (TableDiff tableDiff in databaseDiff.ChangedTables)
                allAddedForeignKeys.UnionWith(tableDiff.ForeignKeysToCreate);
            return allAddedForeignKeys;
        }

        private static HashSet<ForeignKey> GetAllRemovedForeignKeys(DatabaseDiff databaseDiff)
        {
            HashSet<ForeignKey> allRemovedForeignKeys = new();
            foreach (IEnumerable<ForeignKey> removedTableForeignKeys in databaseDiff.RemovedTables.Select(t => t.ForeignKeys))
                allRemovedForeignKeys.UnionWith(removedTableForeignKeys);
            foreach (TableDiff tableDiff in databaseDiff.ChangedTables)
                allRemovedForeignKeys.UnionWith(tableDiff.ForeignKeysToDrop);
            return allRemovedForeignKeys;
        }

        private static HashSet<ForeignKey> GetAllForeignKeysToDrop(
            DatabaseDiff databaseDiff,
            HashSet<ForeignKey> allRemovedForeignKeys)
        {
            HashSet<Guid> columnsChangedOrReferencedByChangedObjects = GetColumnsChangedOrReferencedByChangedObjects(databaseDiff);
            Dictionary<Guid, HashSet<ForeignKey>> colIDToReferencingFKMap = CreateColIDToReferencingFKMap(databaseDiff.OldDatabase.Tables);

            HashSet<ForeignKey> allForeignKeysToDrop = new(allRemovedForeignKeys);
            foreach (Guid columnID in columnsChangedOrReferencedByChangedObjects)
                allForeignKeysToDrop.UnionWith(colIDToReferencingFKMap[columnID]);

            return allForeignKeysToDrop;
        }

        private static HashSet<Guid> GetColumnsChangedOrReferencedByChangedObjects(DatabaseDiff databaseDiff)
        {
            HashSet<Guid> columnsChangedOrReferencedByChangedObjects = new();
            foreach (IEnumerable<Column> removedTableColumns in databaseDiff.RemovedTables.Select(t => t.Columns))
                columnsChangedOrReferencedByChangedObjects.UnionWith(removedTableColumns.Select(c => c.ID));

            foreach (TableDiff tableDiff in databaseDiff.ChangedTables)
            {
                columnsChangedOrReferencedByChangedObjects.UnionWith(tableDiff.RemovedColumns.Select(c => c.ID));
                columnsChangedOrReferencedByChangedObjects.UnionWith(tableDiff.ChangedColumns.Select(cd => cd.OldColumn.ID));
                Dictionary<string, Guid> oldTableColumnIDs = tableDiff.OldTable.Columns.ToDictionary(c => c.Name, c => c.ID);
                if (tableDiff.PrimaryKeyToDrop is not null)
                    columnsChangedOrReferencedByChangedObjects.UnionWith(tableDiff.PrimaryKeyToDrop.Columns.Select(cn => oldTableColumnIDs[cn]));
                foreach (UniqueConstraint uc in tableDiff.UniqueConstraintsToDrop)
                    columnsChangedOrReferencedByChangedObjects.UnionWith(uc.Columns.Select(cn => oldTableColumnIDs[cn]));
            }

            return columnsChangedOrReferencedByChangedObjects;
        }

        private static Dictionary<Guid, HashSet<ForeignKey>> CreateColIDToReferencingFKMap(
            IEnumerable<Table> tables)
        {
            Dictionary<string, Table> tableNameToTableMap = new();
            foreach (Table table in tables)
                tableNameToTableMap.Add(table.Name, table);

            Dictionary<Guid, HashSet<ForeignKey>> colIDToReferencingFKMap = new();
            foreach (Table table in tables)
            {
                foreach (ForeignKey fk in table.ForeignKeys)
                {
                    Dictionary<string, Guid> tableColNameToColIDMap = tableNameToTableMap[fk.ReferencedTableName].Columns
                        .ToDictionary(c => c.Name, c => c.ID);
                    foreach (string cn in fk.ReferencedTableColumnNames)
                    {
                        Guid columnID = tableColNameToColIDMap[cn];
                        if (colIDToReferencingFKMap.ContainsKey(columnID))
                            colIDToReferencingFKMap[columnID].Add(fk);
                        else
                            colIDToReferencingFKMap.Add(columnID, new HashSet<ForeignKey>() { fk });
                    }
                }
            }

            foreach (Table table in tables)
            {
                foreach (Column column in table.Columns)
                {
                    if (!colIDToReferencingFKMap.ContainsKey(column.ID))
                        colIDToReferencingFKMap.Add(column.ID, new HashSet<ForeignKey>());
                }
            }
            return colIDToReferencingFKMap;
        }
    }
}
