using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis.Common;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Analysis.Extensions.MSSQL;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;
using DotNetDBTools.Models.MSSQL.UserDefinedTypes;

namespace DotNetDBTools.Analysis.MSSQL;

internal class MSSQLDiffCreator : DiffCreator
{
    private readonly HashSet<Guid> _addedObjects = new();
    private readonly HashSet<Guid> _changedObjects = new();
    private readonly HashSet<Guid> _objectsThatRequireRedefinition = new();
    private readonly HashSet<Guid> _objectsThatRequireDefaultRedefinition = new();

    public override DatabaseDiff CreateDatabaseDiff(Database newDatabase, Database oldDatabase)
    {
        _addedObjects.Clear();
        _changedObjects.Clear();
        _objectsThatRequireRedefinition.Clear();
        _objectsThatRequireDefaultRedefinition.Clear();

        MSSQLDatabaseDiff dbDiff = new()
        {
            NewDatabase = newDatabase,
            OldDatabase = oldDatabase,
        };

        BuildUserDefinedTypesDiff(dbDiff);
        BuildTablesDiff<MSSQLTableDiff>(dbDiff);
        BuildViewsDiff(dbDiff);

        BuildIndexesDiff(dbDiff);
        BuildTriggersDiff(dbDiff);

        AddDiffsForUnchangedItemsIfMarkedForRedefinition(dbDiff);
        ForeignKeysHelper.BuildUnchangedForeignKeysToRecreateBecauseOfDeps(dbDiff);

        BuildScriptsDiff(dbDiff);
        return dbDiff;
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
        Mark_Columns_ForRedefinitionIfDepsChanged(newItem);
    }

    protected override void OnUnchangedItemProcessed<TItem>(TItem item)
    {
        Mark_Columns_ForRedefinitionIfDepsChanged(item);
    }

    private void BuildUserDefinedTypesDiff(MSSQLDatabaseDiff dbDiff)
    {
        List<MSSQLUserDefinedType> addedUserDefinedTypes = null;
        List<MSSQLUserDefinedType> removedUserDefinedTypes = null;
        List<MSSQLUserDefinedTypeDiff> changedUserDefinedTypes = new();
        FillAddedAndRemovedItemsAndApplyActionToChangedItems(
            ((MSSQLDatabase)dbDiff.NewDatabase).UserDefinedTypes, ((MSSQLDatabase)dbDiff.OldDatabase).UserDefinedTypes,
            ref addedUserDefinedTypes, ref removedUserDefinedTypes,
            (newType, oldType) =>
            {
                MSSQLUserDefinedTypeDiff udtDiff = new()
                {
                    NewUserDefinedType = newType,
                    OldUserDefinedType = oldType,
                };
                changedUserDefinedTypes.Add(udtDiff);
            });

        dbDiff.AddedUserDefinedTypes = addedUserDefinedTypes;
        dbDiff.RemovedUserDefinedTypes = removedUserDefinedTypes;
        dbDiff.ChangedUserDefinedTypes = changedUserDefinedTypes;
    }

    private bool CanBeAlteredWithoutRedefinition(DbObject newDbObject, DbObject oldDbObject)
    {
        if (newDbObject is MSSQLUserDefinedType)
            return false;
        else if (newDbObject is MSSQLColumn column)
            return AreEqual(column.DataType, ((MSSQLColumn)oldDbObject).DataType);
        else
            return true;
    }

    private void Mark_Columns_ForRedefinitionIfDepsChanged(DbObject dbObject)
    {
        if (dbObject is MSSQLTable table)
        {
            foreach (Column column in table.Columns.Where(DependencyRequiresRedefinition))
            {
                _objectsThatRequireRedefinition.Add(column.ID);
                _objectsThatRequireDefaultRedefinition.Add(column.ID);
            }
        }
        else if (dbObject is MSSQLColumn column)
        {
            if (DependencyRequiresRedefinition(column))
            {
                _objectsThatRequireRedefinition.Add(column.ID);
                _objectsThatRequireDefaultRedefinition.Add(column.ID);
            }
        }

        bool DependencyRequiresRedefinition(DbObject dbObject)
        {
            return dbObject switch
            {
                MSSQLColumn x => AnyRequireRedefinition(x.DataType.DependsOn),
                _ => false,
            };
        }
    }

    private void AddDiffsForUnchangedItemsIfMarkedForRedefinition(MSSQLDatabaseDiff dbDiff)
    {
        MSSQLDatabase newDb = (MSSQLDatabase)dbDiff.NewDatabase;
        AddForTableObjects();

        void AddForTableObjects()
        {
            Dictionary<Guid, MSSQLTableDiff> tableIdToTableDiffMap = dbDiff.ChangedTables
                .ToDictionary(x => x.NewTable.ID, x => (MSSQLTableDiff)x);
            foreach (Table table in newDb.Tables.Where(IsNotAdded))
            {
                if (AnyUnchangedColumnRequiresDataTypeOrDefaultRedefinition(table))
                {
                    MSSQLTableDiff tableDiff = tableIdToTableDiffMap.ContainsKey(table.ID)
                        ? tableIdToTableDiffMap[table.ID]
                        : table.CreateEmptyTableDiff();
                    if (!tableIdToTableDiffMap.ContainsKey(table.ID))
                        dbDiff.ChangedTables.Add(tableDiff);

                    Dictionary<Guid, ColumnDiff> columnIdToColumnDiffMap = tableDiff.ColumnsToAlter
                        .ToDictionary(x => x.NewColumn.ID, x => x);
                    foreach (Column column in table.Columns.Where(IsUnchanged))
                    {
                        if (_objectsThatRequireRedefinition.Contains(column.ID)
                            || _objectsThatRequireDefaultRedefinition.Contains(column.ID)
                                && column.Default.Code != null)
                        {
                            ColumnDiff columnDiff = columnIdToColumnDiffMap.ContainsKey(column.ID)
                                ? columnIdToColumnDiffMap[column.ID]
                                : column.CreateEmptyColumnDiff();
                            if (!columnIdToColumnDiffMap.ContainsKey(column.ID))
                                tableDiff.ColumnsToAlter.Add(columnDiff);

                            if (_objectsThatRequireRedefinition.Contains(column.ID))
                                columnDiff.DataTypeToSet = column.DataType;
                            if (_objectsThatRequireDefaultRedefinition.Contains(column.ID)
                                && column.Default.Code != null)
                            {
                                columnDiff.DefaultToSet = column.Default;
                                columnDiff.DefaultToDrop = column.Default;
                                columnDiff.NewColumn.Default = column.Default;
                                columnDiff.OldColumn.Default = column.Default;
                                string defaultConstraintName = ((MSSQLColumn)column).DefaultConstraintName;
                                ((MSSQLColumn)columnDiff.NewColumn).DefaultConstraintName = defaultConstraintName;
                                ((MSSQLColumn)columnDiff.OldColumn).DefaultConstraintName = defaultConstraintName;
                            }
                        }
                    }
                }
            }

            bool AnyUnchangedColumnRequiresDataTypeOrDefaultRedefinition(Table table)
            {
                return table.Columns.Any(x => IsUnchanged(x) &&
                    (_objectsThatRequireRedefinition.Contains(x.ID)
                    || _objectsThatRequireDefaultRedefinition.Contains(x.ID)));
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
