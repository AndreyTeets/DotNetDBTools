namespace DotNetDBTools.Models.MSSQL
{
    public class MSSQLColumnInfo : BaseDBObjectInfo, IColumnInfo
    {
        public string DataType { get; set; }
        public object DefaultValue { get; set; }
    }
}
