using System;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.Core.CSharpDataTypes;
using DotNetDBTools.Definition.PostgreSQL.UserDefinedTypes;

namespace DotNetDBTools.SampleDB.PostgreSQL.Types
{
    public class MyRangeType1 : IRangeType
    {
        public Guid ID => new("B02DB666-FBBC-4CD7-A14D-4049251B9A7B");
        public IDataType Subtype => new DateTimeDataType() { IsWithTimeZone = true };
        public string SubtypeOperatorClass => null;
        public string Collation => null;
        public string CanonicalFunction => null;
        public string SubtypeDiff => null;
        public string MultirangeTypeName => null;
    }
}
