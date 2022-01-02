using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.Common
{
    public static class TriggersHelper
    {
        public static void BuildAllDbTriggersToBeDroppedAndCreated(DatabaseDiff dbDiff)
        {
            dbDiff.TriggersToCreate = new List<Trigger>(GetTriggersToCreate(dbDiff));
            dbDiff.TriggersToDrop = new List<Trigger>(GetTriggersToDrop(dbDiff));
        }

        private static HashSet<Trigger> GetTriggersToCreate(DatabaseDiff dbDiff)
        {
            HashSet<Trigger> triggersToCreate = new();
            foreach (IEnumerable<Trigger> addedTableTriggers in dbDiff.AddedTables.Select(t => t.Triggers))
                triggersToCreate.UnionWith(addedTableTriggers);
            foreach (TableDiff tableDiff in dbDiff.ChangedTables)
                triggersToCreate.UnionWith(tableDiff.TriggersToCreate);
            return triggersToCreate;
        }

        private static HashSet<Trigger> GetTriggersToDrop(DatabaseDiff dbDiff)
        {
            HashSet<Trigger> triggersToDrop = new();
            foreach (IEnumerable<Trigger> removedTableTriggers in dbDiff.RemovedTables.Select(t => t.Triggers))
                triggersToDrop.UnionWith(removedTableTriggers);
            foreach (TableDiff tableDiff in dbDiff.ChangedTables)
                triggersToDrop.UnionWith(tableDiff.TriggersToDrop);
            return triggersToDrop;
        }
    }
}
