﻿using System;
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
    private readonly HashSet<Guid> _objectsThatRequireRedefinition = new();
    private readonly HashSet<Guid> _objectsThatRequireDefaultRedefinition = new();

    public override DatabaseDiff CreateDatabaseDiff(Database newDatabase, Database oldDatabase)
    {
        _addedObjects.Clear();
        _objectsThatRequireRedefinition.Clear();
        _objectsThatRequireDefaultRedefinition.Clear();

        MSSQLDatabase newDb = (MSSQLDatabase)newDatabase;
        MSSQLDatabase oldDb = (MSSQLDatabase)oldDatabase;
        MSSQLDatabaseDiff dbDiff = new()
        {
            NewDatabaseVersion = newDatabase.Version,
            OldDatabaseVersion = oldDatabase.Version,
        };

        BuildUserDefinedTypesDiff(dbDiff, newDb, oldDb);
        BuildTablesDiff<MSSQLTableDiff, MSSQLColumnDiff>(dbDiff, newDb, oldDb);
        BuildViewsDiff(dbDiff, newDb, oldDb);

        BuildIndexesDiff(dbDiff, newDb, oldDb);
        BuildTriggersDiff(dbDiff, newDb, oldDb);

        AddDiffsForUnchangedItemsIfMarkedForRedefinition(dbDiff, newDb);
        ForeignKeysHelper.BuildUnchangedForeignKeysToRecreateBecauseOfDeps(dbDiff, oldDb);

        BuildScriptsDiff(dbDiff, newDb, oldDb);
        return dbDiff;
    }

    protected override void BuildAdditionalColumnDiffProperties(ColumnDiff columnDiff, Column newColumn, Column oldColumn)
    {
        if (columnDiff.IdentityToSet is not null)
            throw new Exception($"Identity change for existing column is not supported (ColumnID='{columnDiff.ColumnID}')");

        if (columnDiff.DataTypeToSet is not null || columnDiff.NotNullToSet is not null)
        {
            columnDiff.DataTypeToSet = newColumn.DataType;
            columnDiff.NotNullToSet = newColumn.NotNull;
            SetDefaultChanged(columnDiff, newColumn, oldColumn);
        }

        if (!AreEqual(((MSSQLColumn)newColumn).DefaultConstraintName, ((MSSQLColumn)oldColumn).DefaultConstraintName))
            SetDefaultChanged(columnDiff, newColumn, oldColumn);

        if (columnDiff.DefaultToSet is not null)
            ((MSSQLColumnDiff)columnDiff).DefaultToSetConstraintName = ((MSSQLColumn)newColumn).DefaultConstraintName;
        if (columnDiff.DefaultToDrop is not null)
            ((MSSQLColumnDiff)columnDiff).DefaultToDropConstraintName = ((MSSQLColumn)oldColumn).DefaultConstraintName;
    }

    protected override void OnAddedItemProcessed<TItem>(TItem item)
    {
        _addedObjects.Add(item.ID);
        _objectsThatRequireRedefinition.Add(item.ID);
    }

    protected override void OnChangedItemProcessed<TItem>(TItem newItem, TItem oldItem)
    {
        if (!CanBeAlteredWithoutRedefinition(newItem, oldItem))
            _objectsThatRequireRedefinition.Add(newItem.ID);
        MarkObjectForRedefinitionIfDepsChanged(newItem);
    }

    protected override void OnUnchangedItemProcessed<TItem>(TItem item)
    {
        MarkObjectForRedefinitionIfDepsChanged(item);
    }

    private void BuildUserDefinedTypesDiff(MSSQLDatabaseDiff dbDiff, MSSQLDatabase newDb, MSSQLDatabase oldDb)
    {
        FillAddedAndRemovedItemsAndAddChangedToBoth(
            newDb.UserDefinedTypes,
            oldDb.UserDefinedTypes,
            out List<MSSQLUserDefinedType> addedUserDefinedTypes,
            out List<MSSQLUserDefinedType> removedUserDefinedTypes);

        dbDiff.UserDefinedTypesToCreate = addedUserDefinedTypes;
        dbDiff.UserDefinedTypesToDrop = removedUserDefinedTypes;
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

    private void MarkObjectForRedefinitionIfDepsChanged(DbObject dbObject)
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
    }

    private void AddDiffsForUnchangedItemsIfMarkedForRedefinition(MSSQLDatabaseDiff dbDiff, MSSQLDatabase newDb)
    {
        AddForTableObjects();

        void AddForTableObjects()
        {
            Dictionary<Guid, MSSQLTableDiff> tableIdToTableDiffMap = dbDiff.ChangedTables
                .ToDictionary(x => x.TableID, x => (MSSQLTableDiff)x);
            foreach (Table table in newDb.Tables.Where(IsNotAdded))
            {
                if (table.Columns.Any(RequiresDataTypeOrDefaultRedifinition))
                {
                    MSSQLTableDiff tableDiff = tableIdToTableDiffMap.ContainsKey(table.ID)
                        ? tableIdToTableDiffMap[table.ID]
                        : table.CreateEmptyTableDiff();
                    if (!tableIdToTableDiffMap.ContainsKey(table.ID))
                        dbDiff.ChangedTables.Add(tableDiff);

                    Dictionary<Guid, MSSQLColumnDiff> columnIdToColumnDiffMap = tableDiff.ColumnsToAlter
                        .ToDictionary(x => x.ColumnID, x => (MSSQLColumnDiff)x);
                    foreach (Column column in table.Columns.Where(IsNotAdded))
                    {
                        if (RequiresDataTypeOrDefaultRedifinition(column))
                        {
                            MSSQLColumnDiff columnDiff = columnIdToColumnDiffMap.ContainsKey(column.ID)
                                ? columnIdToColumnDiffMap[column.ID]
                                : column.CreateEmptyColumnDiff();
                            if (!columnIdToColumnDiffMap.ContainsKey(column.ID))
                                tableDiff.ColumnsToAlter.Add(columnDiff);

                            if (RequiresDataTypeRedifinition(column))
                            {
                                columnDiff.DataTypeToSet = column.DataType;
                                columnDiff.NotNullToSet = column.NotNull;
                            }

                            bool defaultChanged = columnDiff.DefaultToSet is not null || columnDiff.DefaultToDrop is not null;
                            if (!defaultChanged)
                            {
                                string defaultConstraintName = ((MSSQLColumn)column).DefaultConstraintName;

                                columnDiff.DefaultToSet = column.Default;
                                columnDiff.DefaultToSetConstraintName = defaultConstraintName;

                                columnDiff.DefaultToDrop = column.Default;
                                columnDiff.DefaultToDropConstraintName = defaultConstraintName;
                            }
                        }
                    }
                }
            }

            bool RequiresDataTypeOrDefaultRedifinition(Column column)
            {
                return RequiresDataTypeRedifinition(column)
                    || RequiresDefaultRedifinition(column);
            }

            bool RequiresDataTypeRedifinition(Column column)
            {
                return _objectsThatRequireRedefinition.Contains(column.ID);
            }

            bool RequiresDefaultRedifinition(Column column)
            {
                return _objectsThatRequireDefaultRedefinition.Contains(column.ID)
                    && column.Default is not null;
            }
        }

        bool IsNotAdded(DbObject dbObject)
        {
            return !_addedObjects.Contains(dbObject.ID);
        }
    }

    private bool DependencyRequiresRedefinition(DbObject dbObject)
    {
        return dbObject switch
        {
            MSSQLColumn x => AnyRequireRedefinition(x.DataType.DependsOn),
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
