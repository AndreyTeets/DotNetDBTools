using DotNetDBTools.Models.Shared;

namespace DotNetDBTools.Models.SQLite
{
    public class SQLiteColumnInfo : BaseDBObjectInfo, IColumnInfo
    {
        public DataTypeInfo DataType { get; set; }
        public object DefaultValue { get; set; }
    }
}
