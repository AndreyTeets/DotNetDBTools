namespace DotNetDBTools.Models.Shared
{
    public interface IColumnInfo : IDBObjectInfo
    {
        public object DataType { get; set; }
        public object DefaultValue { get; set; }
    }
}
