using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.SQLite
{
    public class ForeignKey : BaseForeignKey
    {
        public ForeignKeyActions OnUpdate { get; set; }
        public ForeignKeyActions OnDelete { get; set; }
    }
}
