using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.MSSQL
{
    public class MSSQLUserDefinedTypeInfo : BaseDBObjectInfo
    {
        public string Nullable { get; set; }
        public DataTypeInfo UnderlyingType { get; set; }
    }
}
