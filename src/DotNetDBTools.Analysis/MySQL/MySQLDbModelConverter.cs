using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MySQL;

namespace DotNetDBTools.Analysis.MySQL;

internal class MySQLDbModelConverter : DbModelConverter<
    MySQLDatabase,
    MySQLTable,
    MySQLView,
    MySQLIndex,
    MySQLTrigger,
    Column>
{
    public MySQLDbModelConverter() : base(
        DatabaseKind.MySQL,
        new MySQLDataTypeConverter(),
        new MySQLDefaultValueConverter(),
        new MySQLDependenciesBuilder(),
        new MySQLDbModelPostProcessor())
    {
    }

    protected override PrimaryKey ConvertPrimaryKey(PrimaryKey pk, string tableName)
    {
        if (pk is null)
            return null;
        return new()
        {
            ID = pk.ID,
            Name = $"PK_{tableName}",
            Columns = pk.Columns,
        };
    }
}
