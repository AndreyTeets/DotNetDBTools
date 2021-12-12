using DotNetDBTools.Definition.Core;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.DefinitionParsing.SQLite
{
    internal class SQLiteDatabaseModelBuilder : DatabaseModelBuilder<
        SQLiteDatabase,
        SQLiteTable,
        SQLiteView,
        Models.Core.Column>
    {
        public SQLiteDatabaseModelBuilder()
            : base(new SQLiteDataTypeMapper(), new SQLiteDefaultValueMapper())
        {
        }

        protected override string GetOnUpdateActionName(BaseForeignKey fk) => ((Definition.SQLite.ForeignKey)fk).OnUpdate.ToString();
        protected override string GetOnDeleteActionName(BaseForeignKey fk) => ((Definition.SQLite.ForeignKey)fk).OnDelete.ToString();
    }
}
