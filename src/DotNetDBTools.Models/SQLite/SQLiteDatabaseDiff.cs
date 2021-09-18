using System.Collections.Generic;

namespace DotNetDBTools.Models.SQLite
{
    public class SQLiteDatabaseDiff
    {
        public SQLiteDatabaseInfo NewDatabase { get; set; }
        public SQLiteDatabaseInfo OldDatabase { get; set; }

        public IEnumerable<SQLiteTableInfo> AddedTables { get; set; }
        public IEnumerable<SQLiteTableInfo> RemovedTables { get; set; }
        public IEnumerable<SQLiteTableDiff> ChangedTables { get; set; }

        public IEnumerable<SQLiteViewInfo> AddedViews { get; set; }
        public IEnumerable<SQLiteViewInfo> RemovedViews { get; set; }
        public IEnumerable<SQLiteViewDiff> ChangedViews { get; set; }
    }
}
