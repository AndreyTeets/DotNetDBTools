using System.Collections.Generic;
using DotNetDBTools.Models.Common;

namespace DotNetDBTools.Models.SQLite
{
    public class SQLiteDatabaseInfo : IDatabaseInfo<SQLiteTableInfo>
    {
        public SQLiteDatabaseInfo(string name)
        {
            Type = DatabaseType.SQLite;
            Name = name;
        }

        public DatabaseType Type { get; private set; }
        public string Name { get; private set; }
        public IEnumerable<ITableInfo<IColumnInfo>> Tables { get; set; } = new List<SQLiteTableInfo>();
        public IEnumerable<IViewInfo> Views { get; set; } = new List<SQLiteViewInfo>();
    }
}
