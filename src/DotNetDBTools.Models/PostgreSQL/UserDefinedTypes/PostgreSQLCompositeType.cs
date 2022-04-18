using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;

public class PostgreSQLCompositeType : DbObject
{
    public List<PostgreSQLCompositeTypeAttribute> Attributes { get; set; }
}

public class PostgreSQLCompositeTypeAttribute
{
    public string Name { get; set; }
    public DataType DataType { get; set; }
}
