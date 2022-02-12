using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.PostgreSQL
{
    public class PostgreSQLFunction : DbObject
    {
        public CodePiece CodePiece { get; set; }
        public bool IsSimple { get; set; }
    }
}
