namespace DotNetDBTools.Models
{
    public interface IColumnInfo : IDBObjectInfo
    {
        public object DataType { get; set; }
        public object DefaultValue { get; set; }
    }
}
