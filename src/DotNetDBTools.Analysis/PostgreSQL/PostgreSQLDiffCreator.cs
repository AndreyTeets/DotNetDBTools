using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;
using DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;

namespace DotNetDBTools.Analysis.PostgreSQL
{
    internal class PostgreSQLDiffCreator : DiffCreator
    {
        private readonly HashSet<string> _changedUserDefinedTypesNames = new();

        public override DatabaseDiff CreateDatabaseDiff(Database newDatabase, Database oldDatabase)
        {
            PostgreSQLDatabaseDiff databaseDiff = new()
            {
                NewDatabase = newDatabase,
                OldDatabase = oldDatabase,
                ViewsToCreate = new List<PostgreSQLView>(),
                ViewsToDrop = new List<PostgreSQLView>(),
                FunctionsToCreate = new List<PostgreSQLFunction>(),
                FunctionsToDrop = new List<PostgreSQLFunction>(),
                ProceduresToCreate = new List<PostgreSQLProcedure>(),
                ProceduresToDrop = new List<PostgreSQLProcedure>(),
            };

            BuildCompositeTypesDiff(databaseDiff, (PostgreSQLDatabase)newDatabase, (PostgreSQLDatabase)oldDatabase);
            BuildDomainTypesDiff(databaseDiff, (PostgreSQLDatabase)newDatabase, (PostgreSQLDatabase)oldDatabase);
            BuildEnumTypesDiff(databaseDiff, (PostgreSQLDatabase)newDatabase, (PostgreSQLDatabase)oldDatabase);
            BuildRangeTypesDiff(databaseDiff, (PostgreSQLDatabase)newDatabase, (PostgreSQLDatabase)oldDatabase);
            FillChangedUserDefinedTypesNames(databaseDiff);

            BuildTablesDiff<PostgreSQLTableDiff>(databaseDiff, newDatabase, oldDatabase);
            ForeignKeysHelper.BuildAllForeignKeysToBeDroppedAndCreated(databaseDiff);

            BuildViewsDiff(databaseDiff);
            return databaseDiff;
        }

        protected override bool AdditionalItemsNonEqualityConditionIsTrue<TItem>(TItem newItem, TItem oldItem)
        {
            if (newItem is Table newTable &&
                newTable.Columns.Any(c => _changedUserDefinedTypesNames.Contains(c.DataType.Name)))
            {
                return true;
            }

            if (newItem is Column newColumn &&
                _changedUserDefinedTypesNames.Contains(newColumn.DataType.Name))
            {
                return true;
            }

            return false;
        }

        private void BuildCompositeTypesDiff(
            PostgreSQLDatabaseDiff databaseDiff, PostgreSQLDatabase newDatabase, PostgreSQLDatabase oldDatabase)
        {
            List<PostgreSQLCompositeType> addedTypes = null;
            List<PostgreSQLCompositeType> removedTypes = null;
            List<PostgreSQLCompositeTypeDiff> changedTypes = new();
            FillAddedAndRemovedItemsAndApplyActionToChangedItems(
                newDatabase.CompositeTypes, oldDatabase.CompositeTypes,
                ref addedTypes, ref removedTypes,
                (newType, oldType) =>
                {
                    PostgreSQLCompositeTypeDiff typeDiff = new()
                    {
                        NewCompositeType = newType,
                        OldCompositeType = oldType,
                    };
                    changedTypes.Add(typeDiff);
                });

            databaseDiff.AddedCompositeTypes = addedTypes;
            databaseDiff.RemovedCompositeTypes = removedTypes;
            databaseDiff.ChangedCompositeTypes = changedTypes;
        }

