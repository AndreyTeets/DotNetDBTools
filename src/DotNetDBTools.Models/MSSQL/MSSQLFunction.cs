using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.MSSQL;

public class MSSQLFunction : DbObject
{
    public CodePiece CreateStatement { get; set; }
}
