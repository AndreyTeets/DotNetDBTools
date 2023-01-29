using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis.Common;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Analysis.Extensions.MySQL;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MySQL;

namespace DotNetDBTools.Analysis.MySQL;

internal class MySQLDiffCreator : DiffCreator
{
    private readonly HashSet<Guid> _addedObjects = new();
    private readonly HashSet<Guid> _changedObjects = new();
    private readonly HashSet<Guid> _objectsThatRequireRedefinition = new();

    public override DatabaseDiff CreateDatabaseDiff(Database newDatabase, Database oldDatabase)
    {
        _addedObjects.Clear();
        _changedObjects.Clear();
        _objectsThatRequireRedefinition.Clear();

        MySQLDatabaseDiff dbDiff = new()
        {
            NewVersion = newDatabase.Version,
            OldVersion = oldDatabase.Version,
        };

        BuildTablesDiff<MySQLTableDiff, MySQLColumnDiff>(dbDiff, newDatabase, oldDatabase);
        BuildViewsDiff(dbDiff, newDatabase, oldDatabase);

        BuildIndexesDiff(dbDiff, newDatabase, oldDatabase);
        BuildTriggersDiff(dbDiff, newDatabase, oldDatabase);
        Mark_Constraints_Indexes_Triggers_ForRedefinitionIfDepsChanged(newDatabase);

        AddDiffsForUnchangedItemsIfMarkedForRedefinition(dbDiff, newDatabase);
        ForeignKeysHelper.BuildUnchangedForeignKeysToRecreateBecauseOfChangedReferencedObjects(dbDiff, oldDatabase);

        BuildScriptsDiff(dbDiff, newDatabase, oldDatabase);
        return dbDiff;
    }

