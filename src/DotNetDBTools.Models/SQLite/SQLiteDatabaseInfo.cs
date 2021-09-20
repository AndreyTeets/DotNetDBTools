using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.SQLite
{
    public class SQLiteDatabaseInfo : IDatabaseInfo<SQLiteTableInfo>
    {
        public SQLiteDatabaseInfo(string name)
        {
            Kind = DatabaseKind.SQLite;
            Name = name;
        }

        public DatabaseKind Kind { get; private set; }
        public string Name { get; private set; }
        public IEnumerable<ITableInfo> Tables { get; set; } = new List<SQLiteTableInfo>();
        public IEnumerable<IViewInfo> Views { get; set; } = new List<SQLiteViewInfo>();
    }
}
