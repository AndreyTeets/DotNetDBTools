using System;

namespace DotNetDBTools.Models.Core
{
    public class ColumnInfo : IDBObjectInfo
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public DataTypeInfo DataType { get; set; }
        public bool Nullable { get; set; }
        public bool Unique { get; set; }
        public bool Identity { get; set; }
        public object Default { get; set; }
    }
}
