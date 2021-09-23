using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.SQLite
{
    public class SQLiteTableDiff
    {
        public SQLiteTableInfo NewTable { get; set; }
        public SQLiteTableInfo OldTable { get; set; }

        public IEnumerable<ColumnInfo> AddedColumns { get; set; }
        public IEnumerable<ColumnInfo> RemovedColumns { get; set; }
        public IEnumerable<ColumnDiff> ChangedColumns { get; set; }

        public IEnumerable<ForeignKeyInfo> AddedForeignKeys { get; set; }
        public IEnumerable<ForeignKeyInfo> RemovedForeignKeys { get; set; }
    }
}
