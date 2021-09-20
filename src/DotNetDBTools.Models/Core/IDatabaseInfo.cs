using System.Collections.Generic;

namespace DotNetDBTools.Models.Core
{
    public interface IDatabaseInfo<out TableInfo>
        where TableInfo : ITableInfo<IColumnInfo>
    {
        public DatabaseKind Kind { get; }
        public string Name { get; }
        public IEnumerable<ITableInfo<IColumnInfo>> Tables { get; set; }
        public IEnumerable<IViewInfo> Views { get; set; }
    }
}
