using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.Common;

internal static class IndexesHelper
{
    public static void BuildAllDbIndexesToBeDroppedAndCreated(DatabaseDiff dbDiff)
    {
        dbDiff.IndexesToCreate = new List<Index>(GetIndexesToCreate(dbDiff));
        dbDiff.IndexesToDrop = new List<Index>(GetIndexesToDrop(dbDiff));
    }

    private static HashSet<Index> GetIndexesToCreate(DatabaseDiff dbDiff)
    {
        HashSet<Index> indexesToCreate = new();
        foreach (IEnumerable<Index> addedTableIndexes in dbDiff.AddedTables.Select(t => t.Indexes))
            indexesToCreate.UnionWith(addedTableIndexes);
        foreach (TableDiff tableDiff in dbDiff.ChangedTables)
            indexesToCreate.UnionWith(tableDiff.IndexesToCreate);
        return indexesToCreate;
    }

    private static HashSet<Index> GetIndexesToDrop(DatabaseDiff dbDiff)
    {
        HashSet<Index> indexesToDrop = new();
        foreach (IEnumerable<Index> removedTableIndexes in dbDiff.RemovedTables.Select(t => t.Indexes))
            indexesToDrop.UnionWith(removedTableIndexes);
        foreach (TableDiff tableDiff in dbDiff.ChangedTables)
            indexesToDrop.UnionWith(tableDiff.IndexesToDrop);
        return indexesToDrop;
    }
}
