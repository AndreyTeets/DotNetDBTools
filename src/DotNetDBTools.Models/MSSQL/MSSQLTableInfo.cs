using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.MSSQL
{
    public class MSSQLTableInfo : BaseDBObjectInfo, ITableInfo<MSSQLColumnInfo>
    {
        public IEnumerable<IColumnInfo> Columns { get; set; }
        public IEnumerable<IForeignKeyInfo> ForeignKeys { get; set; }
    }
}
