using System.Collections.Generic;
using DotNetDBTools.Analysis.Common;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;
using DotNetDBTools.Models.MSSQL.UserDefinedTypes;

namespace DotNetDBTools.Analysis.MSSQL;

internal class MSSQLDiffCreator : DiffCreator
{
    private readonly HashSet<string> _changedUserDefinedTypesNames = new();

    public override DatabaseDiff CreateDatabaseDiff(Database newDatabase, Database oldDatabase)
    {
        MSSQLDatabaseDiff dbDiff = new()
        {
            NewDatabase = newDatabase,
            OldDatabase = oldDatabase,
            ViewsToCreate = new List<View>(),
            ViewsToDrop = new List<View>(),
            UserDefinedTableTypesToCreate = new List<MSSQLUserDefinedTableType>(),
            UserDefinedTableTypesToDrop = new List<MSSQLUserDefinedTableType>(),
            FunctionsToCreate = new List<MSSQLFunction>(),
            FunctionsToDrop = new List<MSSQLFunction>(),
            ProceduresToCreate = new List<MSSQLProcedure>(),
            ProceduresToDrop = new List<MSSQLProcedure>(),
        };

        BuildUserDefinedTypesDiff(dbDiff);
        FillChangedUserDefinedTypesNames(dbDiff);

        BuildTablesDiff<MSSQLTableDiff>(dbDiff);
        IndexesHelper.BuildAllDbIndexesToBeDroppedAndCreated(dbDiff);
        TriggersHelper.BuildAllDbTriggersToBeDroppedAndCreated(dbDiff);
        ForeignKeysHelper.BuildAllForeignKeysToBeDroppedAndCreated(dbDiff);

        BuildViewsDiff(dbDiff);
        BuildScriptsDiff(dbDiff);
        return dbDiff;
    }

    protected override void SetDataTypeChanged(ColumnDiff columnDiff)
    {
        base.SetDataTypeChanged(columnDiff);
        if (_changedUserDefinedTypesNames.Contains(columnDiff.NewColumn.DataType.Name))
            columnDiff.DataTypeChanged = true;
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

    private void FillChangedUserDefinedTypesNames(MSSQLDatabaseDiff dbDiff)
    {
        _changedUserDefinedTypesNames.Clear();
        foreach (MSSQLUserDefinedTypeDiff typeDiff in dbDiff.ChangedUserDefinedTypes)
            _changedUserDefinedTypesNames.Add(typeDiff.NewUserDefinedType.Name);
    }
}
