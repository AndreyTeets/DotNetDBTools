using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Common.Editors
{
    internal interface IForeignKeyEditor
    {
        public void CreateForeignKeys(DatabaseDiff dbDiff);
        public void DropForeignKeys(DatabaseDiff dbDiff);
    }
}
