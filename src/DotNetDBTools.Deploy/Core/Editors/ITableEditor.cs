using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core.Editors
{
    internal interface ITableEditor
    {
        public void CreateTable(Table table);
        public void DropTable(Table table);
        public void AlterTable(TableDiff tableDiff);
    }
}
