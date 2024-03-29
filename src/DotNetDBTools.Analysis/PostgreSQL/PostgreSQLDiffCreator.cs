﻿using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis.Common;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Analysis.Extensions;
using DotNetDBTools.Analysis.Extensions.PostgreSQL;
using DotNetDBTools.Models;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;
using DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;

namespace DotNetDBTools.Analysis.PostgreSQL;

internal class PostgreSQLDiffCreator : DiffCreator
{
    private readonly HashSet<Guid> _addedObjects = new();
    private readonly HashSet<Guid> _changedObjects = new();
    private readonly HashSet<Guid> _objectsThatRequireRedefinition = new();
    private readonly HashSet<Guid> _objectsThatRequireDefaultRedefinition = new();
    private readonly HashSet<Guid> _domainTypesThatHaveTableColumnTransitivelyDependingOnItThroughComplexType = new();

    public override DatabaseDiff CreateDatabaseDiff(Database newDatabase, Database oldDatabase)
    {
        _addedObjects.Clear();
        _changedObjects.Clear();
        _objectsThatRequireRedefinition.Clear();
        _objectsThatRequireDefaultRedefinition.Clear();
        _domainTypesThatHaveTableColumnTransitivelyDependingOnItThroughComplexType.Clear();
        FillDomainTypesThatHaveTableColumnTransitivelyDependingOnItThroughComplexType(newDatabase);

        PostgreSQLDatabase newDb = (PostgreSQLDatabase)newDatabase;
        PostgreSQLDatabase oldDb = (PostgreSQLDatabase)oldDatabase;
        PostgreSQLDatabaseDiff dbDiff = new()
        {
            NewVersion = newDatabase.Version,
            OldVersion = oldDatabase.Version,
        };

        BuildSequencesDiff(dbDiff, newDb, oldDb);
        BuildTypesDiff(dbDiff, newDb, oldDb);
        BuildTablesDiff<PostgreSQLTableDiff, PostgreSQLColumnDiff>(dbDiff, newDb, oldDb);
        BuildProgrammableObjectsDiff(dbDiff, newDb, oldDb);

        BuildIndexesDiff(dbDiff, newDb, oldDb);
        BuildTriggersDiff(dbDiff, newDb, oldDb);
        Mark_Constraints_Indexes_Triggers_ForRedefinitionIfDepsChanged(newDb);

        AddDiffsForUnchangedItemsIfMarkedForRedefinition(dbDiff, newDb);
        ForeignKeysHelper.BuildUnchangedForeignKeysToRecreateBecauseOfChangedReferencedObjects(dbDiff, oldDb);

        BuildScriptsDiff(dbDiff, newDb, oldDb);
        return dbDiff;
    }

    protected override void BuildAdditionalColumnDiffProperties(ColumnDiff columnDiff, Column newColumn, Column oldColumn)
    {
        PostgreSQLColumnDiff cDiff = (PostgreSQLColumnDiff)columnDiff;
        PostgreSQLColumn newC = (PostgreSQLColumn)newColumn;
        PostgreSQLColumn oldC = (PostgreSQLColumn)oldColumn;

        if (newC.IdentityGenerationKind != oldC.IdentityGenerationKind)
            cDiff.IdentityGenerationKindToSet = newC.IdentityGenerationKind;

        if (newC.IdentitySequenceOptions is not null && oldC.IdentitySequenceOptions is null)
            cDiff.IdentitySequenceOptionsToSet = newC.IdentitySequenceOptions;
        else if (newC.IdentitySequenceOptions is not null && oldC.IdentitySequenceOptions is not null)
            cDiff.IdentitySequenceOptionsToSet = GetChangedSequenceOptions(newC.IdentitySequenceOptions, oldC.IdentitySequenceOptions);
    }

    protected override void OnAddedItemProcessed<TItem>(TItem item)
    {
        _addedObjects.Add(item.ID);
        _objectsThatRequireRedefinition.Add(item.ID);
        // TODO don't add table when parsing column deps works for programmable objects
        if (item is Column column)
            _objectsThatRequireRedefinition.Add(column.Parent.ID);
    }

