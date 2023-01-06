using System;
using System.Collections.Generic;
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
        Dictionary<Guid, Table> triggerToTableMap = CreateTriggerToTableMap(dbDiff.NewDatabase.Tables);
        foreach (Trigger trigger in dbDiff.TriggersToCreate)
            CreateTrigger(trigger, triggerToTableMap[trigger.ID]);
    }

    public void DropTriggers(DatabaseDiff dbDiff)
    {
        foreach (Trigger trigger in dbDiff.TriggersToDrop)
            DropTrigger(trigger);
    }

    private void CreateTrigger(Trigger trg, Table table)
    {
        _queryExecutor.Execute(new CreateTriggerQuery(trg));
        _queryExecutor.Execute(Create<TInsertDNDBTDbObjectRecordQuery>(trg.ID, table.ID, DbObjectType.Trigger, trg.Name, trg.GetCode()));
    }

    private void DropTrigger(Trigger trg)
    {
        _queryExecutor.Execute(new DropTriggerQuery(trg));
        _queryExecutor.Execute(Create<TDeleteDNDBTDbObjectRecordQuery>(trg.ID));
    }

    private static Dictionary<Guid, Table> CreateTriggerToTableMap(IEnumerable<Table> tables)
    {
        Dictionary<Guid, Table> triggerToTableMap = new();
        foreach (Table table in tables)
        {
            foreach (Trigger trg in table.Triggers)
                triggerToTableMap.Add(trg.ID, table);
        }
        return triggerToTableMap;
    }
}
