using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;

public class PostgreSQLEnumType : DbObject
{
    public IEnumerable<string> AllowedValues { get; set; }
}
