namespace DotNetDBTools.Models.Agnostic
{
    public class AgnosticColumnInfo : BaseDBObjectInfo, IColumnInfo
    {
        public string DataType { get; set; }
        public object DefaultValue { get; set; }
    }
}
