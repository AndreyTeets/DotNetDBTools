using System;

namespace DotNetDBTools.Models.Core
{
    public class ColumnInfo : IDBObjectInfo
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string DataTypeName { get; set; }
        public object DefaultValue { get; set; }
        public string Length { get; set; }
        public string IsUnicode { get; set; }
        public string IsFixedLength { get; set; }
    }
}
