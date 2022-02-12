using System;
using System.Collections.Generic;
using DotNetDBTools.Deploy.Common.Queries.DDL;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;
using DotNetDBTools.Models.Core;
using static DotNetDBTools.Deploy.Core.InstanceCreator;

namespace DotNetDBTools.Deploy.Common.Editors
{
    internal abstract class TriggerEditor<
        TInsertDNDBTSysInfoQuery,
        TDeleteDNDBTSysInfoQuery,
        TCreateTriggerQuery,
        TDropTriggerQuery>
        : ITriggerEditor
        where TInsertDNDBTSysInfoQuery : InsertDNDBTSysInfoQuery
        where TDeleteDNDBTSysInfoQuery : DeleteDNDBTSysInfoQuery
        where TCreateTriggerQuery : CreateTriggerQuery
        where TDropTriggerQuery : DropTriggerQuery
    {
        protected readonly IQueryExecutor QueryExecutor;

        protected TriggerEditor(IQueryExecutor queryExecutor)
        {
            QueryExecutor = queryExecutor;
        }

        public void CreateTriggers(DatabaseDiff dbDiff)
        {
            Dictionary<Guid, Table> triggerToTableMap = CreateTriggerToTableMap(dbDiff.NewDatabase.Tables);
            foreach (Trigger trigger in dbDiff.TriggersToCreate)
                CreateTrigger(trigger, triggerToTableMap[trigger.ID]);
        }

        public void DropTriggers(DatabaseDiff dbDiff)
        {
            Dictionary<Guid, Table> triggerToTableMap = CreateTriggerToTableMap(dbDiff.OldDatabase.Tables);
            foreach (Trigger trigger in dbDiff.TriggersToDrop)
                DropTrigger(trigger, triggerToTableMap[trigger.ID]);
        }

        private void CreateTrigger(Trigger trg, Table table)
        {
            QueryExecutor.Execute(Create<TCreateTriggerQuery>(trg));
            QueryExecutor.Execute(Create<TInsertDNDBTSysInfoQuery>(trg.ID, table.ID, DbObjectType.Trigger, trg.Name, trg.GetCode()));
        }

        private void DropTrigger(Trigger trg, Table table)
        {
            QueryExecutor.Execute(Create<TDropTriggerQuery>(trg, table));
            QueryExecutor.Execute(Create<TDeleteDNDBTSysInfoQuery>(trg.ID));
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
}
