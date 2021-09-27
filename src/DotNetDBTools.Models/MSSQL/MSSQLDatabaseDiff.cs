using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.MSSQL
{
    public class MSSQLDatabaseDiff : DatabaseDiff
    {
        public IEnumerable<MSSQLUserDefinedTypeInfo> AddedUserDefinedTypes { get; set; }
        public IEnumerable<MSSQLUserDefinedTypeInfo> RemovedUserDefinedTypes { get; set; }
        public IEnumerable<MSSQLUserDefinedTypeDiff> ChangedUserDefinedTypes { get; set; }

        public IEnumerable<MSSQLUserDefinedTableTypeInfo> UserDefinedTableTypesToCreate { get; set; }
        public IEnumerable<MSSQLUserDefinedTableTypeInfo> UserDefinedTableTypesToDrop { get; set; }

        public IEnumerable<MSSQLFunctionInfo> FunctionsToCreate { get; set; }
        public IEnumerable<MSSQLFunctionInfo> FunctionsToDrop { get; set; }

        public IEnumerable<MSSQLProcedureInfo> ProceduresToCreate { get; set; }
        public IEnumerable<MSSQLProcedureInfo> ProceduresToDrop { get; set; }
    }
}
