using System.Collections.Generic;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL.UserDefinedTypes;

namespace DotNetDBTools.Models.MSSQL;

public class MSSQLDatabaseDiff : DatabaseDiff
{
    public List<MSSQLUserDefinedType> AddedUserDefinedTypes { get; set; } = new();
    public List<MSSQLUserDefinedType> RemovedUserDefinedTypes { get; set; } = new();
    public List<MSSQLUserDefinedTypeDiff> ChangedUserDefinedTypes { get; set; } = new();

    public List<MSSQLUserDefinedTableType> UserDefinedTableTypesToCreate { get; set; } = new();
    public List<MSSQLUserDefinedTableType> UserDefinedTableTypesToDrop { get; set; } = new();

    public List<MSSQLFunction> FunctionsToCreate { get; set; } = new();
    public List<MSSQLFunction> FunctionsToDrop { get; set; } = new();

    public List<MSSQLProcedure> ProceduresToCreate { get; set; } = new();
    public List<MSSQLProcedure> ProceduresToDrop { get; set; } = new();
}
