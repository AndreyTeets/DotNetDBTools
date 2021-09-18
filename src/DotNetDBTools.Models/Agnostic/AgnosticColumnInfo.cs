using DotNetDBTools.Models.Common;

namespace DotNetDBTools.Models.Agnostic
{
    public class AgnosticColumnInfo : BaseDBObjectInfo, IColumnInfo
    {
        public object DataType { get; set; }
        public object DefaultValue { get; set; }
    }
}
