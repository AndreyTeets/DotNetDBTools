using System.Collections.Generic;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL.UserDefinedTypes;

namespace DotNetDBTools.Models.MSSQL;

public class MSSQLDatabase : Database
{
    public MSSQLDatabase()
    {
        Kind = DatabaseKind.MSSQL;
    }

    public List<MSSQLUserDefinedType> UserDefinedTypes { get; set; } = new();
    public List<MSSQLUserDefinedTableType> UserDefinedTableTypes { get; set; } = new();
    public List<MSSQLFunction> Functions { get; set; } = new();
    public List<MSSQLProcedure> Procedures { get; set; } = new();
}
