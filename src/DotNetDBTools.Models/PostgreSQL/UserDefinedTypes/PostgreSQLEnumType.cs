using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.PostgreSQL.UserDefinedTypes
{
    public class PostgreSQLEnumType : DBObject
    {
        public IEnumerable<string> AllowedValues { get; set; }
    }
}
