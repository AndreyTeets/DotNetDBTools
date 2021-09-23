using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.MSSQL
{
    public class MSSQLDatabaseInfo : DatabaseInfo
    {
        public MSSQLDatabaseInfo(string name)
        {
            Kind = DatabaseKind.MSSQL;
            Name = name;
            Tables = new List<MSSQLTableInfo>();
            Views = new List<MSSQLViewInfo>();
        }
        public List<MSSQLFunctionInfo> Functions { get; set; } = new();
        public List<MSSQLUserDefinedTypeInfo> UserDefinedTypes { get; set; } = new();
    }
}
