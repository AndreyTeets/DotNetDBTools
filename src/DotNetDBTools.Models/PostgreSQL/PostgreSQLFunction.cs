using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.PostgreSQL;

public class PostgreSQLFunction : DbObject
{
    public CodePiece CreateStatement { get; set; }
}
