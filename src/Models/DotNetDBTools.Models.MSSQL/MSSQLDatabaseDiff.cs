using System.Collections.Generic;

namespace DotNetDBTools.Models.MSSQL
{
    public class MSSQLDatabaseDiff
    {
        public MSSQLDatabaseInfo NewDatabase { get; set; }
        public MSSQLDatabaseInfo OldDatabase { get; set; }

        public IEnumerable<MSSQLTableInfo> AddedTables { get; set; }
        public IEnumerable<MSSQLTableInfo> RemovedTables { get; set; }
        public IEnumerable<MSSQLTableDiff> ChangedTables { get; set; }

        public IEnumerable<MSSQLViewInfo> AddedViews { get; set; }
        public IEnumerable<MSSQLViewInfo> RemovedViews { get; set; }
        public IEnumerable<MSSQLViewDiff> ChangedViews { get; set; }

        public IEnumerable<MSSQLFunctionInfo> AddedFunctions { get; set; }
        public IEnumerable<MSSQLFunctionInfo> RemovedFunctions { get; set; }
        public IEnumerable<MSSQLFunctionDiff> ChangedFunctions { get; set; }

        public IEnumerable<MSSQLUserDefinedTypeInfo> AddedUserDefinedTypes { get; set; }
        public IEnumerable<MSSQLUserDefinedTypeInfo> RemovedUserDefinedTypes { get; set; }
        public IEnumerable<MSSQLUserDefinedTypeDiff> ChangedUserDefinedTypes { get; set; }
    }
}
