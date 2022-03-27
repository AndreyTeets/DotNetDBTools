using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.MSSQL.UserDefinedTypes;

public class MSSQLUserDefinedType : DbObject
{
    public DataType UnderlyingDataType { get; set; }
    public bool NotNull { get; set; }
}
