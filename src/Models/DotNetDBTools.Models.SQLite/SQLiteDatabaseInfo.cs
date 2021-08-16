using System.Collections.Generic;

namespace DotNetDBTools.Models.SQLite
{
    public class SQLiteDatabaseInfo : IDatabaseInfo<SQLiteTableInfo>
    {
        public IEnumerable<ITableInfo<IColumnInfo>> Tables { get; set; } = new List<SQLiteTableInfo>();
        public IEnumerable<IViewInfo> Views { get; set; } = new List<SQLiteViewInfo>();
    }
}
