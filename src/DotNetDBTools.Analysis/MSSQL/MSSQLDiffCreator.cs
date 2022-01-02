using System.Collections.Generic;
using DotNetDBTools.Analysis.Common;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Analysis.MSSQL
{
    internal class MSSQLDiffCreator : DiffCreator
    {
        public override DatabaseDiff CreateDatabaseDiff(Database newDatabase, Database oldDatabase)
        {
            MSSQLDatabaseDiff databaseDiff = new()
            {
                NewDatabase = newDatabase,
                OldDatabase = oldDatabase,
                ViewsToCreate = new List<MSSQLView>(),
                ViewsToDrop = new List<MSSQLView>(),
                UserDefinedTableTypesToCreate = new List<MSSQLUserDefinedTableType>(),
                UserDefinedTableTypesToDrop = new List<MSSQLUserDefinedTableType>(),
                FunctionsToCreate = new List<MSSQLFunction>(),
                FunctionsToDrop = new List<MSSQLFunction>(),
                ProceduresToCreate = new List<MSSQLProcedure>(),
                ProceduresToDrop = new List<MSSQLProcedure>(),
            };

            BuildUserDefinedTypesDiff(databaseDiff, (MSSQLDatabase)newDatabase, (MSSQLDatabase)oldDatabase);

            BuildTablesDiff<MSSQLTableDiff>(databaseDiff, newDatabase, oldDatabase);
            IndexesHelper.BuildAllDbIndexesToBeDroppedAndCreated(databaseDiff);
            TriggersHelper.BuildAllDbTriggersToBeDroppedAndCreated(databaseDiff);
            ForeignKeysHelper.BuildAllForeignKeysToBeDroppedAndCreated(databaseDiff);

            BuildViewsDiff(databaseDiff);
            return databaseDiff;
        }

        private void BuildUserDefinedTypesDiff(
            MSSQLDatabaseDiff databaseDiff, MSSQLDatabase newDatabase, MSSQLDatabase oldDatabase)
        {
            List<MSSQLUserDefinedType> addedUserDefinedTypes = null;
            List<MSSQLUserDefinedType> removedUserDefinedTypes = null;
            List<MSSQLUserDefinedTypeDiff> changedUserDefinedTypes = new();
            FillAddedAndRemovedItemsAndApplyActionToChangedItems(
                newDatabase.UserDefinedTypes, oldDatabase.UserDefinedTypes,
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

            databaseDiff.AddedUserDefinedTypes = addedUserDefinedTypes;
            databaseDiff.RemovedUserDefinedTypes = removedUserDefinedTypes;
            databaseDiff.ChangedUserDefinedTypes = changedUserDefinedTypes;
        }
    }
}
