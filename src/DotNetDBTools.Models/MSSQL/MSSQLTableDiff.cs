using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.MSSQL
{
    public class MSSQLTableDiff
    {
        public MSSQLTableInfo NewTable { get; set; }
        public MSSQLTableInfo OldTable { get; set; }

        public IEnumerable<ColumnInfo> AddedColumns { get; set; }
        public IEnumerable<ColumnInfo> RemovedColumns { get; set; }
        public IEnumerable<ColumnDiff> ChangedColumns { get; set; }

        public IEnumerable<MSSQLForeignKeyInfo> AddedForeignKeys { get; set; }
        public IEnumerable<MSSQLForeignKeyInfo> RemovedForeignKeys { get; set; }
        public IEnumerable<MSSQLForeignKeyDiff> ChangedForeignKeys { get; set; }
    }
}