    protected override void OnChangedItemProcessed<TItem>(TItem newItem, TItem oldItem)
    {
        _changedObjects.Add(newItem.ID);
        if (!CanBeAlteredWithoutRedefinition(newItem, oldItem))
        {
            _objectsThatRequireRedefinition.Add(newItem.ID);
            // TODO don't add table when parsing column deps works for programmable objects
            if (newItem is Column column)
                _objectsThatRequireRedefinition.Add(column.Parent.ID);
        }
        MarkObjectForRedefinitionIfDepsChanged(newItem);
    }

    protected override void OnUnchangedItemProcessed<TItem>(TItem item)
    {
        MarkObjectForRedefinitionIfDepsChanged(item);
    }

    private void FillDomainTypesThatHaveTableColumnTransitivelyDependingOnItThroughComplexType(Database database)
    {
        foreach (Table table in database.Tables)
        {
            foreach (Column column in table.Columns.Where(x => x.DataType.DependsOn.Count == 1))
            {
                DbObject udt = column.DataType.DependsOn.Single();
                if (udt is PostgreSQLDomainType && IsArray(column.DataType.Name))
                    _domainTypesThatHaveTableColumnTransitivelyDependingOnItThroughComplexType.Add(udt.ID);

                IEnumerable<DbObject> allUdtTransitiveDeps = udt.GetTransitiveDependencies(x => x.GetDependencies());
                foreach (PostgreSQLDomainType domainType in allUdtTransitiveDeps.OfType<PostgreSQLDomainType>())
                    _domainTypesThatHaveTableColumnTransitivelyDependingOnItThroughComplexType.Add(domainType.ID);
            }
        }

        static bool IsArray(string typeName)
        {
            PostgreSQLHelperMethods.GetNormalizedTypeNameWithoutArray(typeName, out string _, out string arrayDimStr);
            return arrayDimStr != "";
        }
    }

    private void BuildSequencesDiff(PostgreSQLDatabaseDiff dbDiff, PostgreSQLDatabase newDb, PostgreSQLDatabase oldDb)
    {
        List<PostgreSQLSequence> addedSequences = null;
        List<PostgreSQLSequence> removedSequences = null;
        List<PostgreSQLSequenceDiff> changedSequences = new();
        FillAddedAndRemovedItemsAndApplyActionToChangedItems(
            newDb.Sequences,
            oldDb.Sequences,
            ref addedSequences, ref removedSequences,
            (newSequence, oldSequence) =>
            {
                bool ownedByChanged = !AreEqual(newSequence.OwnedBy, oldSequence.OwnedBy);
                PostgreSQLSequenceDiff sequenceDiff = new()
                {
                    ID = newSequence.ID,
                    NewName = newSequence.Name,
                    OldName = oldSequence.Name,
                    DataTypeToSet = !AreEqual(newSequence.DataType, oldSequence.DataType) ? newSequence.DataType : null,
                    OptionsToSet = GetChangedSequenceOptions(newSequence.Options, oldSequence.Options),
                    OwnedByToSet = ownedByChanged && newSequence.OwnedBy != (null, null) ? newSequence.OwnedBy : (null, null),
                    OwnedByToDrop = ownedByChanged && newSequence.OwnedBy == (null, null) ? oldSequence.OwnedBy : (null, null),
                };
                changedSequences.Add(sequenceDiff);
            });

        dbDiff.SequencesToCreate = addedSequences;
        dbDiff.SequencesToDrop = removedSequences;
        dbDiff.SequencesToAlter = changedSequences;
    }

