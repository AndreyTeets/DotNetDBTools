using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Analysis.SQLite;

public class SQLiteDbModelConverter : DbModelConverter<
    SQLiteDatabase,
    SQLiteTable,
    SQLiteView,
    Column>
{
    public SQLiteDbModelConverter() : base(
        DatabaseKind.SQLite,
        new SQLiteDataTypeConverter(),
        new SQLiteDefaultValueConverter(),
        new SQLiteDbModelPostProcessor())
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
