using System.Collections.Generic;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;

namespace DotNetDBTools.Models.PostgreSQL
{
    public class PostgreSQLDatabase : Database
    {
        public PostgreSQLDatabase()
            : this(null) { }

        public PostgreSQLDatabase(string name)
        {
            Kind = DatabaseKind.PostgreSQL;
            Name = name;
            Tables = new List<PostgreSQLTable>();
            Views = new List<PostgreSQLView>();
            CompositeTypes = new List<PostgreSQLCompositeType>();
            DomainTypes = new List<PostgreSQLDomainType>();
            EnumTypes = new List<PostgreSQLEnumType>();
            RangeTypes = new List<PostgreSQLRangeType>();
            Functions = new List<PostgreSQLFunction>();
            Procedures = new List<PostgreSQLProcedure>();
        }

        public IEnumerable<PostgreSQLCompositeType> CompositeTypes { get; set; }
        public IEnumerable<PostgreSQLDomainType> DomainTypes { get; set; }
        public IEnumerable<PostgreSQLEnumType> EnumTypes { get; set; }
        public IEnumerable<PostgreSQLRangeType> RangeTypes { get; set; }

        public IEnumerable<PostgreSQLFunction> Functions { get; set; }
        public IEnumerable<PostgreSQLProcedure> Procedures { get; set; }
    }
}
