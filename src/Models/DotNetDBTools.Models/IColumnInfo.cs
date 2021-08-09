namespace DotNetDBTools.Models
{
    public interface IColumnInfo : IDBObjectInfo
    {
        public string DataType { get; set; }
        public object DefaultValue { get; set; }
    }
}
