using System.Collections.Generic;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL.UserDefinedTypes;

namespace DotNetDBTools.Models.MSSQL;

public class MSSQLDatabase : Database
{
    public MSSQLDatabase()
    {
        Kind = DatabaseKind.MSSQL;
        Tables = new List<MSSQLTable>();
        Views = new List<MSSQLView>();
        Scripts = new List<Script>();
        UserDefinedTypes = new List<MSSQLUserDefinedType>();
        UserDefinedTableTypes = new List<MSSQLUserDefinedTableType>();
        Functions = new List<MSSQLFunction>();
        Procedures = new List<MSSQLProcedure>();
    }

    public IEnumerable<MSSQLUserDefinedType> UserDefinedTypes { get; set; }
    public IEnumerable<MSSQLUserDefinedTableType> UserDefinedTableTypes { get; set; }
    public IEnumerable<MSSQLFunction> Functions { get; set; }
    public IEnumerable<MSSQLProcedure> Procedures { get; set; }
}
