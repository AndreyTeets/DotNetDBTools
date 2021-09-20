using System.Collections.Generic;

namespace DotNetDBTools.Models.Core
{
    public interface IDatabaseInfo<out TableInfo>
        where TableInfo : ITableInfo
    {
        public DatabaseKind Kind { get; }
        public string Name { get; }
        public IEnumerable<ITableInfo> Tables { get; set; }
        public IEnumerable<IViewInfo> Views { get; set; }
    }
}
