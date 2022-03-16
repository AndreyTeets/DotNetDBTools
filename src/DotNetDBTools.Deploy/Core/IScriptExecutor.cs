using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core;

internal interface IScriptExecutor
{
    public void ExecuteScripts(DatabaseDiff dbDiff, ScriptKind scriptKind);
    public void DeleteRemovedScriptsExecutionRecords(DatabaseDiff dbDiff);
}
