﻿using System.Collections.Generic;

namespace DotNetDBTools.Models.Common
{
    public interface IDatabaseInfo<out TableInfo>
        where TableInfo : ITableInfo<IColumnInfo>
    {
        public DatabaseType Type { get; }
        public string Name { get; }
        public IEnumerable<ITableInfo<IColumnInfo>> Tables { get; set; }
        public IEnumerable<IViewInfo> Views { get; set; }
    }
}
