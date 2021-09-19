using DotNetDBTools.Models.Shared;

namespace DotNetDBTools.Models.Agnostic
{
    public class AgnosticColumnInfo : BaseDBObjectInfo, IColumnInfo
    {
        public DataTypeInfo DataType { get; set; }
        public object DefaultValue { get; set; }
    }
}
