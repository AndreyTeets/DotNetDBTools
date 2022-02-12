using System;
using System.Collections.Generic;
using DotNetDBTools.Deploy.Common.Queries.DDL;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;
using DotNetDBTools.Models.Core;
using static DotNetDBTools.Deploy.Core.InstanceCreator;

namespace DotNetDBTools.Deploy.Common.Editors;

internal abstract class ForeignKeyEditor<
    TInsertDNDBTSysInfoQuery,
    TDeleteDNDBTSysInfoQuery,
    TCreateForeignKeyQuery,
    TDropForeignKeyQuery>
    : IForeignKeyEditor
    where TInsertDNDBTSysInfoQuery : InsertDNDBTSysInfoQuery
    where TDeleteDNDBTSysInfoQuery : DeleteDNDBTSysInfoQuery
    where TCreateForeignKeyQuery : CreateForeignKeyQuery
    where TDropForeignKeyQuery : DropForeignKeyQuery
{
    protected readonly IQueryExecutor QueryExecutor;

    protected ForeignKeyEditor(IQueryExecutor queryExecutor)
    {
        QueryExecutor = queryExecutor;
    }

    public void CreateForeignKeys(DatabaseDiff dbDiff)
    {
        Dictionary<Guid, Table> fkToTableMap = CreateFKToTableMap(dbDiff.NewDatabase.Tables);
        foreach (ForeignKey fk in dbDiff.AllForeignKeysToCreate)
            CreateForeignKey(fk, fkToTableMap[fk.ID]);
    }

    public void DropForeignKeys(DatabaseDiff dbDiff)
    {
        Dictionary<Guid, Table> fkToTableMap = CreateFKToTableMap(dbDiff.OldDatabase.Tables);
        foreach (ForeignKey fk in dbDiff.AllForeignKeysToDrop)
            DropForeignKey(fk, fkToTableMap[fk.ID]);
    }

    public void CreateForeignKey(ForeignKey fk, Table table)
    {
        QueryExecutor.Execute(Create<TCreateForeignKeyQuery>(fk, table.Name));
        QueryExecutor.Execute(Create<TInsertDNDBTSysInfoQuery>(fk.ID, table.ID, DbObjectType.ForeignKey, fk.Name));
    }

    public void DropForeignKey(ForeignKey fk, Table table)
    {
        QueryExecutor.Execute(Create<TDropForeignKeyQuery>(fk, table.Name));
        QueryExecutor.Execute(Create<TDeleteDNDBTSysInfoQuery>(fk.ID));
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
