﻿using System.Collections.Generic;

namespace DotNetDBTools.Models
{
    public interface IDatabaseInfo<out TableInfo>
        where TableInfo : ITableInfo<IColumnInfo>
    {
        public IEnumerable<ITableInfo<IColumnInfo>> Tables { get; set; }
        public IEnumerable<IViewInfo> Views { get; set; }
    }
}
