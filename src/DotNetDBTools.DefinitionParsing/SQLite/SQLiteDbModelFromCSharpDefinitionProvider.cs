using DotNetDBTools.Definition.Core;
using DotNetDBTools.DefinitionParsing.Common;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.DefinitionParsing.SQLite;

internal class SQLiteDbModelFromCSharpDefinitionProvider : DbModelFromCSharpDefinitionProvider<
    SQLiteDatabase,
    SQLiteTable,
    SQLiteView,
    SQLiteIndex,
    SQLiteTrigger,
    Models.Core.Column>
{
    public SQLiteDbModelFromCSharpDefinitionProvider() : base(
        new SQLiteDataTypeMapper(),
        new SpecificDbmsDbObjectCodeMapper(),
        new SQLiteDefaultValueMapper())
    {
    }

    protected override void BuildAdditionalPrimaryKeyModelProperties(Models.Core.PrimaryKey pkModel, BasePrimaryKey pk, string tableName)
    {
        pkModel.Name = $"PK_{tableName}";
    }

    protected override string GetOnUpdateActionName(BaseForeignKey fk) =>
        MapFKActionNameFromDefinitionToModel(((Definition.SQLite.ForeignKey)fk).OnUpdate.ToString());
    protected override string GetOnDeleteActionName(BaseForeignKey fk) =>
        MapFKActionNameFromDefinitionToModel(((Definition.SQLite.ForeignKey)fk).OnDelete.ToString());
}
