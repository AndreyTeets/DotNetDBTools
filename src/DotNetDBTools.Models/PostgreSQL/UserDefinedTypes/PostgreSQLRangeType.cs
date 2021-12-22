using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.PostgreSQL.UserDefinedTypes
{
    public class PostgreSQLRangeType : DBObject
    {
        public DataType Subtype { get; set; }
        public string SubtypeOperatorClass { get; set; }
        public string Collation { get; set; }
        public string CanonicalFunction { get; set; }
        public string SubtypeDiff { get; set; }
        public string MultirangeTypeName { get; set; }
    }
}
