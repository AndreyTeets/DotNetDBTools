using DotNetDBTools.CodeParsing;
using DotNetDBTools.CodeParsing.Models;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.DefinitionParsing.SQLite;

internal class SQLiteDbModelFromSqlDefinitionProvider : DbModelFromSqlDefinitionProvider<
    SQLiteDatabase,
    SQLiteTable,
    SQLiteView,
    Column>
{
    public SQLiteDbModelFromSqlDefinitionProvider() : base(
        new SQLiteCodeParser())
    {
    }

    protected override void BuildAdditionalPrimaryKeyModelProperties(PrimaryKey pkModel, ConstraintInfo pk, string tableName)
    {
        pkModel.Name = $"PK_{tableName}";
    }
}
