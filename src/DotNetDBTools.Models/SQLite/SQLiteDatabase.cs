using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.SQLite;

public class SQLiteDatabase : Database
{
    public SQLiteDatabase()
    {
        Kind = DatabaseKind.SQLite;
        Tables = new List<SQLiteTable>();
        Views = new List<SQLiteView>();
        Scripts = new List<Script>();
    }
}
