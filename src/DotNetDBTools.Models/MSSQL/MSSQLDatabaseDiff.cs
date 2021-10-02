using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.MSSQL
{
    public class MSSQLDatabaseDiff : DatabaseDiff
    {
        public IEnumerable<MSSQLUserDefinedType> AddedUserDefinedTypes { get; set; }
        public IEnumerable<MSSQLUserDefinedType> RemovedUserDefinedTypes { get; set; }
        public IEnumerable<MSSQLUserDefinedTypeDiff> ChangedUserDefinedTypes { get; set; }

        public IEnumerable<MSSQLUserDefinedTableType> UserDefinedTableTypesToCreate { get; set; }
        public IEnumerable<MSSQLUserDefinedTableType> UserDefinedTableTypesToDrop { get; set; }

        public IEnumerable<MSSQLFunction> FunctionsToCreate { get; set; }
        public IEnumerable<MSSQLFunction> FunctionsToDrop { get; set; }

        public IEnumerable<MSSQLProcedure> ProceduresToCreate { get; set; }
        public IEnumerable<MSSQLProcedure> ProceduresToDrop { get; set; }
    }
}
