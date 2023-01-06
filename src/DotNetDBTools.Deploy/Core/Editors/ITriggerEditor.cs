using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core.Editors;

internal interface ITriggerEditor
{
    public void CreateTriggers(DatabaseDiff dbDiff);
    public void DropTriggers(DatabaseDiff dbDiff);
}
