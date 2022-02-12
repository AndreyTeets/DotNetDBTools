using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.MSSQL
{
    public class MSSQLUserDefinedType : DbObject
    {
        public bool Nullable { get; set; }
        public DataType UnderlyingDataType { get; set; }
    }
}
