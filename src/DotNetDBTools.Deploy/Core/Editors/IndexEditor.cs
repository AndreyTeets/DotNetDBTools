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
        foreach (Index index in dbDiff.IndexesToCreate)
            CreateIndex(index);
    }

    public void DropIndexes(DatabaseDiff dbDiff)
    {
        foreach (Index index in dbDiff.IndexesToDrop)
            DropIndex(index);
    }

    private void CreateIndex(Index index)
    {
        _queryExecutor.Execute(new CreateIndexQuery(index));
        _queryExecutor.Execute(Create<TInsertDNDBTDbObjectRecordQuery>(index.ID, index.Parent.ID, DbObjectType.Index, index.Name));
    }

    private void DropIndex(Index index)
    {
        _queryExecutor.Execute(new DropIndexQuery(index));
        _queryExecutor.Execute(Create<TDeleteDNDBTDbObjectRecordQuery>(index.ID));
    }
}
