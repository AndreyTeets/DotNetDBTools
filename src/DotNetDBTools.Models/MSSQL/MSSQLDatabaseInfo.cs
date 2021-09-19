using System.Collections.Generic;
using DotNetDBTools.Models.Shared;

namespace DotNetDBTools.Models.MSSQL
{
    public class MSSQLDatabaseInfo : IDatabaseInfo<MSSQLTableInfo>
    {
        public MSSQLDatabaseInfo(string name)
        {
            Type = DatabaseType.MSSQL;
            Name = name;
        }

        public DatabaseType Type { get; private set; }
        public string Name { get; private set; }
        public IEnumerable<ITableInfo<IColumnInfo>> Tables { get; set; } = new List<MSSQLTableInfo>();
        public IEnumerable<IViewInfo> Views { get; set; } = new List<MSSQLViewInfo>();
        public List<MSSQLFunctionInfo> Functions { get; set; } = new();
        public List<MSSQLUserDefinedTypeInfo> UserDefinedTypes { get; set; } = new();
    }
}
