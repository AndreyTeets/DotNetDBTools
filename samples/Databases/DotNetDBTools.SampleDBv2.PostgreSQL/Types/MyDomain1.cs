using System;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.Core.CSharpDataTypes;
using DotNetDBTools.Definition.PostgreSQL;
using DotNetDBTools.Definition.PostgreSQL.UserDefinedTypes;

namespace DotNetDBTools.SampleDB.PostgreSQL.Types
{
    public class MyDomain1 : IDomain
    {
        public Guid DNDBT_OBJECT_ID => new("A28BCB6C-3CBC-467E-A52C-AC740C98A537");
        public IDataType UnderlyingType => new StringDataType() { Length = 111 };
        public bool NotNull => false;
        public IDefaultValue Default => null;

        public CheckConstraint MyDomain1_CK1 = new("7A053CEE-ABCC-4993-8EEA-12B87C5194E6")
        {
            Code = "CHECK (value = lower(value))",
        };
        public CheckConstraint MyDomain1_CK2 = new("7905B7A8-CF45-4328-8A2B-00616D98235E")
        {
            Code = "CHECK (char_length(value) > 3)",
        };
    }
}