    private void BuildTypesDiff(PostgreSQLDatabaseDiff dbDiff, PostgreSQLDatabase newDb, PostgreSQLDatabase oldDb)
    {
        List<DbObject> objectsToCreate = null;
        List<DbObject> objectsToDrop = null;
        List<object> objectsToAlter = new();
        FillAddedAndRemovedItemsAndApplyActionToChangedItems(
            GetObjectsOrderedByDependencies(newDb),
            GetObjectsOrderedByDependencies(oldDb),
            ref objectsToCreate,
            ref objectsToDrop,
            (newObject, oldObject) =>
            {
                if (newObject is PostgreSQLDomainType
                    && CanBeAlteredWithoutRedefinition(newObject, oldObject)
                    && !DependencyRequiresRedefinition(newObject))
                {
                    PostgreSQLDomainTypeDiff domainTypeDiff = CreateDomainTypeDiff(
                        (PostgreSQLDomainType)newObject, (PostgreSQLDomainType)oldObject);
                    objectsToAlter.Add(domainTypeDiff);
                }
                else
                {
                    objectsToCreate.Add(newObject);
                    objectsToDrop.Add(oldObject);
                }
            });

        dbDiff.CompositeTypesToCreate = objectsToCreate.OfType<PostgreSQLCompositeType>().ToList();
        dbDiff.CompositeTypesToDrop = objectsToDrop.OfType<PostgreSQLCompositeType>().ToList();

        dbDiff.DomainTypesToCreate = objectsToCreate.OfType<PostgreSQLDomainType>().ToList();
        dbDiff.DomainTypesToDrop = objectsToDrop.OfType<PostgreSQLDomainType>().ToList();
        dbDiff.DomainTypesToAlter = objectsToAlter.OfType<PostgreSQLDomainTypeDiff>().ToList();

        dbDiff.EnumTypesToCreate = objectsToCreate.OfType<PostgreSQLEnumType>().ToList();
        dbDiff.EnumTypesToDrop = objectsToDrop.OfType<PostgreSQLEnumType>().ToList();

        dbDiff.RangeTypesToCreate = objectsToCreate.OfType<PostgreSQLRangeType>().ToList();
        dbDiff.RangeTypesToDrop = objectsToDrop.OfType<PostgreSQLRangeType>().ToList();

        static IEnumerable<DbObject> GetObjectsOrderedByDependencies(PostgreSQLDatabase db)
        {
            return db.CompositeTypes.Select(x => (DbObject)x)
                .Concat(db.DomainTypes.Select(x => (DbObject)x))
                .Concat(db.EnumTypes.Select(x => (DbObject)x))
                .Concat(db.RangeTypes.Select(x => (DbObject)x))
                .OrderByDependenciesFirst(x => x.GetDependencies());
        }

        PostgreSQLDomainTypeDiff CreateDomainTypeDiff(PostgreSQLDomainType newType, PostgreSQLDomainType oldType)
        {
            FillAddedAndRemovedItemsAndAddChangedToBoth(
                newType.CheckConstraints,
                oldType.CheckConstraints,
                out List<CheckConstraint> checkConstraintsToCreate,
                out List<CheckConstraint> checkConstraintsToDrop);

            bool defaultChanged = !AreEqual(newType.Default, oldType.Default);
            return new PostgreSQLDomainTypeDiff()
            {
                ID = newType.ID,
                NewName = newType.Name,
                OldName = oldType.Name,
                NotNullToSet = newType.NotNull != oldType.NotNull ? newType.NotNull : null,
                DefaultToSet = defaultChanged && newType.Default is not null ? newType.Default : null,
                DefaultToDrop = defaultChanged && oldType.Default is not null ? oldType.Default : null,
                CheckConstraintsToCreate = checkConstraintsToCreate,
                CheckConstraintsToDrop = checkConstraintsToDrop,
            };
        }
    }

    private void BuildProgrammableObjectsDiff(PostgreSQLDatabaseDiff dbDiff, PostgreSQLDatabase newDb, PostgreSQLDatabase oldDb)
    {
        FillAddedAndRemovedItemsAndAddChangedToBoth(
            GetObjectsOrderedByDependencies(newDb),
            GetObjectsOrderedByDependencies(oldDb),
            out List<DbObject> objectsToCreate,
            out List<DbObject> objectsToDrop);

        dbDiff.ViewsToCreate = objectsToCreate.OfType<View>().ToList();
        dbDiff.ViewsToDrop = objectsToDrop.OfType<View>().ToList();

        dbDiff.FunctionsToCreate = objectsToCreate.OfType<PostgreSQLFunction>().ToList();
        dbDiff.FunctionsToDrop = objectsToDrop.OfType<PostgreSQLFunction>().ToList();

        dbDiff.ProceduresToCreate = objectsToCreate.OfType<PostgreSQLProcedure>().ToList();
        dbDiff.ProceduresToDrop = objectsToDrop.OfType<PostgreSQLProcedure>().ToList();

        static IEnumerable<DbObject> GetObjectsOrderedByDependencies(PostgreSQLDatabase db)
        {
            return db.Views.Select(x => (DbObject)x)
                .Concat(db.Functions.Select(x => (DbObject)x))
                .Concat(db.Procedures.Select(x => (DbObject)x))
                .OrderByDependenciesFirst(x => x.GetDependencies());
        }
    }