    protected override void BuildAdditionalColumnDiffProperties(ColumnDiff columnDiff, Column newColumn, Column oldColumn)
    {
        if (columnDiff.DataTypeToSet is not null
            || columnDiff.NotNullToSet is not null
            || columnDiff.IdentityToSet is not null)
        {
            ((MySQLColumnDiff)columnDiff).DefinitionToSet = newColumn;
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
        if (!CanBeAlteredWithoutRedefinition(newItem, oldItem))
            _objectsThatRequireRedefinition.Add(newItem.ID);
        MarkObjectForRedefinitionIfDepsChanged(newItem);
    }

    protected override void OnUnchangedItemProcessed<TItem>(TItem item)
    {
        MarkObjectForRedefinitionIfDepsChanged(item);
    }

    private bool CanBeAlteredWithoutRedefinition(DbObject newDbObject, DbObject oldDbObject)
    {
        if (newDbObject is Column column)
            return AreEqual(column.DataType, ((Column)oldDbObject).DataType);
        else
            return true;
    }

    private void MarkObjectForRedefinitionIfDepsChanged(DbObject dbObject)
    {
        if (DependencyRequiresRedefinition(dbObject))
            _objectsThatRequireRedefinition.Add(dbObject.ID);
    }

    private void Mark_Constraints_Indexes_Triggers_ForRedefinitionIfDepsChanged(Database newDb)
    {
        foreach (Table table in newDb.Tables)
        {
            if (table.PrimaryKey is not null && DependencyRequiresRedefinition(table.PrimaryKey))
                _objectsThatRequireRedefinition.Add(table.PrimaryKey.ID);
            foreach (UniqueConstraint uc in table.UniqueConstraints.Where(DependencyRequiresRedefinition))
                _objectsThatRequireRedefinition.Add(uc.ID);
            foreach (CheckConstraint ck in table.CheckConstraints.Where(DependencyRequiresRedefinition))
                _objectsThatRequireRedefinition.Add(ck.ID);
            foreach (ForeignKey fk in table.ForeignKeys.Where(DependencyRequiresRedefinition))
                _objectsThatRequireRedefinition.Add(fk.ID);
            foreach (Index index in table.Indexes.Where(DependencyRequiresRedefinition))
                _objectsThatRequireRedefinition.Add(index.ID);
            foreach (Trigger trigger in table.Triggers.Where(DependencyRequiresRedefinition))
                _objectsThatRequireRedefinition.Add(trigger.ID);
        }
    }

    private void AddDiffsForUnchangedItemsIfMarkedForRedefinition(MySQLDatabaseDiff dbDiff, Database newDb)
    {
        AddForTableObjects();

        void AddForTableObjects()
        {
            Dictionary<Guid, MySQLTableDiff> tableIdToTableDiffMap = dbDiff.ChangedTables
                .ToDictionary(x => x.ID, x => (MySQLTableDiff)x);
            foreach (Table table in newDb.Tables.Where(IsNotAdded))
            {
                if (AnyUnchangedConstraintRequiresRedefinition(table))
                {
                    MySQLTableDiff tableDiff = tableIdToTableDiffMap.ContainsKey(table.ID)
                        ? tableIdToTableDiffMap[table.ID]
                        : table.CreateEmptyTableDiff();
                    if (!tableIdToTableDiffMap.ContainsKey(table.ID))
                        dbDiff.ChangedTables.Add(tableDiff);

                    if (table.PrimaryKey is not null && IsUnchanged(table.PrimaryKey))
                    {
                        if (_objectsThatRequireRedefinition.Contains(table.PrimaryKey.ID))
                        {
                            tableDiff.PrimaryKeyToCreate = table.PrimaryKey;
                            tableDiff.PrimaryKeyToDrop = table.PrimaryKey;
                        }
                    }
                    foreach (UniqueConstraint uc in table.UniqueConstraints.Where(IsUnchanged))
                    {
                        if (_objectsThatRequireRedefinition.Contains(uc.ID))
                        {
                            tableDiff.UniqueConstraintsToCreate.Add(uc);
                            tableDiff.UniqueConstraintsToDrop.Add(uc);
                        }
                    }
                    foreach (CheckConstraint ck in table.CheckConstraints.Where(IsUnchanged))
                    {
                        if (_objectsThatRequireRedefinition.Contains(ck.ID))
                        {
                            tableDiff.CheckConstraintsToCreate.Add(ck);
                            tableDiff.CheckConstraintsToDrop.Add(ck);
                        }
                    }
                    foreach (ForeignKey fk in table.ForeignKeys.Where(IsUnchanged))
                    {
                        if (_objectsThatRequireRedefinition.Contains(fk.ID))
                        {
                            tableDiff.ForeignKeysToCreate.Add(fk);
                            tableDiff.ForeignKeysToDrop.Add(fk);
                        }
                    }
                }

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

            bool AnyUnchangedConstraintRequiresRedefinition(Table table)
            {
                return table.PrimaryKey is not null && IsUnchanged(table.PrimaryKey)
                        && _objectsThatRequireRedefinition.Contains(table.PrimaryKey.ID)
                    || table.UniqueConstraints.Any(x => IsUnchanged(x)
                        && _objectsThatRequireRedefinition.Contains(x.ID))
                    || table.CheckConstraints.Any(x => IsUnchanged(x)
                        && _objectsThatRequireRedefinition.Contains(x.ID))
                    || table.ForeignKeys.Any(x => IsUnchanged(x)
                        && _objectsThatRequireRedefinition.Contains(x.ID));
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
            PrimaryKey x => AnyRequireRedefinition(x.DependsOn),
            UniqueConstraint x => AnyRequireRedefinition(x.DependsOn),
            CheckConstraint x => AnyRequireRedefinition(x.Expression.DependsOn),
            ForeignKey x => AnyRequireRedefinition(x.DependsOn),
            MySQLIndex x => AnyRequireRedefinition(x.DependsOn),
            MySQLTrigger x => AnyRequireRedefinition(x.CreateStatement.DependsOn),
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
