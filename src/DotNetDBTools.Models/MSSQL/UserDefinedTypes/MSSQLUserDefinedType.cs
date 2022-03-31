using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.MSSQL.UserDefinedTypes;

public class MSSQLUserDefinedType : DbObject
{
    public DataType UnderlyingType { get; set; }
    public bool NotNull { get; set; }
}