    private bool CanBeAlteredWithoutRedefinition(DbObject newDbObject, DbObject oldDbObject)
    {
        if (newDbObject is PostgreSQLSequence
            || newDbObject is PostgreSQLTable)
        {
            return true;
        }
        else if (newDbObject is PostgreSQLColumn column)
        {
            return AreEqual(column.DataType, ((PostgreSQLColumn)oldDbObject).DataType);
        }
        else if (newDbObject is PostgreSQLDomainType newType)
        {
            PostgreSQLDomainType oldTypeExceptAlterableProperties = (PostgreSQLDomainType)oldDbObject.CopyModel();
            oldTypeExceptAlterableProperties.Name = newType.Name;
            oldTypeExceptAlterableProperties.NotNull = newType.NotNull;
            oldTypeExceptAlterableProperties.Default = newType.Default;
            oldTypeExceptAlterableProperties.CheckConstraints = newType.CheckConstraints;
            return AreEqual(newType, oldTypeExceptAlterableProperties)
                && !_domainTypesThatHaveTableColumnTransitivelyDependingOnItThroughComplexType.Contains(newType.ID);
        }
        return false;
    }

    private void MarkObjectForRedefinitionIfDepsChanged(DbObject dbObject)
    {
        if (dbObject is PostgreSQLTable table)
        {
            foreach (Column column in table.Columns.Where(DependencyRequiresRedefinition))
            {
                _objectsThatRequireRedefinition.Add(column.ID);
                // TODO don't add table when parsing column deps works for programmable objects
                _objectsThatRequireRedefinition.Add(column.Parent.ID);
            }
        }
        else if (DependencyRequiresRedefinition(dbObject))
        {
            _objectsThatRequireRedefinition.Add(dbObject.ID);
            // TODO don't add table when parsing column deps works for programmable objects
            if (dbObject is Column column)
                _objectsThatRequireRedefinition.Add(column.Parent.ID);
        }
    }

