using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.MSSQL;

public class MSSQLColumnDiff : ColumnDiff
{
    public string DefaultToSetConstraintName { get; set; }
    public string DefaultToDropConstraintName { get; set; }
}
