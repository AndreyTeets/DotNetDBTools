using DotNetDBTools.Models.Shared;

namespace DotNetDBTools.Models.MSSQL
{
    public class MSSQLColumnInfo : BaseDBObjectInfo, IColumnInfo
    {
        public DataTypeInfo DataType { get; set; }
        public object DefaultValue { get; set; }
    }
}
