using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.MSSQL
{
    public class MSSQLDatabaseDiff : DatabaseDiff
    {
        public IEnumerable<MSSQLFunctionInfo> AddedFunctions { get; set; }
        public IEnumerable<MSSQLFunctionInfo> RemovedFunctions { get; set; }
        public IEnumerable<MSSQLFunctionDiff> ChangedFunctions { get; set; }

        public IEnumerable<MSSQLUserDefinedTypeInfo> AddedUserDefinedTypes { get; set; }
        public IEnumerable<MSSQLUserDefinedTypeInfo> RemovedUserDefinedTypes { get; set; }
        public IEnumerable<MSSQLUserDefinedTypeDiff> ChangedUserDefinedTypes { get; set; }
    }
}
