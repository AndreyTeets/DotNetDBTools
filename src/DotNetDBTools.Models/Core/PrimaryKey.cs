using System.Collections.Generic;

namespace DotNetDBTools.Models.Core
{
    public class PrimaryKey : DBObject
    {
        public IEnumerable<string> Columns { get; set; }
    }
}
