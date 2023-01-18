using System;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.PostgreSQL;
using DotNetDBTools.Definition.PostgreSQL.UserDefinedTypes;

namespace DotNetDBTools.SampleDB.PostgreSQL.Types
{
    public class MyDomain2 : IDomain
    {
        public Guid DNDBT_OBJECT_ID => new("2200D040-A892-43B5-9B5E-DB9F6458187F");
        public IDataType UnderlyingType => new MyCompositeType1();
        public bool NotNull => true;
        public IDefaultValue Default => new VerbatimDefaultValue(@"'(""some string"", ""{42.78, -4, 0}"")'");
    }
}
