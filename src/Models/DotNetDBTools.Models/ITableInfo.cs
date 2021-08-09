﻿using System.Collections.Generic;

namespace DotNetDBTools.Models
{
    public interface ITableInfo<out ColumnInfo> : IDBObjectInfo
        where ColumnInfo : IColumnInfo
    {
        public IEnumerable<IColumnInfo> Columns { get; set; }
        public IEnumerable<IForeignKeyInfo> ForeignKeys { get; set; }
    }
}
