using System.Collections.Generic;

namespace DotNetDBTools.Models.MSSQL
{
    public class MSSQLDatabaseInfo : IDatabaseInfo<MSSQLTableInfo>
    {
        public IEnumerable<ITableInfo<IColumnInfo>> Tables { get; set; } = new List<MSSQLTableInfo>();
        public IEnumerable<IViewInfo> Views { get; set; } = new List<MSSQLViewInfo>();
        public List<MSSQLFunctionInfo> Functions { get; set; } = new();
    }
}
