using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.MSSQL
{
    public class MSSQLUserDefinedDataType : DataType
    {
        public DataType UnderlyingType { get; set; }
    }
}
