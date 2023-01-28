using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.PostgreSQL;

public class PostgreSQLIndex : Index
{
    public string Method { get; set; }
    public CodePiece Expression { get; set; }
}
