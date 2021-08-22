using System.Collections.Generic;

namespace DotNetDBTools.Models.MSSQL
{
    public class MSSQLTableDiff
    {
        public MSSQLTableInfo NewTable { get; set; }
        public MSSQLTableInfo OldTable { get; set; }

        public IEnumerable<MSSQLColumnInfo> AddedColumns { get; set; }
        public IEnumerable<MSSQLColumnInfo> RemovedColumns { get; set; }
        public IEnumerable<MSSQLColumnDiff> ChangedColumns { get; set; }

        public IEnumerable<MSSQLForeignKeyInfo> AddedForeignKeys { get; set; }
        public IEnumerable<MSSQLForeignKeyInfo> RemovedForeignKeys { get; set; }
        public IEnumerable<MSSQLForeignKeyDiff> ChangedForeignKeys { get; set; }
    }
}
