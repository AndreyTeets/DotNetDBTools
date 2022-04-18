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

    public List<MSSQLUserDefinedType> UserDefinedTypes { get; set; }
    public List<MSSQLUserDefinedTableType> UserDefinedTableTypes { get; set; }
    public List<MSSQLFunction> Functions { get; set; }
    public List<MSSQLProcedure> Procedures { get; set; }

    public override void InitializeAdditionalProperties()
    {
        UserDefinedTypes = new();
        UserDefinedTableTypes = new();
        Functions = new();
        Procedures = new();
    }
}
