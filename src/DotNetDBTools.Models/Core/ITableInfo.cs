using System.Collections.Generic;

namespace DotNetDBTools.Models.Core
{
    public interface ITableInfo : IDBObjectInfo
    {
        public IEnumerable<ColumnInfo> Columns { get; set; }
        public IEnumerable<IForeignKeyInfo> ForeignKeys { get; set; }
    }
}
