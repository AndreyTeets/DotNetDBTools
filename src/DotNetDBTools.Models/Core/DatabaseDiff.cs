using System.Collections.Generic;

namespace DotNetDBTools.Models.Core;

public abstract class DatabaseDiff
{
    public Database NewDatabase { get; set; }
    public Database OldDatabase { get; set; }

    public IEnumerable<Table> AddedTables { get; set; }
    public IEnumerable<Table> RemovedTables { get; set; }
    public IEnumerable<TableDiff> ChangedTables { get; set; }

    public IEnumerable<View> ViewsToCreate { get; set; }
    public IEnumerable<View> ViewsToDrop { get; set; }

    public IEnumerable<Index> IndexesToCreate { get; set; }
    public IEnumerable<Index> IndexesToDrop { get; set; }

    public IEnumerable<Trigger> TriggersToCreate { get; set; }
    public IEnumerable<Trigger> TriggersToDrop { get; set; }

    public IEnumerable<ForeignKey> AllForeignKeysToCreate { get; set; }
    public IEnumerable<ForeignKey> AllForeignKeysToDrop { get; set; }

    public IEnumerable<Script> AddedScripts { get; set; }
    public IEnumerable<Script> RemovedScripts { get; set; }
}
