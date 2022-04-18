using System;
using System.Collections.Generic;
using DotNetDBTools.Deploy.Common.Queries.DDL;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;
using DotNetDBTools.Models.Core;
using static DotNetDBTools.Deploy.Core.InstanceCreator;

namespace DotNetDBTools.Deploy.Common.Editors;

internal abstract class IndexEditor<
    TInsertDNDBTDbObjectRecordQuery,
    TDeleteDNDBTDbObjectRecordQuery,
    TCreateIndexQuery,
    TDropIndexQuery>
    : IIndexEditor
    where TInsertDNDBTDbObjectRecordQuery : InsertDNDBTDbObjectRecordQuery
    where TDeleteDNDBTDbObjectRecordQuery : DeleteDNDBTDbObjectRecordQuery
    where TCreateIndexQuery : CreateIndexQuery
    where TDropIndexQuery : DropIndexQuery
{
    private readonly IQueryExecutor _queryExecutor;

    protected IndexEditor(IQueryExecutor queryExecutor)
    {
        _queryExecutor = queryExecutor;
    }

    public void CreateIndexes(DatabaseDiff dbDiff)
    {
        Dictionary<Guid, Table> indexToTableMap = CreateIndexToTableMap(dbDiff.NewDatabase.Tables);
        foreach (Index index in dbDiff.IndexesToCreate)
            CreateIndex(index, indexToTableMap[index.ID]);
    }

    public void DropIndexes(DatabaseDiff dbDiff)
    {
        Dictionary<Guid, Table> indexToTableMap = CreateIndexToTableMap(dbDiff.OldDatabase.Tables);
        foreach (Index index in dbDiff.IndexesToDrop)
            DropIndex(index, indexToTableMap[index.ID]);
    }

    private void CreateIndex(Index index, Table table)
    {
        _queryExecutor.Execute(Create<TCreateIndexQuery>(index, table));
        _queryExecutor.Execute(Create<TInsertDNDBTDbObjectRecordQuery>(index.ID, table.ID, DbObjectType.Index, index.Name));
    }

    private void DropIndex(Index index, Table table)
    {
        _queryExecutor.Execute(Create<TDropIndexQuery>(index, table));
        _queryExecutor.Execute(Create<TDeleteDNDBTDbObjectRecordQuery>(index.ID));
    }

    private static Dictionary<Guid, Table> CreateIndexToTableMap(IEnumerable<Table> tables)
    {
        Dictionary<Guid, Table> indexToTableMap = new();
        foreach (Table table in tables)
        {
            foreach (Index index in table.Indexes)
                indexToTableMap.Add(index.ID, table);
        }
        return indexToTableMap;
    }
}