        private void BuildDomainTypesDiff(
            PostgreSQLDatabaseDiff databaseDiff, PostgreSQLDatabase newDatabase, PostgreSQLDatabase oldDatabase)
        {
            List<PostgreSQLDomainType> addedTypes = null;
            List<PostgreSQLDomainType> removedTypes = null;
            List<PostgreSQLDomainTypeDiff> changedTypes = new();
            FillAddedAndRemovedItemsAndApplyActionToChangedItems(
                newDatabase.DomainTypes, oldDatabase.DomainTypes,
                ref addedTypes, ref removedTypes,
                (newType, oldType) =>
                {
                    PostgreSQLDomainTypeDiff typeDiff = new()
                    {
                        NewDomainType = newType,
                        OldDomainType = oldType,
                    };
                    changedTypes.Add(typeDiff);
                });

            databaseDiff.AddedDomainTypes = addedTypes;
            databaseDiff.RemovedDomainTypes = removedTypes;
            databaseDiff.ChangedDomainTypes = changedTypes;
        }

        private void BuildEnumTypesDiff(
            PostgreSQLDatabaseDiff databaseDiff, PostgreSQLDatabase newDatabase, PostgreSQLDatabase oldDatabase)
        {
            List<PostgreSQLEnumType> addedEnumTypes = null;
            List<PostgreSQLEnumType> removedEnumTypes = null;
            List<PostgreSQLEnumTypeDiff> changedEnumTypes = new();
            FillAddedAndRemovedItemsAndApplyActionToChangedItems(
                newDatabase.EnumTypes, oldDatabase.EnumTypes,
                ref addedEnumTypes, ref removedEnumTypes,
                (newType, oldType) =>
                {
                    PostgreSQLEnumTypeDiff typeDiff = new()
                    {
                        NewEnumType = newType,
                        OldEnumType = oldType,
                    };
                    changedEnumTypes.Add(typeDiff);
                });

            databaseDiff.AddedEnumTypes = addedEnumTypes;
            databaseDiff.RemovedEnumTypes = removedEnumTypes;
            databaseDiff.ChangedEnumTypes = changedEnumTypes;
        }

        private void BuildRangeTypesDiff(
            PostgreSQLDatabaseDiff databaseDiff, PostgreSQLDatabase newDatabase, PostgreSQLDatabase oldDatabase)
        {
            List<PostgreSQLRangeType> addedRangeTypes = null;
            List<PostgreSQLRangeType> removedRangeTypes = null;
            List<PostgreSQLRangeTypeDiff> changedRangeTypes = new();
            FillAddedAndRemovedItemsAndApplyActionToChangedItems(
                newDatabase.RangeTypes, oldDatabase.RangeTypes,
                ref addedRangeTypes, ref removedRangeTypes,
                (newType, oldType) =>
                {
                    PostgreSQLRangeTypeDiff typeDiff = new()
                    {
                        NewRangeType = newType,
                        OldRangeType = oldType,
                    };
                    changedRangeTypes.Add(typeDiff);
                });

            databaseDiff.AddedRangeTypes = addedRangeTypes;
            databaseDiff.RemovedRangeTypes = removedRangeTypes;
            databaseDiff.ChangedRangeTypes = changedRangeTypes;
        }

        private void FillChangedUserDefinedTypesNames(PostgreSQLDatabaseDiff dbDiff)
        {
            _changedUserDefinedTypesNames.Clear();
            foreach (PostgreSQLCompositeTypeDiff typeDiff in dbDiff.ChangedCompositeTypes)
                _changedUserDefinedTypesNames.Add(typeDiff.NewCompositeType.Name);
            foreach (PostgreSQLDomainTypeDiff typeDiff in dbDiff.ChangedDomainTypes)
                _changedUserDefinedTypesNames.Add(typeDiff.NewDomainType.Name);
            foreach (PostgreSQLEnumTypeDiff typeDiff in dbDiff.ChangedEnumTypes)
                _changedUserDefinedTypesNames.Add(typeDiff.NewEnumType.Name);
            foreach (PostgreSQLRangeTypeDiff typeDiff in dbDiff.ChangedRangeTypes)
                _changedUserDefinedTypesNames.Add(typeDiff.NewRangeType.Name);
        }
    }
}
