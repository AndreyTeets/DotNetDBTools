using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.MSSQL
{
    public class MSSQLUserDefinedTypeInfo : BaseDBObjectInfo
    {
        public string Nullable { get; set; }
        public string UnderlyingDataTypeName { get; set; }
        public string UnderlyingDataTypeLength { get; set; }
        public string UnderlyingDataTypeIsUnicode { get; set; }
        public string UnderlyingDataTypeIsFixedLength { get; set; }
    }
}
