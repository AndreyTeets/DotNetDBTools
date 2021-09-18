using DotNetDBTools.Models.Common;

namespace DotNetDBTools.Models.SQLite
{
    public class SQLiteColumnInfo : BaseDBObjectInfo, IColumnInfo
    {
        public object DataType { get; set; }
        public object DefaultValue { get; set; }
    }
}
