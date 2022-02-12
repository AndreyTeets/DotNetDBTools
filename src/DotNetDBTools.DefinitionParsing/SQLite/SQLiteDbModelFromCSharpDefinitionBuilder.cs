using DotNetDBTools.Definition.Core;
using DotNetDBTools.DefinitionParsing.Common;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.DefinitionParsing.SQLite
{
    internal class SQLiteDbModelFromCSharpDefinitionBuilder : DbModelFromCSharpDefinitionBuilder<
        SQLiteDatabase,
        SQLiteTable,
        SQLiteView,
        Models.Core.Column>
    {
        public SQLiteDbModelFromCSharpDefinitionBuilder() : base(
            new SQLiteDataTypeMapper(),
            new SpecificDbmsDbObjectCodeMapper(),
            new SQLiteDefaultValueMapper())
        {
        }

        protected override string GetOnUpdateActionName(BaseForeignKey fk) =>
            MapFKActionNameFromDefinitionToModel(((Definition.SQLite.ForeignKey)fk).OnUpdate.ToString());
        protected override string GetOnDeleteActionName(BaseForeignKey fk) =>
            MapFKActionNameFromDefinitionToModel(((Definition.SQLite.ForeignKey)fk).OnDelete.ToString());
    }
}
