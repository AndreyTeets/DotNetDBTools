using System.Collections.Generic;

namespace DotNetDBTools.Models.Core
{
    public class UniqueConstraint : DBObject
    {
        public IEnumerable<string> Columns { get; set; }
    }
}
