using System.Collections.Generic;

namespace DotNetDBTools.Models.MSSQL
{
    public class MSSQLTableInfo : BaseDBObjectInfo, ITableInfo<MSSQLColumnInfo>
    {
        public IEnumerable<IColumnInfo> Columns { get; set; }
        public IEnumerable<IForeignKeyInfo> ForeignKeys { get; set; }
    }
}
