namespace DotNetDBTools.Models.Core
{
    public interface IColumnInfo : IDBObjectInfo
    {
        public DataTypeInfo DataType { get; set; }
        public object DefaultValue { get; set; }
    }
}
