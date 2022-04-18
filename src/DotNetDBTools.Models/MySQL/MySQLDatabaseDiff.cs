using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.MySQL;

public class MySQLDatabaseDiff : DatabaseDiff
{
    public List<MySQLFunction> FunctionsToCreate { get; set; }
    public List<MySQLFunction> FunctionsToDrop { get; set; }

    public List<MySQLProcedure> ProceduresToCreate { get; set; }
    public List<MySQLProcedure> ProceduresToDrop { get; set; }
}
