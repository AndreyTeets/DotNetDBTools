using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis.Extensions;
using DotNetDBTools.Deploy.Core.Queries;
using DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;
using DotNetDBTools.Generation.Core;
using DotNetDBTools.Models.Core;
using static DotNetDBTools.Deploy.Core.InstanceCreator;

namespace DotNetDBTools.Deploy.Core;

internal abstract class ScriptExecutor<
    TInsertDNDBTScriptExecutionRecordQuery,
    TDeleteDNDBTScriptExecutionRecordQuery>
    : IScriptExecutor
    where TInsertDNDBTScriptExecutionRecordQuery : InsertDNDBTScriptExecutionRecordQuery
    where TDeleteDNDBTScriptExecutionRecordQuery : DeleteDNDBTScriptExecutionRecordQuery
{
    protected readonly IQueryExecutor QueryExecutor;
    protected virtual bool AppendSemicolon => false;

    protected ScriptExecutor(IQueryExecutor queryExecutor)
    {
        QueryExecutor = queryExecutor;
    }

    public void ExecuteScripts(DatabaseDiff dbDiff, ScriptKind scriptKind)
    {
        foreach (Script script in GetOrderedByNameScriptsToExecute(dbDiff, scriptKind))
            ExecuteScript(script, dbDiff.OldDatabase.Version);

        foreach (Script script in GetScriptsToAddRecordWithoutExecution(dbDiff, scriptKind))
            AddScriptExecutionRecord(script, -1);
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

    private IEnumerable<Script> GetScriptsToAddRecordWithoutExecution(DatabaseDiff dbDiff, ScriptKind scriptKind)
    {
        return dbDiff.AddedScripts
            .Where(x => x.Kind == scriptKind)
            .Except(GetOrderedByNameScriptsToExecute(dbDiff, scriptKind));
    }

    private void ExecuteScript(Script script, long executedOnDbVersion)
    {
        string scriptCode = AppendSemicolon ? script.GetCode().AppendSemicolonIfAbsent() : script.GetCode();
        QueryExecutor.Execute(new GenericQuery(scriptCode));
        AddScriptExecutionRecord(script, executedOnDbVersion);
    }

    private void AddScriptExecutionRecord(Script script, long executedOnDbVersion)
    {
        QueryExecutor.Execute(Create<TInsertDNDBTScriptExecutionRecordQuery>(script, executedOnDbVersion));
    }

    private void DeleteScriptExecutionRecord(Script script)
    {
        QueryExecutor.Execute(Create<TDeleteDNDBTScriptExecutionRecordQuery>(script.ID));
    }
}
