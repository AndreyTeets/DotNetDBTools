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
            UserDefinedTypes = new List<MSSQLUserDefinedTypeInfo>();
            UserDefinedTableTypes = new List<MSSQLUserDefinedTableTypeInfo>();
            Functions = new List<MSSQLFunctionInfo>();
            Procedures = new List<MSSQLProcedureInfo>();
        }
        public IEnumerable<MSSQLUserDefinedTypeInfo> UserDefinedTypes { get; set; }
        public IEnumerable<MSSQLUserDefinedTableTypeInfo> UserDefinedTableTypes { get; set; }
        public IEnumerable<MSSQLFunctionInfo> Functions { get; set; }
        public IEnumerable<MSSQLProcedureInfo> Procedures { get; set; }
    }
}
