using System.Collections.Generic;

namespace DotNetDBTools.Models.Core
{
    public abstract class DatabaseInfo
    {
        public DatabaseKind Kind { get; protected set; }
        public string Name { get; protected set; }
        public IEnumerable<TableInfo> Tables { get; set; }
        public IEnumerable<ViewInfo> Views { get; set; }
    }
}