    private void Mark_Constraints_Indexes_Triggers_ForRedefinitionIfDepsChanged(PostgreSQLDatabase newDb)
    {
        foreach (PostgreSQLDomainType type in newDb.DomainTypes)
        {
            if (DependencyRequiresDefaultRedefinition(type))
                _objectsThatRequireDefaultRedefinition.Add(type.ID);
            foreach (CheckConstraint ck in type.CheckConstraints.Where(DependencyRequiresRedefinition))
                _objectsThatRequireRedefinition.Add(ck.ID);
        }
        foreach (Table table in newDb.Tables)
        {
            foreach (Column column in table.Columns.Where(DependencyRequiresDefaultRedefinition))
                _objectsThatRequireDefaultRedefinition.Add(column.ID);
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

    private void AddDiffsForUnchangedItemsIfMarkedForRedefinition(PostgreSQLDatabaseDiff dbDiff, PostgreSQLDatabase newDb)
    {
        AddForTypes();
        AddForTableObjects();
        AddForProgrammableObjects();

        void AddForTypes()
        {
            foreach (PostgreSQLCompositeType type in newDb.CompositeTypes.Where(IsUnchanged))
            {
                if (_objectsThatRequireRedefinition.Contains(type.ID))
                {
                    dbDiff.CompositeTypesToCreate.Add(type);
                    dbDiff.CompositeTypesToDrop.Add(type);
                }
            }

            Dictionary<Guid, PostgreSQLDomainTypeDiff> typeIdToTypeDiffMap = dbDiff.DomainTypesToAlter
                .ToDictionary(x => x.ID, x => x);
            foreach (PostgreSQLDomainType type in newDb.DomainTypes.Where(IsNotAdded))
            {
                // Those that changed and require redefinition are already added when change processed
                if (_objectsThatRequireRedefinition.Contains(type.ID) && IsUnchanged(type))
                {
                    dbDiff.DomainTypesToCreate.Add(type);
                    dbDiff.DomainTypesToDrop.Add(type);
                }
                else if (RequiresDefaultRedifinition(type)
                    || AnyUnchangedCheckConstraintRequiresRedefinition(type))
                {
                    PostgreSQLDomainTypeDiff typeDiff = typeIdToTypeDiffMap.ContainsKey(type.ID)
                        ? typeIdToTypeDiffMap[type.ID]
                        : type.CreateEmptyDomainTypeDiff();
                    if (!typeIdToTypeDiffMap.ContainsKey(type.ID))
                        dbDiff.DomainTypesToAlter.Add(typeDiff);

                    bool defaultChanged = typeDiff.DefaultToSet is not null || typeDiff.DefaultToDrop is not null;
                    if (RequiresDefaultRedifinition(type) && !defaultChanged)
                    {
                        typeDiff.DefaultToSet = type.Default;
                        typeDiff.DefaultToDrop = type.Default;
                    }

                    foreach (CheckConstraint ck in type.CheckConstraints.Where(IsUnchanged))
                    {
                        if (_objectsThatRequireRedefinition.Contains(ck.ID))
                        {
                            typeDiff.CheckConstraintsToCreate.Add(ck);
                            typeDiff.CheckConstraintsToDrop.Add(ck);
                        }
                    }
                }
            }

            foreach (PostgreSQLRangeType type in newDb.RangeTypes.Where(IsUnchanged))
            {
                if (_objectsThatRequireRedefinition.Contains(type.ID))
                {
                    dbDiff.RangeTypesToCreate.Add(type);
                    dbDiff.RangeTypesToDrop.Add(type);
                }
            }

            bool RequiresDefaultRedifinition(PostgreSQLDomainType type)
            {
                return _objectsThatRequireDefaultRedefinition.Contains(type.ID)
                    && type.Default is not null;
            }

            bool AnyUnchangedCheckConstraintRequiresRedefinition(PostgreSQLDomainType type)
            {
                return type.CheckConstraints.Any(x => IsUnchanged(x)
                    && _objectsThatRequireRedefinition.Contains(x.ID));
            }
        }

        void AddForTableObjects()
        {
            Dictionary<Guid, PostgreSQLTableDiff> tableIdToTableDiffMap = dbDiff.ChangedTables
                .ToDictionary(x => x.ID, x => (PostgreSQLTableDiff)x);
            foreach (Table table in newDb.Tables.Where(IsNotAdded))
            {
                if (table.Columns.Any(RequiresDataTypeOrDefaultRedifinition)
                    || AnyUnchangedConstraintRequiresRedefinition(table))
                {
                    PostgreSQLTableDiff tableDiff = tableIdToTableDiffMap.ContainsKey(table.ID)
                        ? tableIdToTableDiffMap[table.ID]
                        : table.CreateEmptyTableDiff();
                    if (!tableIdToTableDiffMap.ContainsKey(table.ID))
                        dbDiff.ChangedTables.Add(tableDiff);

                    Dictionary<Guid, ColumnDiff> columnIdToColumnDiffMap = tableDiff.ColumnsToAlter
                        .ToDictionary(x => x.ID, x => x);
                    foreach (Column column in table.Columns.Where(IsNotAdded))
                    {
                        if (RequiresDataTypeOrDefaultRedifinition(column))
                        {
                            ColumnDiff columnDiff = columnIdToColumnDiffMap.ContainsKey(column.ID)
                                ? columnIdToColumnDiffMap[column.ID]
                                : column.CreateEmptyColumnDiff();
                            if (!columnIdToColumnDiffMap.ContainsKey(column.ID))
                                tableDiff.ColumnsToAlter.Add(columnDiff);

                            if (RequiresDataTypeRedifinition(column))
                                columnDiff.DataTypeToSet = column.DataType;

                            bool defaultChanged = columnDiff.DefaultToSet is not null || columnDiff.DefaultToDrop is not null;
                            if (RequiresDefaultRedifinition(column) && !defaultChanged)
                            {
                                columnDiff.DefaultToSet = column.Default;
                                columnDiff.DefaultToDrop = column.Default;
                            }
                        }
                    }

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

        void AddForProgrammableObjects()
        {
            foreach (PostgreSQLView view in newDb.Views.Where(IsUnchanged))
            {
                if (_objectsThatRequireRedefinition.Contains(view.ID))
                {
                    dbDiff.ViewsToCreate.Add(view);
                    dbDiff.ViewsToDrop.Add(view);
                }
            }
            foreach (PostgreSQLFunction function in newDb.Functions.Where(IsUnchanged))
            {
                if (_objectsThatRequireRedefinition.Contains(function.ID))
                {
                    dbDiff.FunctionsToCreate.Add(function);
                    dbDiff.FunctionsToDrop.Add(function);
                }
            }
            foreach (PostgreSQLProcedure procedure in newDb.Procedures.Where(IsUnchanged))
            {
                if (_objectsThatRequireRedefinition.Contains(procedure.ID))
                {
                    dbDiff.ProceduresToCreate.Add(procedure);
                    dbDiff.ProceduresToDrop.Add(procedure);
                }
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
            PostgreSQLCompositeType x => x.Attributes.Any(a => AnyRequireRedefinition(a.DataType.DependsOn)),
            PostgreSQLDomainType x => AnyRequireRedefinition(x.UnderlyingType.DependsOn),
            PostgreSQLRangeType x => AnyRequireRedefinition(x.Subtype.DependsOn),
            PostgreSQLColumn x => AnyRequireRedefinition(x.DataType.DependsOn),
            PostgreSQLView x => AnyRequireRedefinition(x.CreateStatement.DependsOn),
            PostgreSQLFunction x => AnyRequireRedefinition(x.CreateStatement.DependsOn),
            PostgreSQLProcedure x => AnyRequireRedefinition(x.CreateStatement.DependsOn),
            PrimaryKey x => AnyRequireRedefinition(x.DependsOn),
            UniqueConstraint x => AnyRequireRedefinition(x.DependsOn),
            CheckConstraint x => AnyRequireRedefinition(x.Expression.DependsOn),
            ForeignKey x => AnyRequireRedefinition(x.DependsOn),
            PostgreSQLIndex x => AnyRequireRedefinition(x.DependsOn)
                || x.Expression is not null && AnyRequireRedefinition(x.Expression.DependsOn),
            PostgreSQLTrigger x => AnyRequireRedefinition(x.CreateStatement.DependsOn),
            _ => false,
        };
    }

    private bool DependencyRequiresDefaultRedefinition(DbObject dbObject)
    {
        return dbObject switch
        {
            PostgreSQLDomainType x => x.Default is not null && AnyRequireRedefinition(x.Default.DependsOn),
            PostgreSQLColumn x => x.Default is not null && AnyRequireRedefinition(x.Default.DependsOn),
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

    private PostgreSQLSequenceOptions GetChangedSequenceOptions(
        PostgreSQLSequenceOptions newOptions, PostgreSQLSequenceOptions oldOptions)
    {
        PostgreSQLSequenceOptions changedOptions = null;
        if (!AreEqual(newOptions, oldOptions))
        {
            changedOptions = new PostgreSQLSequenceOptions();
            if (newOptions.StartWith != oldOptions.StartWith)
                changedOptions.StartWith = newOptions.StartWith;
            if (newOptions.IncrementBy != oldOptions.IncrementBy)
                changedOptions.IncrementBy = newOptions.IncrementBy;
            if (newOptions.MinValue != oldOptions.MinValue)
                changedOptions.MinValue = newOptions.MinValue;
            if (newOptions.MaxValue != oldOptions.MaxValue)
                changedOptions.MaxValue = newOptions.MaxValue;
            if (newOptions.Cache != oldOptions.Cache)
                changedOptions.Cache = newOptions.Cache;
            if (newOptions.Cycle != oldOptions.Cycle)
                changedOptions.Cycle = newOptions.Cycle;
        }
        return changedOptions;
    }
}
