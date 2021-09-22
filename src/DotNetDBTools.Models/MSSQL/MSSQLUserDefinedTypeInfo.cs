using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.MSSQL
{
    public class MSSQLUserDefinedTypeInfo : BaseDBObjectInfo
    {
        public bool Nullable { get; set; }
        public DataTypeInfo UnderlyingDataType { get; set; }
    }
}
