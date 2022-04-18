using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.SQLite;

public class SQLiteDatabase : Database
{
    public SQLiteDatabase()
    {
        Kind = DatabaseKind.SQLite;
    }
}
