using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.MSSQL
{
    public class MSSQLUserDefinedTypeInfo : BaseDBObjectInfo
    {
        public string Nullable { get; set; }
        public string UnderlyingDataTypeName { get; set; }
    }
}
