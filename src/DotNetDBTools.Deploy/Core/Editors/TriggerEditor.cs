using DotNetDBTools.Deploy.Core.Queries.DDL;
using DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;
using DotNetDBTools.Generation.Core;
using DotNetDBTools.Models.Core;
using static DotNetDBTools.Deploy.Core.InstanceCreator;

namespace DotNetDBTools.Deploy.Core.Editors;

internal abstract class TriggerEditor<
    TInsertDNDBTDbObjectRecordQuery,
    TDeleteDNDBTDbObjectRecordQuery>
    : ITriggerEditor
    where TInsertDNDBTDbObjectRecordQuery : InsertDNDBTDbObjectRecordQuery
    where TDeleteDNDBTDbObjectRecordQuery : DeleteDNDBTDbObjectRecordQuery
{
    private readonly IQueryExecutor _queryExecutor;

    protected TriggerEditor(IQueryExecutor queryExecutor)
    {
        _queryExecutor = queryExecutor;
    }

    public void CreateTriggers(DatabaseDiff dbDiff)
    {
        foreach (Trigger trigger in dbDiff.TriggersToCreate)
            CreateTrigger(trigger);
    }

    public void DropTriggers(DatabaseDiff dbDiff)
    {
        foreach (Trigger trigger in dbDiff.TriggersToDrop)
            DropTrigger(trigger);
    }

    private void CreateTrigger(Trigger trg)
    {
        _queryExecutor.Execute(new CreateTriggerQuery(trg));
        _queryExecutor.Execute(Create<TInsertDNDBTDbObjectRecordQuery>(trg.ID, trg.Parent.ID, DbObjectType.Trigger, trg.Name, trg.GetCreateStatement()));
    }

    private void DropTrigger(Trigger trg)
    {
        _queryExecutor.Execute(new DropTriggerQuery(trg));
        _queryExecutor.Execute(Create<TDeleteDNDBTDbObjectRecordQuery>(trg.ID));
    }
}
