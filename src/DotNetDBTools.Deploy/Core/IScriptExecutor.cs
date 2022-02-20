using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core.Editors;

internal interface IScriptExecutor
{
    public void ExecuteScripts(DatabaseDiff dbDiff, ScriptKind scriptKind);
    public void DeleteRemovedScriptsExecutionRecords(DatabaseDiff dbDiff);
}
