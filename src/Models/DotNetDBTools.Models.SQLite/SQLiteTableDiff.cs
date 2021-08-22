using System.Collections.Generic;

namespace DotNetDBTools.Models.SQLite
{
    public class SQLiteTableDiff
    {
        public SQLiteTableInfo NewTable { get; set; }
        public SQLiteTableInfo OldTable { get; set; }

        public IEnumerable<SQLiteColumnInfo> AddedColumns { get; set; }
        public IEnumerable<SQLiteColumnInfo> RemovedColumns { get; set; }
        public IEnumerable<SQLiteColumnDiff> ChangedColumns { get; set; }

        public IEnumerable<SQLiteForeignKeyInfo> AddedForeignKeys { get; set; }
        public IEnumerable<SQLiteForeignKeyInfo> RemovedForeignKeys { get; set; }
        public IEnumerable<SQLiteForeignKeyDiff> ChangedForeignKeys { get; set; }
    }
}
