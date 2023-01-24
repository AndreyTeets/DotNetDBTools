using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.Common;

internal static class ForeignKeysHelper
{
    public static void BuildUnchangedForeignKeysToRecreateBecauseOfDeps(DatabaseDiff dbDiff, Database oldDb)
    {
        HashSet<ForeignKey> changedForeignKeysToCreate = GetChangedForeignKeysToCreate(dbDiff);
        HashSet<ForeignKey> changedForeignKeysToDrop = GetChangedForeignKeysToDrop(dbDiff);

        HashSet<ForeignKey> allForeignKeysToDrop = GetAllForeignKeysToDrop(dbDiff, oldDb, changedForeignKeysToDrop);

        HashSet<ForeignKey> unchangedForeignKeysToRecreateBecauseOfDeps = new(allForeignKeysToDrop);
        unchangedForeignKeysToRecreateBecauseOfDeps.ExceptWith(changedForeignKeysToDrop);

        dbDiff.UnchangedForeignKeysToRecreateBecauseOfDeps = unchangedForeignKeysToRecreateBecauseOfDeps.ToList();
    }

    private static HashSet<ForeignKey> GetChangedForeignKeysToCreate(DatabaseDiff dbDiff)
    {
        HashSet<ForeignKey> changedForeignKeysToCreate = new();
        foreach (IEnumerable<ForeignKey> addedTableForeignKeys in dbDiff.AddedTables.Select(t => t.ForeignKeys))
            changedForeignKeysToCreate.UnionWith(addedTableForeignKeys);
        foreach (TableDiff tableDiff in dbDiff.ChangedTables)
            changedForeignKeysToCreate.UnionWith(tableDiff.ForeignKeysToCreate);
        return changedForeignKeysToCreate;
    }

    private static HashSet<ForeignKey> GetChangedForeignKeysToDrop(DatabaseDiff dbDiff)
    {
        HashSet<ForeignKey> changedForeignKeysToDrop = new();
        foreach (IEnumerable<ForeignKey> removedTableForeignKeys in dbDiff.RemovedTables.Select(t => t.ForeignKeys))
            changedForeignKeysToDrop.UnionWith(removedTableForeignKeys);
        foreach (TableDiff tableDiff in dbDiff.ChangedTables)
            changedForeignKeysToDrop.UnionWith(tableDiff.ForeignKeysToDrop);
        return changedForeignKeysToDrop;
    }

    private static HashSet<ForeignKey> GetAllForeignKeysToDrop(
        DatabaseDiff dbDiff,
        Database oldDb,
        HashSet<ForeignKey> foreignKeysToDrop)
    {
        HashSet<Guid> columnsChangedOrReferencedByChangedObjects = GetColumnsChangedOrReferencedByChangedObjects(dbDiff, oldDb);
        Dictionary<Guid, HashSet<ForeignKey>> colIDToReferencingFKMap = CreateColIDToReferencingFKMap(oldDb.Tables);

        HashSet<ForeignKey> allForeignKeysToDrop = new(foreignKeysToDrop);
        foreach (Guid columnID in columnsChangedOrReferencedByChangedObjects)
            allForeignKeysToDrop.UnionWith(colIDToReferencingFKMap[columnID]);
        return allForeignKeysToDrop;
    }

    private static HashSet<Guid> GetColumnsChangedOrReferencedByChangedObjects(DatabaseDiff dbDiff, Database oldDb)
    {
        HashSet<Guid> columnsChangedOrReferencedByChangedObjects = new();
        foreach (IEnumerable<Column> removedTableColumns in dbDiff.RemovedTables.Select(t => t.Columns))
            columnsChangedOrReferencedByChangedObjects.UnionWith(removedTableColumns.Select(c => c.ID));

        Dictionary<Guid, Table> oldDbTables = oldDb.Tables.ToDictionary(x => x.ID, x => x);
        foreach (TableDiff tableDiff in dbDiff.ChangedTables)
        {
            columnsChangedOrReferencedByChangedObjects.UnionWith(tableDiff.ColumnsToDrop.Select(c => c.ID));
            columnsChangedOrReferencedByChangedObjects.UnionWith(tableDiff.ColumnsToAlter.Select(cd => cd.ColumnID));
            Dictionary<string, Guid> oldTableColumnIDs = oldDbTables[tableDiff.TableID].Columns.ToDictionary(c => c.Name, c => c.ID);
            if (tableDiff.PrimaryKeyToDrop is not null)
                columnsChangedOrReferencedByChangedObjects.UnionWith(tableDiff.PrimaryKeyToDrop.Columns.Select(cn => oldTableColumnIDs[cn]));
            foreach (UniqueConstraint uc in tableDiff.UniqueConstraintsToDrop)
                columnsChangedOrReferencedByChangedObjects.UnionWith(uc.Columns.Select(cn => oldTableColumnIDs[cn]));
        }

        return columnsChangedOrReferencedByChangedObjects;
    }

    private static Dictionary<Guid, HashSet<ForeignKey>> CreateColIDToReferencingFKMap(IEnumerable<Table> tables)
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
