using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.SQLite
{
    public class SQLiteDatabaseInfo : DatabaseInfo
    {
        public SQLiteDatabaseInfo(string name)
        {
            Kind = DatabaseKind.SQLite;
            Name = name;
            Tables = new List<SQLiteTableInfo>();
            Views = new List<SQLiteViewInfo>();
        }
    }
}
