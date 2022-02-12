using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.MSSQL;

public class MSSQLColumn : Column
{
    public string DefaultConstraintName { get; set; }
}
