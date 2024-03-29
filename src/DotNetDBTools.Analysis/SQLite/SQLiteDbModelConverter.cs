﻿using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Analysis.SQLite;

internal class SQLiteDbModelConverter : DbModelConverter<
    SQLiteDatabase,
    SQLiteTable,
    SQLiteView,
    SQLiteIndex,
    SQLiteTrigger,
    Column>
{
    public SQLiteDbModelConverter() : base(
        DatabaseKind.SQLite,
        new SQLiteDataTypeConverter(),
        new SQLiteDefaultValueConverter(),
        new SQLiteDependenciesBuilder(),
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
