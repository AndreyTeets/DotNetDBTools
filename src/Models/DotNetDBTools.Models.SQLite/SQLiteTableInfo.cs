using System.Collections.Generic;

namespace DotNetDBTools.Models.SQLite
{
    public class SQLiteTableInfo : BaseDBObjectInfo, ITableInfo<SQLiteColumnInfo>
    {
        public IEnumerable<IColumnInfo> Columns { get; set; }
        public IEnumerable<IForeignKeyInfo> ForeignKeys { get; set; }
    }
}
