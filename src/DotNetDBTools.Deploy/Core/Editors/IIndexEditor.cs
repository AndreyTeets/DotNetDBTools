using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core.Editors;

internal interface IIndexEditor
{
    public void CreateIndexes(DatabaseDiff dbDiff);
    public void DropIndexes(DatabaseDiff dbDiff);
}
