using System.Collections.Generic;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Analysis.MSSQL
{
    internal class MSSQLDiffCreator : DiffCreator
    {
        public override DatabaseDiff CreateDatabaseDiff(DatabaseInfo newDatabase, DatabaseInfo oldDatabase)
        {
            MSSQLDatabaseDiff databaseDiff = new()
            {
                NewDatabase = newDatabase,
                OldDatabase = oldDatabase,
                ViewsToCreate = new List<MSSQLViewInfo>(),
                ViewsToDrop = new List<MSSQLViewInfo>(),
                UserDefinedTableTypesToCreate = new List<MSSQLUserDefinedTableTypeInfo>(),
                UserDefinedTableTypesToDrop = new List<MSSQLUserDefinedTableTypeInfo>(),
                FunctionsToCreate = new List<MSSQLFunctionInfo>(),
                FunctionsToDrop = new List<MSSQLFunctionInfo>(),
                ProceduresToCreate = new List<MSSQLProcedureInfo>(),
                ProceduresToDrop = new List<MSSQLProcedureInfo>(),
            };

            BuildTablesDiff<MSSQLTableDiff>(databaseDiff, newDatabase, oldDatabase);
            BuildUserDefinedTypesDiff(databaseDiff, (MSSQLDatabaseInfo)newDatabase, (MSSQLDatabaseInfo)oldDatabase);
            ForeignKeysHelper.BuildAllForeignKeysToBeDroppedAndCreated(databaseDiff);
            return databaseDiff;
        }

        private void BuildUserDefinedTypesDiff(
            MSSQLDatabaseDiff databaseDiff, MSSQLDatabaseInfo newDatabase, MSSQLDatabaseInfo oldDatabase)
        {
            List<MSSQLUserDefinedTypeInfo> addedUserDefinedTypes = null;
            List<MSSQLUserDefinedTypeInfo> removedUserDefinedTypes = null;
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
