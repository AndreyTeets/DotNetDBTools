using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.PostgreSQL.UserDefinedTypes
{
    public interface IRangeType : IDbObject, IDataType
    {
        public IDataType Subtype { get; }
        public string SubtypeOperatorClass { get; }
        public string Collation { get; }
        public string CanonicalFunction { get; }
        public string SubtypeDiff { get; }
        public string MultirangeTypeName { get; }
    }
}
