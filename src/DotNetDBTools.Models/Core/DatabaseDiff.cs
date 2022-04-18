using System.Collections.Generic;

namespace DotNetDBTools.Models.Core;

public abstract class DatabaseDiff
{
    public Database NewDatabase { get; set; }
    public Database OldDatabase { get; set; }

    public List<Table> AddedTables { get; set; }
    public List<Table> RemovedTables { get; set; }
    public List<TableDiff> ChangedTables { get; set; }

    public List<View> ViewsToCreate { get; set; }
    public List<View> ViewsToDrop { get; set; }

    public List<Index> IndexesToCreate { get; set; }
    public List<Index> IndexesToDrop { get; set; }

    public List<Trigger> TriggersToCreate { get; set; }
    public List<Trigger> TriggersToDrop { get; set; }

    public List<ForeignKey> AllForeignKeysToCreate { get; set; }
    public List<ForeignKey> AllForeignKeysToDrop { get; set; }

    public List<Script> AddedScripts { get; set; }
    public List<Script> RemovedScripts { get; set; }
}
