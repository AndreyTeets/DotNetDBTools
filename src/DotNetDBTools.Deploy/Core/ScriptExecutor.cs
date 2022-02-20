using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Deploy.Core.Queries;
using DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;
using DotNetDBTools.Models.Core;
using static DotNetDBTools.Deploy.Core.InstanceCreator;

namespace DotNetDBTools.Deploy.Core.Editors;

internal abstract class ScriptExecutor<
    TInsertDNDBTScriptExecutionRecordQuery,
    TDeleteDNDBTScriptExecutionRecordQuery>
    : IScriptExecutor
    where TInsertDNDBTScriptExecutionRecordQuery : InsertDNDBTScriptExecutionRecordQuery
    where TDeleteDNDBTScriptExecutionRecordQuery : DeleteDNDBTScriptExecutionRecordQuery
{
    protected readonly IQueryExecutor QueryExecutor;

    protected ScriptExecutor(IQueryExecutor queryExecutor)
    {
        QueryExecutor = queryExecutor;
    }

    public void ExecuteScripts(DatabaseDiff dbDiff, ScriptKind scriptKind)
    {
        foreach (Script script in GetOrderedByNameScriptsToExecute(dbDiff, scriptKind))
            ExecuteScript(script, dbDiff.OldDatabase.Version);
    }

    public void DeleteRemovedScriptsExecutionRecords(DatabaseDiff dbDiff)
    {
        foreach (Script script in dbDiff.RemovedScripts)
            DeleteScriptExecutionRecord(script);
    }

    private IEnumerable<Script> GetOrderedByNameScriptsToExecute(DatabaseDiff dbDiff, ScriptKind scriptKind)
    {
        return dbDiff.AddedScripts
            .Where(x => x.Kind == scriptKind &&
                dbDiff.OldDatabase.Version >= x.MinDbVersionToExecute &&
                dbDiff.OldDatabase.Version <= x.MaxDbVersionToExecute)
            .OrderByName();
    }

    private void ExecuteScript(Script script, long oldDbVersion)
    {
        QueryExecutor.Execute(new GenericQuery($"{script.GetCode()}"));
        QueryExecutor.Execute(Create<TInsertDNDBTScriptExecutionRecordQuery>(script, oldDbVersion));
    }

    private void DeleteScriptExecutionRecord(Script script)
    {
        QueryExecutor.Execute(Create<TDeleteDNDBTScriptExecutionRecordQuery>(script.ID));
    }
}
