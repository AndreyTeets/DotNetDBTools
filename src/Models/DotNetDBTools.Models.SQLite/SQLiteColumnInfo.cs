namespace DotNetDBTools.Models.SQLite
{
    public class SQLiteColumnInfo : BaseDBObjectInfo, IColumnInfo
    {
        public string DataType { get; set; }
        public object DefaultValue { get; set; }
    }
}
