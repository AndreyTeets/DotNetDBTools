using System.Collections.Generic;

namespace DotNetDBTools.Models.Core
{
    public class UniqueConstraintInfo : DBObjectInfo
    {
        public IEnumerable<string> Columns { get; set; }
    }
}
