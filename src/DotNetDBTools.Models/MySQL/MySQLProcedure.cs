using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.MySQL;

public class MySQLProcedure : DbObject
{
    public CodePiece CreateStatement { get; set; }
}
