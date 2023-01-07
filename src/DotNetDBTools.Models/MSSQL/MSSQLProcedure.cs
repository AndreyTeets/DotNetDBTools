using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.MSSQL;

public class MSSQLProcedure : DbObject
{
    public CodePiece CreateStatement { get; set; }
}
