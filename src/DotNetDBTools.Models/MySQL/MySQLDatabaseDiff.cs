using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.MySQL;

public class MySQLDatabaseDiff : DatabaseDiff
{
    public IEnumerable<MySQLFunction> FunctionsToCreate { get; set; }
    public IEnumerable<MySQLFunction> FunctionsToDrop { get; set; }

    public IEnumerable<MySQLProcedure> ProceduresToCreate { get; set; }
    public IEnumerable<MySQLProcedure> ProceduresToDrop { get; set; }
}
