using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.PostgreSQL;

public class PostgreSQLIndex : Index
{
    public CodePiece Expression { get; set; }
}
