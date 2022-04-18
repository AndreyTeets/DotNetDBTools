using System.Collections.Generic;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL.UserDefinedTypes;

namespace DotNetDBTools.Models.MSSQL;

public class MSSQLDatabaseDiff : DatabaseDiff
{
    public List<MSSQLUserDefinedType> AddedUserDefinedTypes { get; set; }
    public List<MSSQLUserDefinedType> RemovedUserDefinedTypes { get; set; }
    public List<MSSQLUserDefinedTypeDiff> ChangedUserDefinedTypes { get; set; }

    public List<MSSQLUserDefinedTableType> UserDefinedTableTypesToCreate { get; set; }
    public List<MSSQLUserDefinedTableType> UserDefinedTableTypesToDrop { get; set; }

    public List<MSSQLFunction> FunctionsToCreate { get; set; }
    public List<MSSQLFunction> FunctionsToDrop { get; set; }

    public List<MSSQLProcedure> ProceduresToCreate { get; set; }
    public List<MSSQLProcedure> ProceduresToDrop { get; set; }
}
