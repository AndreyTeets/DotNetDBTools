using System.Collections.Generic;

namespace DotNetDBTools.Models.Core
{
    public abstract class DatabaseDiff
    {
        public DatabaseInfo NewDatabase { get; set; }
        public DatabaseInfo OldDatabase { get; set; }

        public IEnumerable<TableInfo> AddedTables { get; set; }
        public IEnumerable<TableInfo> RemovedTables { get; set; }
        public IEnumerable<TableDiff> ChangedTables { get; set; }

        public IEnumerable<ViewInfo> AddedViews { get; set; }
        public IEnumerable<ViewInfo> RemovedViews { get; set; }
        public IEnumerable<ViewDiff> ChangedViews { get; set; }

        public IEnumerable<ForeignKeyInfo> AllForeignKeysToAdd { get; set; }
        public IEnumerable<ForeignKeyInfo> AllForeignKeysToDrop { get; set; }
    }
}
