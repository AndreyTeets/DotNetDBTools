using System.Collections.Generic;

namespace DotNetDBTools.Models.Core
{
    public abstract class DatabaseDiff
    {
        public Database NewDatabase { get; set; }
        public Database OldDatabase { get; set; }

        public IEnumerable<Table> AddedTables { get; set; }
        public IEnumerable<Table> RemovedTables { get; set; }
        public IEnumerable<TableDiff> ChangedTables { get; set; }

        public IEnumerable<View> ViewsToCreate { get; set; }
        public IEnumerable<View> ViewsToDrop { get; set; }

        public IEnumerable<ForeignKey> AllForeignKeysToCreate { get; set; }
        public IEnumerable<ForeignKey> AllForeignKeysToDrop { get; set; }
    }
}
