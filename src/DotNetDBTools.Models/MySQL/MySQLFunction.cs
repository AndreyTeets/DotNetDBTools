using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.MySQL;

public class MySQLFunction : DbObject
{
    public CodePiece CreateStatement { get; set; }
}
