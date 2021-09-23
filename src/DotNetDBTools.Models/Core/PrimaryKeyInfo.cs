using System.Collections.Generic;

namespace DotNetDBTools.Models.Core
{
    public class PrimaryKeyInfo : DBObjectInfo
    {
        public IEnumerable<string> Columns { get; set; }
    }
}
