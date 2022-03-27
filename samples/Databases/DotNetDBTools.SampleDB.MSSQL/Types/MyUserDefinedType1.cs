using System;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.Core.CSharpDataTypes;
using DotNetDBTools.Definition.MSSQL.UserDefinedTypes;

namespace DotNetDBTools.SampleDB.MSSQL.Types
{
    public class MyUserDefinedType1 : IUserDefinedType
    {
        public Guid ID => new("0CD1E71C-CC9C-440F-AC0B-81A1D6F7DDAA");
        public IDataType UnderlyingType => new StringDataType() { Length = 100 };
        public bool NotNull => false;
    }
}
