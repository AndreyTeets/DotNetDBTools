using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.PostgreSQL
{
    public class PostgreSQLDatabaseDiff : DatabaseDiff
    {
        public IEnumerable<PostgreSQLFunction> FunctionsToCreate { get; set; }
        public IEnumerable<PostgreSQLFunction> FunctionsToDrop { get; set; }

        public IEnumerable<PostgreSQLProcedure> ProceduresToCreate { get; set; }
        public IEnumerable<PostgreSQLProcedure> ProceduresToDrop { get; set; }
    }
}
