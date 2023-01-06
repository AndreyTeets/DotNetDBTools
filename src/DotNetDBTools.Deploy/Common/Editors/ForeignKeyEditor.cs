using System;
using System.Collections.Generic;
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
        Dictionary<Guid, Table> fkToTableMap = CreateFKToTableMap(dbDiff.NewDatabase.Tables);
        foreach (ForeignKey fk in dbDiff.AllForeignKeysToCreate)
            CreateForeignKey(fk, fkToTableMap[fk.ID]);
    }

    public void DropForeignKeys(DatabaseDiff dbDiff)
    {
        foreach (ForeignKey fk in dbDiff.AllForeignKeysToDrop)
            DropForeignKey(fk);
    }

    public void CreateForeignKey(ForeignKey fk, Table table)
    {
        _queryExecutor.Execute(Create<TCreateForeignKeyQuery>(fk));
        _queryExecutor.Execute(Create<TInsertDNDBTDbObjectRecordQuery>(fk.ID, table.ID, DbObjectType.ForeignKey, fk.Name));
    }

    public void DropForeignKey(ForeignKey fk)
    {
        _queryExecutor.Execute(Create<TDropForeignKeyQuery>(fk));
        _queryExecutor.Execute(Create<TDeleteDNDBTDbObjectRecordQuery>(fk.ID));
    }

    private static Dictionary<Guid, Table> CreateFKToTableMap(IEnumerable<Table> tables)
    {
        Dictionary<Guid, Table> fkToTableMap = new();
        foreach (Table table in tables)
        {
            foreach (ForeignKey fk in table.ForeignKeys)
                fkToTableMap.Add(fk.ID, table);
        }
        return fkToTableMap;
    }
}
