using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Analysis.SQLite;

internal class SQLiteDiffCreator : DiffCreator
{
    private readonly HashSet<Guid> _addedObjects = new();
    private readonly HashSet<Guid> _changedObjects = new();
    private readonly HashSet<Guid> _objectsThatRequireRedefinition = new();

    public override DatabaseDiff CreateDatabaseDiff(Database newDatabase, Database oldDatabase)
    {
        _addedObjects.Clear();
        _changedObjects.Clear();
        _objectsThatRequireRedefinition.Clear();

        SQLiteDatabaseDiff dbDiff = new()
        {
            NewVersion = newDatabase.Version,
            OldVersion = oldDatabase.Version,
        };

        BuildTablesDiff<SQLiteTableDiff, ColumnDiff>(dbDiff, newDatabase, oldDatabase);
        BuildViewsDiff(dbDiff, newDatabase, oldDatabase);

        BuildIndexesDiff(dbDiff, newDatabase, oldDatabase);
        BuildTriggersDiff(dbDiff, newDatabase, oldDatabase);
        Mark_Indexes_Triggers_ForRedefinitionIfDepsChanged(newDatabase);

        AddDiffsForUnchangedItemsIfMarkedForRedefinition(dbDiff, newDatabase);

        BuildScriptsDiff(dbDiff, newDatabase, oldDatabase);
        return dbDiff;
    }

    protected override void BuildAdditionalTableDiffProperties(TableDiff tableDiff, Table newTable, Table oldTable)
    {
        SQLiteTableDiff tDiff = (SQLiteTableDiff)tableDiff;
        tDiff.NewTableToDefine = newTable;
        tDiff.ChangedColumnsNewNames = GetChangedColumnsNewNames();
        tDiff.ChangedColumnsOldNames = GetChangedColumnsOldNames();

        List<string> GetChangedColumnsNewNames()
        {
            return newTable.Columns.Select(x => x.Name)
                .Except(tableDiff.ColumnsToAdd.Select(x => x.Name))
                .ToList();
        }

        List<string> GetChangedColumnsOldNames()
        {
            return oldTable.Columns.Select(x => x.Name)
                .Except(tableDiff.ColumnsToDrop.Select(x => x.Name))
                .ToList();
        }
    }

    protected override void OnAddedItemProcessed<TItem>(TItem item)
    {
        _addedObjects.Add(item.ID);
        _objectsThatRequireRedefinition.Add(item.ID);
    }

    protected override void OnChangedItemProcessed<TItem>(TItem newItem, TItem oldItem)
    {
        _changedObjects.Add(newItem.ID);
        if (!CanBeAlteredWithoutRedefinition())
            _objectsThatRequireRedefinition.Add(newItem.ID);
        MarkObjectForRedefinitionIfDepsChanged(newItem);
    }

    protected override void OnUnchangedItemProcessed<TItem>(TItem item)
    {
        MarkObjectForRedefinitionIfDepsChanged(item);
    }

    private bool CanBeAlteredWithoutRedefinition()
    {
        return false;
    }

    private void MarkObjectForRedefinitionIfDepsChanged(DbObject dbObject)
    {
        if (DependencyRequiresRedefinition(dbObject))
            _objectsThatRequireRedefinition.Add(dbObject.ID);
    }

    private void Mark_Indexes_Triggers_ForRedefinitionIfDepsChanged(Database newDb)
    {
        foreach (Table table in newDb.Tables)
        {
            foreach (Index index in table.Indexes.Where(DependencyRequiresRedefinition))
                _objectsThatRequireRedefinition.Add(index.ID);
            foreach (Trigger trigger in table.Triggers.Where(DependencyRequiresRedefinition))
                _objectsThatRequireRedefinition.Add(trigger.ID);
        }
    }

    private void AddDiffsForUnchangedItemsIfMarkedForRedefinition(SQLiteDatabaseDiff dbDiff, Database newDb)
    {
        foreach (Table table in newDb.Tables.Where(IsNotAdded))
        {
            foreach (Index index in table.Indexes.Where(IsUnchanged))
            {
                if (_objectsThatRequireRedefinition.Contains(index.ID))
                {
                    dbDiff.IndexesToCreate.Add(index);
                    dbDiff.IndexesToDrop.Add(index);
                }
            }

            foreach (Trigger trigger in table.Triggers.Where(IsUnchanged))
            {
                if (_objectsThatRequireRedefinition.Contains(trigger.ID))
                {
                    dbDiff.TriggersToCreate.Add(trigger);
                    dbDiff.TriggersToDrop.Add(trigger);
                }
            }
        }

        foreach (SQLiteView view in newDb.Views.Where(IsUnchanged))
        {
            if (_objectsThatRequireRedefinition.Contains(view.ID))
            {
                dbDiff.ViewsToCreate.Add(view);
                dbDiff.ViewsToDrop.Add(view);
            }
        }

        bool IsNotAdded(DbObject dbObject)
        {
            return !_addedObjects.Contains(dbObject.ID);
        }

        bool IsUnchanged(DbObject dbObject)
        {
            return !_addedObjects.Contains(dbObject.ID) && !_changedObjects.Contains(dbObject.ID);
        }
    }

    private bool DependencyRequiresRedefinition(DbObject dbObject)
    {
        return dbObject switch
        {
            SQLiteView x => AnyRequireRedefinition(x.CreateStatement.DependsOn),
            SQLiteIndex x => _objectsThatRequireRedefinition.Contains(x.Parent.ID) || AnyRequireRedefinition(x.DependsOn),
            SQLiteTrigger x => _objectsThatRequireRedefinition.Contains(x.Parent.ID) || AnyRequireRedefinition(x.CreateStatement.DependsOn),
            _ => false,
        };
    }

    private bool AnyRequireRedefinition(IEnumerable<DbObject> dependencies)
    {
        foreach (DbObject dependency in dependencies)
        {
            if (_objectsThatRequireRedefinition.Contains(dependency.ID))
                return true;
        }
        return false;
    }
}
