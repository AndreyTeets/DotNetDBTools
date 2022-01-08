using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.PostgreSQL
{
    public class PostgreSQLFunction : DBObject
    {
        public CodePiece CodePiece { get; set; }
        public List<DBObject> Dependencies { get; set; }
    }
}
