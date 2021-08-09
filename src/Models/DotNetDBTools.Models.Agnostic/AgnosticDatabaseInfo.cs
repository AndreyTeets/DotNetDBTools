using System.Collections.Generic;

namespace DotNetDBTools.Models.Agnostic
{
    public class AgnosticDatabaseInfo : IDatabaseInfo<AgnosticTableInfo>
    {
        public IEnumerable<ITableInfo<IColumnInfo>> Tables { get; set; } = new List<AgnosticTableInfo>();
        public IEnumerable<IViewInfo> Views { get; set; } = new List<AgnosticViewInfo>();
    }
}
