using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.SQLite
{
    public class SQLiteDatabase : Database
    {
        public SQLiteDatabase()
            : this(null) { }

        public SQLiteDatabase(string name)
        {
            Kind = DatabaseKind.SQLite;
            Name = name;
            Tables = new List<SQLiteTable>();
            Views = new List<SQLiteView>();
        }
    }
}
