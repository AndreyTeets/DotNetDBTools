using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core.Editors;

internal interface ITableEditor
{
    public void CreateTables(DatabaseDiff dbDiff);
    public void DropTables(DatabaseDiff dbDiff);
    public void AlterTables(DatabaseDiff dbDiff);
}
