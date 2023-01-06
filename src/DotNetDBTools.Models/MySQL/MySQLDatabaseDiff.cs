using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.MySQL;

public class MySQLDatabaseDiff : DatabaseDiff
{
    public List<MySQLFunction> FunctionsToCreate { get; set; } = new();
    public List<MySQLFunction> FunctionsToDrop { get; set; } = new();

    public List<MySQLProcedure> ProceduresToCreate { get; set; } = new();
    public List<MySQLProcedure> ProceduresToDrop { get; set; } = new();
}
