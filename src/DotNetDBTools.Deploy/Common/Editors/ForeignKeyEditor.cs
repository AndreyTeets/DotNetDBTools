using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Deploy.Common.Queries.DDL;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;
using DotNetDBTools.Models.Core;
using static DotNetDBTools.Deploy.Core.InstanceCreator;

namespace DotNetDBTools.Deploy.Common.Editors;

internal abstract class ForeignKeyEditor<
    TInsertDNDBTDbObjectRecordQuery,
    TDeleteDNDBTDbObjectRecordQuery,
    TCreateForeignKeyQuery,
    TDropForeignKeyQuery>
    : IForeignKeyEditor
    where TInsertDNDBTDbObjectRecordQuery : InsertDNDBTDbObjectRecordQuery
    where TDeleteDNDBTDbObjectRecordQuery : DeleteDNDBTDbObjectRecordQuery
    where TCreateForeignKeyQuery : CreateForeignKeyQuery
    where TDropForeignKeyQuery : DropForeignKeyQuery
{
    private readonly IQueryExecutor _queryExecutor;

    protected ForeignKeyEditor(IQueryExecutor queryExecutor)
    {
        _queryExecutor = queryExecutor;
    }

    public void CreateForeignKeys(DatabaseDiff dbDiff)
    {
        foreach (ForeignKey fk in GetAllForeignKeysToCreate(dbDiff))
            CreateForeignKey(fk);
    }

    public void DropForeignKeys(DatabaseDiff dbDiff)
    {
        foreach (ForeignKey fk in GetAllForeignKeysToDrop(dbDiff))
            DropForeignKey(fk);
    }

    private static IEnumerable<ForeignKey> GetAllForeignKeysToCreate(DatabaseDiff dbDiff)
    {
        List<ForeignKey> allForeignKeysToCreate = new();
        foreach (IEnumerable<ForeignKey> addedTableForeignKeys in dbDiff.AddedTables.Select(t => t.ForeignKeys))
            allForeignKeysToCreate.AddRange(addedTableForeignKeys);
        foreach (TableDiff tableDiff in dbDiff.ChangedTables)
            allForeignKeysToCreate.AddRange(tableDiff.ForeignKeysToCreate);
        allForeignKeysToCreate.AddRange(dbDiff.UnchangedForeignKeysToRecreateBecauseOfDeps);
        return allForeignKeysToCreate;
    }

    private static IEnumerable<ForeignKey> GetAllForeignKeysToDrop(DatabaseDiff dbDiff)
    {
        List<ForeignKey> allForeignKeysToDrop = new();
        foreach (IEnumerable<ForeignKey> addedTableForeignKeys in dbDiff.RemovedTables.Select(t => t.ForeignKeys))
            allForeignKeysToDrop.AddRange(addedTableForeignKeys);
        foreach (TableDiff tableDiff in dbDiff.ChangedTables)
            allForeignKeysToDrop.AddRange(tableDiff.ForeignKeysToDrop);
        allForeignKeysToDrop.AddRange(dbDiff.UnchangedForeignKeysToRecreateBecauseOfDeps);
        return allForeignKeysToDrop;
    }

    private void CreateForeignKey(ForeignKey fk)
    {
        _queryExecutor.Execute(Create<TCreateForeignKeyQuery>(fk));
        _queryExecutor.Execute(Create<TInsertDNDBTDbObjectRecordQuery>(fk.ID, fk.Parent.ID, DbObjectType.ForeignKey, fk.Name));
    }

    private void DropForeignKey(ForeignKey fk)
    {
        _queryExecutor.Execute(Create<TDropForeignKeyQuery>(fk));
        _queryExecutor.Execute(Create<TDeleteDNDBTDbObjectRecordQuery>(fk.ID));
    }
}
