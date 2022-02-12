using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.PostgreSQL.UserDefinedTypes
{
    public class PostgreSQLDomainType : DbObject
    {
        public DataType UnderlyingType { get; set; }
        public object Default { get; set; }
        public bool Nullable { get; set; }
        public IEnumerable<CheckConstraint> CheckConstraints { get; set; }
    }
}
