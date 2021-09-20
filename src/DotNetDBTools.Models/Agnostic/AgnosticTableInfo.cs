using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.Agnostic
{
    public class AgnosticTableInfo : BaseDBObjectInfo, ITableInfo<AgnosticColumnInfo>
    {
        public IEnumerable<IColumnInfo> Columns { get; set; }
        public IEnumerable<IForeignKeyInfo> ForeignKeys { get; set; }
    }
}
