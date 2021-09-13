namespace DotNetDBTools.Models.MSSQL
{
    public class MSSQLColumnInfo : BaseDBObjectInfo, IColumnInfo
    {
        public object DataType { get; set; }
        public object DefaultValue { get; set; }
    }
}
