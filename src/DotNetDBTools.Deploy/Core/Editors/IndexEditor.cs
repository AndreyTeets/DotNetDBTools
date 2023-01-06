using System;
using System.Collections.Generic;
using DotNetDBTools.Deploy.Core.Queries.DDL;
using DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;
using DotNetDBTools.Models.Core;
using static DotNetDBTools.Deploy.Core.InstanceCreator;

namespace DotNetDBTools.Deploy.Core.Editors;

internal abstract class IndexEditor<
    TInsertDNDBTDbObjectRecordQuery,
    TDeleteDNDBTDbObjectRecordQuery>
    : IIndexEditor
    where TInsertDNDBTDbObjectRecordQuery : InsertDNDBTDbObjectRecordQuery
    where TDeleteDNDBTDbObjectRecordQuery : DeleteDNDBTDbObjectRecordQuery
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
        foreach (Index index in dbDiff.IndexesToDrop)
            DropIndex(index);
    }

    private void CreateIndex(Index index, Table table)
    {
        _queryExecutor.Execute(new CreateIndexQuery(index));
        _queryExecutor.Execute(Create<TInsertDNDBTDbObjectRecordQuery>(index.ID, table.ID, DbObjectType.Index, index.Name));
    }

    private void DropIndex(Index index)
    {
        _queryExecutor.Execute(new DropIndexQuery(index));
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
