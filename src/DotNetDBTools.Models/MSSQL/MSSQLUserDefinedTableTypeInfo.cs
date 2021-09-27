using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.MSSQL
{
    public class MSSQLUserDefinedTableTypeInfo : DBObjectInfo
    {
        public IEnumerable<ColumnInfo> Columns { get; set; }
        public PrimaryKeyInfo PrimaryKey { get; set; }
        public IEnumerable<UniqueConstraintInfo> UniqueConstraints { get; set; }
        public IEnumerable<ForeignKeyInfo> ForeignKeys { get; set; }
    }
}
