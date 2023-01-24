using System.Collections.Generic;

namespace DotNetDBTools.Models.Core;

public abstract class DatabaseDiff
{
    public long NewVersion { get; set; }
    public long OldVersion { get; set; }

    public List<Table> AddedTables { get; set; } = new();
    public List<Table> RemovedTables { get; set; } = new();
    public List<TableDiff> ChangedTables { get; set; } = new();

    public List<View> ViewsToCreate { get; set; } = new();
    public List<View> ViewsToDrop { get; set; } = new();

    public List<Index> IndexesToCreate { get; set; } = new();
    public List<Index> IndexesToDrop { get; set; } = new();

    public List<Trigger> TriggersToCreate { get; set; } = new();
    public List<Trigger> TriggersToDrop { get; set; } = new();

    public List<ForeignKey> UnchangedForeignKeysToRecreateBecauseOfDeps { get; set; } = new();

    public List<Script> AddedScripts { get; set; } = new();
    public List<Script> RemovedScripts { get; set; } = new();
}
