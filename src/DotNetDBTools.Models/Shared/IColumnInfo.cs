namespace DotNetDBTools.Models.Shared
{
    public interface IColumnInfo : IDBObjectInfo
    {
        public DataTypeInfo DataType { get; set; }
        public object DefaultValue { get; set; }
    }
}
