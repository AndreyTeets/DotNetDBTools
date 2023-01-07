using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.PostgreSQL;

public class PostgreSQLFunction : DbObject
{
    public CodePiece CreateStatement { get; set; }

    /// <summary>
    /// Meaning it does not transitively depend on tables.
    /// </summary>
    public bool IsSimple { get; set; }
}
