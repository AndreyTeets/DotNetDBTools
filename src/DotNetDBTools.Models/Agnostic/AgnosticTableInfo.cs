using System.Collections.Generic;
using DotNetDBTools.Models.Common;

namespace DotNetDBTools.Models.Agnostic
{
    public class AgnosticTableInfo : BaseDBObjectInfo, ITableInfo<AgnosticColumnInfo>
    {
        public IEnumerable<IColumnInfo> Columns { get; set; }
        public IEnumerable<IForeignKeyInfo> ForeignKeys { get; set; }
    }
}
