using System.Collections.Generic;

namespace DotNetDBTools.Models.Core
{
    public class IndexInfo : DBObjectInfo
    {
        public IEnumerable<string> Columns { get; set; }
        public IEnumerable<string> IncludeColumns { get; set; }
        public bool Unique { get; set; }
    }
}
